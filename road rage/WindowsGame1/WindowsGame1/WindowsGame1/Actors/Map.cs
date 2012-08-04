using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Actors ;

namespace TangoGames.RoadFighter.Actors
{
    /// <summary>
    /// Essa é Interface do Mapa controle dos elementos da cena
    /// </summary>
    public interface IMap
    {
        void Add(IDrawableActor actor);
        void Remove(IDrawableActor actor);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        Vector2 Velocity {get; set; }
        int MaxSpeed { get; set; }
        int MaxSpeedGlobal { get; set; }
        int CheckPointPixelDistance { get; set; }
        float CheckPointTime { get; set; }    //tempo em milisegundos de duração da gasolina (90000 = 1 min e 30 segundos
        float TimerCount { get; set; }         //Contador do tempo decorrido em milisegundos
        //controle de alcançe do checkPoint
        bool CheckPointReach { get; }
        bool CheckPointHeroiReady { get; set; }
        bool HeroiStopping { get; set; }
        bool EndOfGas { get; set; }

        event EventHandler<CollisionEventArgs> ColisionsOccours;
        event EventHandler<OutOfBoundsEventArgs> OutOfBounds;
        event EventHandler<ChangeRoadEventArgs> ChangeRoadType;
        IDrawableActor Road { get; }
        void ChangeLaneRegister(IChangeLanelistener listener);
        void ChangeLaneUnRegister(IChangeLanelistener listener);
        float RatioPxMt { get; }        //razão 20 pixel por metros

        //Quantidade e pixels percorridos 
        float PixelsCount { get; }

    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : DrawableGameComponent, IMap
    {

        #region Construção e inicialização

        public Map(Scene scene)
            : base(scene.Game)
        {
            this.scene=scene;

            //gera background
            FifoBackground = new MyFifo();
            //gerar tres backgounds que ficam rolando na fila
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));

            //gera instancia de gestor de estradas
            roads = new RoadManager(scene, this);
            FifoRoad = new MyFifo();
            //gera a fila de estradas pelo gerenciado de extradas 
            FifoRoad.Enqueue(roads.CurrentRoad);
            FifoRoad.Enqueue(roads.NextRoad());
            FifoRoad.Enqueue(roads.NextRoad());

            actors = new DrawAbleActorCollection(scene.Game);

            _safeRemoveList = new List<IDrawableActor>();

            listenersChangeLane = new List<IChangeLanelistener>() ;

            //indica se o ponto de controle já alcançado
            checkPointReach = false;
            CheckPointHeroiReady = false;
            HeroiStopping = false;
            _checkPointRoadMark = false;
            endOfGas = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        #endregion

        #region Área do Update do Map

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //milisegundo decorridos desde o último frame 
            //deveria ser (float)gametime.ElapsedGameTime.TotalMilliseconds mas teriamos que ajusta a velocidade de pixels de acordo com a performace de tempo de cada frame.
            //optamos em colocar 16 milisegundos fixos baseado em 60 FPS fixo para não afetar a contagem de pixel do calculo da distancia percorrida
            float ElapseTime = 16;

            // Atualiza velocidade
            velocity = UpdateSpeed();

            //atualiza o tempo decorrido do checkPoint
            if (!checkPointReach) { timerCount += ElapseTime; }

            //verifica se alcançou o checkPoint
            if ( pixelsCount > _checkPointPixelDistance ) { checkPointReach = true; }

            //verifica se o tempo acabou
            if (timerCount > checkPointTime) { endOfGas = true; }

            //Atualiza rolagem das estradas e background
            UpdateRoads(gameTime);

            if (checkPointReach && HeroiStopping && velocity.Y == 0.0F) { FillGasCheckPoint(gameTime); }

            foreach (IDrawableActor actor in actors)
            {

                actor.Location += velocity;
                actor.Update(gameTime);


                //Testa se ator saiu da tela e dispara evento OutOfBounds
                if (!actor.Outofscreen && ! scene.Game.Window.ClientBounds.Intersects (actor.Bounds))
                {
                    actor.Outofscreen = true;
                    if (OutOfBounds != null)
                        OutOfBounds(this, new OutOfBoundsEventArgs(actor));
                }

            }

            // Teste de colisão entre os objetos colidiveis
            Dictionary<ICollidable,Boolean > actorsAlreadyTested = new Dictionary<ICollidable,Boolean >();

            foreach (ICollidable actor1 in ElementsToCollide )
            {
                foreach (ICollidable actor2 in ElementsToCollide) 
                {
                    if ((!actorsAlreadyTested.ContainsKey(actor2)) && (actor1 != actor2) && (actor1.Collided(actor2))) 
                    {
                        if (ColisionsOccours != null)
                            ColisionsOccours(this, new CollisionEventArgs(actor1, actor2));
                    }
                     
                }
                actorsAlreadyTested[actor1]=true;
            }

            base.Update(gameTime);

            pixelsCount += velocity.Y;

            //remover com segurança os atores 
            SafeRemove();
        }

        /// <summary>
        /// Atualização das rolagem do fundo e da estrada
        /// </summary>
        /// <param name="gameTime">faixa de tempo do jogo</param>
        private void UpdateRoads(GameTime gameTime)
        {

            //Atualiza a posição das imagems de fundo na fila
            foreach (IDrawableActor bkg in FifoBackground)
            {
                bkg.Location += velocity;
                bkg.Update(gameTime);
            }

            //Atualiza a posição das imagems das estradas na fila
            foreach (IDrawableActor road in FifoRoad)
            {
                road.Location += velocity;
                road.Update(gameTime);

                //Processa a liata de ouvintes de troca de estrada atualiza a informação das pista para o ator ouvinte
                foreach (IChangeLanelistener cll in listenersChangeLane)
                {
                    if (road.Bounds.Bottom >= cll.Bounds.Y && road.Bounds.Top <= cll.Bounds.Y && cll.CurrentLanes != ((IRoad)road).Lanes)
                        cll.NewLanes = ((IRoad)road).Lanes;
                }
            }

            //atuliza a rolagem de fundo
            adjustPosition(FifoBackground, true);

            //atualiza a rolagem de estradas
            if (adjustPosition(FifoRoad, false))
            {
                //dispara o evento de troca de estrada
                if (ChangeRoadType != null)
                    ChangeRoadType(this, new ChangeRoadEventArgs(((IRoad)FifoRoad.First()).Lanes, ((StraightRoad)FifoRoad.First()).RoadType));
            }

        }

        private Vector2 UpdateSpeed()
        {
            float speed = velocity.Y;
            float speedinc = speed * _acceleration;
            if (speedinc < _acceleration) { speedinc = _acceleration; }

            if (!checkPointReach && !endOfGas) { speed += speedinc; }
            else if (endOfGas) { speed -= speedinc; }
            else if (HeroiStopping)
            {
                speed -= speedinc;
                if (speed <= 1 && pixelCheckPoint > pixelsCount) { speed += speedinc * 1.5F; }
            }
            else
            {
                if (speed > _checkPointSpeed) { speed -= speedinc; }
                else { speed += speedinc; }
            }

            if ( speed > _maxSpeed ) { speed = _maxSpeed;}
            if ( speed < 0 ) {speed = 0.0F;}

            return new Vector2 (velocity.X, speed);
        }


        /// <summary>
        /// Encher o tanque
        /// </summary>
        /// <param name="gameTime"></param>
        private void FillGasCheckPoint(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            temptimer += (int)time; 

            int timewait = 3000 - temptimer ;

            float fill = timerCount;

            if (timewait >0 ) {fill = (timerCount) * ( time / timewait ); }

            timerCount -= fill;


            if (timerCount<=0) { RestartCheckPoint();}

        }

        private void RestartCheckPoint()
        {
            temptimer = 0;
            timerCount = 0;
            checkPointReach = false;
            CheckPointHeroiReady = false;
            HeroiStopping = false;
            _checkPointRoadMark = false;
            endOfGas = false;
            _checkPointPixelDistance += (int) (_checkPointPixelBase) ;
            _checkPointPixelBase += 5000; 
            _maxSpeed += 2;
            if (_maxSpeed > _maxSpeedGlobal) _maxSpeed = _maxSpeedGlobal;
        }
        #endregion

        #region metodos Interface IMAP

        public override void Draw(GameTime gameTime)
        {
            foreach (IDrawableActor bkg in FifoBackground) bkg.Draw(gameTime);
            foreach (IDrawableActor road in FifoRoad) road.Draw(gameTime);
            actors.Draw(gameTime);
        }

        public void Add(IDrawableActor actor)
        {
            actors.Add(actor);
        }

        public void Remove(IDrawableActor actor)
        {
            _safeRemoveList.Add(actor);
            //actors.Remove(actor);
        }

        private void SafeRemove()
        {
            foreach (IDrawableActor actor in _safeRemoveList) actors.Remove(actor);
            _safeRemoveList.Clear();
        }

        #endregion

        #region Controle de Rolagem de atores

        /// <summary>
        /// Verifica se o primeiro elemento da fila saiu da tela e reposiciona para último da fila
        /// e atualiza o posição para anterior ao penúltimo.
        /// </summary>
        /// <param name="fifo">Fila que será atualizada</param>
        /// <param name="enqueue">Flag que indica modo de rolagem. True: rolagem simples( o primeiro movido para último ). False: GERAÇÃO DE ESTRADAS.  </param>
        /// <returns></returns>
        private bool adjustPosition(MyFifo fifo, bool enqueue)
        {
            if ( fifo.Peek().Bounds.Top > scene.Game.Window.ClientBounds.Bottom)
            {

                if (enqueue) 
                {
                    //move o primeiro para úlima posição da fila
                    fifo.Enqueue(fifo.Dequeue());
                }
                else
                { 
                    //GERAÇÃO DE ESTRADAS
                    fifo.Dequeue();

                    //ação de entrada no checkpoint automática
                    if (checkPointReach)
                    {
                        IRoad ckproad = roads.NextRoadCheckPoint();
                        if (ckproad.RoadType == roads.CheckPointTargetRoad && CheckPointHeroiReady && !_checkPointRoadMark ) 
                        { 
                            ckproad.CheckPoint = true;
                            _checkPointRoadMark = true;
                            //salva o ponto de parada do heroi no checkpoint
                            pixelCheckPoint = ckproad.GlobalPixelPosition + ((IDrawableActor)ckproad).Bounds.Height / 2;
                        }
                        fifo.Enqueue((IDrawableActor)ckproad);

                    }
                    // regra do honorato
                    else if ( (pixelsCount + scene.Game.Window.ClientBounds.Height * 4)  > _checkPointPixelDistance)
                    {
                        fifo.Enqueue(roads.NextRoadCheckPoint());
                    }
                    else
                    {
                        fifo.Enqueue(roads.NextRoad());
                    }
                    //Verifica sinalização das estradas
                    CheckRoadSign (fifo);
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Atualiza a Estada que conterá a sinalização no chão
        /// </summary>
        /// <param name="fifo"></param>
        private void CheckRoadSign(MyFifo fifo) 
        {
            if (((IRoad)fifo.Last).Lanes.Count < ((IRoad)fifo.SecondLast).Lanes.Count)
            {
                int lane = ((IRoad)fifo.SecondLast).Lanes.LastIndex;
                int X = ((IRoad)fifo.SecondLast).Lanes.LanesList[lane];
                ((IRoad)fifo.SecondLast).RoadSign = X;
            }
        }

        #endregion

        #region Map Properties
        
        public Vector2 Velocity    { get { return velocity; }       set { velocity = value;  } }

        /// <summary>
        /// Velocidade máxima na sessão do check point
        /// </summary>
        public int MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }

        /// <summary>
        /// Velocidade máxima global - máxima do jogo
        /// </summary>
        public int MaxSpeedGlobal  { get { return _maxSpeedGlobal; } set { _maxSpeedGlobal = value; } }

        public int CheckPointPixelDistance { get { return _checkPointPixelDistance; } set{ _checkPointPixelDistance=value;} }

        public bool CheckPointReach { get { return checkPointReach; } }

        public bool CheckPointHeroiReady { get; set; }
        public bool HeroiStopping { get; set; }

        public float CheckPointTime { get { return checkPointTime; } set {checkPointTime=value;} }    //tempo em milisegundos de duração da gasolina (90000 = 1 min e 30 segundos
        public float TimerCount { get { return timerCount; } set { timerCount=value; } }

        public bool EndOfGas { get { return endOfGas; } set { endOfGas = value; } }

        /// <summary>
        /// Contado de pixes percorridos
        /// </summary>
        public float PixelsCount { get { return pixelsCount; } }

        //razão 20 pixel por metros
        public float RatioPxMt { get { return 20.0F; } }  

        public IDrawableActor Road { get { return FifoRoad.First(); } }

        #endregion

        #region Map Fields
        private DrawAbleActorCollection actors;
        private Vector2 velocity;

        //filas para rolagem de fundo e estradas
        private MyFifo FifoBackground;
        private MyFifo FifoRoad;

        //==============================================================
        //testes com pista vazia
        //_maxSpeed=14 /checkPointTime = 90000 ==> resultado 3886 pontos
        //_maxSpeed=16 /checkPointTime = 90000 ==> resultado 4545 pontos
        //_maxSpeed=18 /checkPointTime = 90000 ==> resultado 5120 pontos
        //_maxSpeed=20 /checkPointTime = 90000 ==> resultado 5696 pontos
        //==============================================================

        private float _acceleration = 0.01F;
        private int _maxSpeed = 14;
        private int _maxSpeedGlobal = 20;
        //distancia em pixel para o checkpoint
        private int _checkPointPixelDistance = 60000;  // 3000 * 20

        private int _checkPointPixelBase = 65000;
        private List<IDrawableActor> _safeRemoveList;

        //controle de alcançe do checkPoint
        private bool checkPointReach;

        //velocidade para entrar no posto do chepoint
        private int _checkPointSpeed = 7;

        private bool _checkPointRoadMark;

        //tempo em milisegundos de duração da gasolina (90000 = 1 min e 30 segundos)
        private float checkPointTime = 90000;

        //Contador do tempo decorrido em milisegundos
        private float timerCount = 0;

        //final do combustivel (tempo)
        private bool endOfGas;

        //Quantidade e pixels percorridos 
        float pixelsCount;

        //roads manager
        private IRoadManager roads;

        //Current Scene 
        private Scene scene;

        //lista de ouvinte do evento de troca de pista
        private List<IChangeLanelistener> listenersChangeLane;

        private int temptimer = 0;

        private float pixelCheckPoint;

        #endregion

        #region Collision
        public event EventHandler<CollisionEventArgs> ColisionsOccours;


        /// <summary>
        /// Os atores colidíveis. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="actors"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<ICollidable> ElementsToCollide
        {
            get
            {
                return from e in actors
                       where e is ICollidable
                       select e as ICollidable;
            }
        }
        #endregion

        #region Out of Bounds Actor

        public event EventHandler<OutOfBoundsEventArgs> OutOfBounds;

        #endregion

        #region ChangeRoad Event

        public void ChangeLaneRegister(IChangeLanelistener listener)
        {
            listener.NewLanes = ((IRoad)FifoRoad.First()).Lanes;
            listenersChangeLane.Add(listener);
        }

        public void ChangeLaneUnRegister(IChangeLanelistener listener)
        {
            listenersChangeLane.Remove(listener);
        }

        public event EventHandler<ChangeRoadEventArgs> ChangeRoadType;

        private class BackGround : BasicDrawingActor
        {
            public BackGround(Game game, SpriteBatch spriteBatch)
                : base(game, game.Content.Load<Texture2D>("Textures/grass1920"))
            {
                this.SpriteBatch = spriteBatch;
            }
            public override void Draw(GameTime gameTime)
            {
                SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
            }
        }

        #endregion

        #region MyFifo Class Fila de controle de Rolagem

        private class MyFifo : Queue<IDrawableActor> 
        {
            public IDrawableActor Last { get; private set; }
            public IDrawableActor SecondLast { get; private set; }
            public IDrawableActor Fist { get { return base.Peek(); } }

            public new void Enqueue(IDrawableActor item) 
            {
                SecondLast = Last;
                Last = item;
                if (item is IRoad) { ((IRoad)item).UpdateRoadSequence((IRoad)SecondLast); }
                base.Enqueue(item);
                if (SecondLast != null) adjustNext();
            }
            private void adjustNext()
            {
                Last.Location = new Vector2(Last.Bounds.X, SecondLast.Location.Y - Last.Bounds.Height);
            }

        }

        #endregion

    }

    #region EventsArgs
    /// <summary>
    /// Classe para eventos colisão entre atores
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        ICollidable colliderA;
        public ICollidable ColliderA
        {
            get { return colliderA; }
            private set { colliderA = value; }
        }

        ICollidable colliderB;
        public ICollidable ColliderB
        {
            get { return colliderB; }
            private set { colliderB = value; }
        }

        public CollisionEventArgs(ICollidable a, ICollidable b)
        {
            ColliderA = a;
            ColliderB = b;
        }
    }

    /// <summary>
    /// Class para eventos de saida da tela
    /// </summary>
    public class OutOfBoundsEventArgs : EventArgs
    {
        IDrawableActor outactor;

        public IDrawableActor OutActor {  get { return outactor; } private set { outactor = value; }  }

        public OutOfBoundsEventArgs(IDrawableActor outator) { this.outactor = outator; }

    }

    /// <summary>
    /// Class para eventos de troca de tipo da estrada
    /// </summary>
    public class ChangeRoadEventArgs : EventArgs
    {
        ILanes lanes;
        RoadTypes roadtype;

        public ILanes CurrentLanes { get { return lanes; } private set { lanes = value; } }
        public RoadTypes Roadtype { get { return roadtype; } private set { roadtype = value; } }

        public ChangeRoadEventArgs(ILanes lanes, RoadTypes roadtype ) 
        { 
            this.lanes = lanes;
            this.roadtype = roadtype;
        }

    }

    #endregion

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Actors
{
    public class StraightRoad : BasicDrawingActor, IRoad
    {
        private Scene scene;

        private IMap map;

        private RoadTypes roadtype;
        private int ajuste;
        int roadsign = 0 ;
        int distance = 0;
        private bool checkpoint;
        private BasicDrawingActor sign1;
        private Sign2 sign2;
        private BasicDrawingActor gasStation;

        public RoadTypes RoadType { get { return roadtype; } }

        protected StraightRoad(Scene scene, IMap map, RoadTypes roadtype, Texture2D texture)
            : base(scene.Game, texture)
        {
            this.scene = scene;
            this.map = map;
            this.roadtype = roadtype;
            this.SpriteBatch = scene.currentSpriteBatch;
            this.Scrollable = true;
            this.Location = Vector2.Zero;
            commonInit();
        }


        public StraightRoad(Scene scene, IMap map, RoadTypes roadtype)
            : base(scene.Game, RoadTexture(roadtype, scene.Game))
        {
            this.scene = scene;
            this.map = map;
            this.roadtype = roadtype;
            this.SpriteBatch = scene.currentSpriteBatch;
            this.Scrollable = true;
            this.Location = Vector2.Zero;
            commonInit();
        }

        private void commonInit() 
        {
            ajuste = (scene.Game.Window.ClientBounds.Width / 2) - (Bounds.Width / 2);
            ajuste += (int) ValorAjuste();
            sign1  = new Sign1(scene.Game,scene.currentSpriteBatch );
            sign2  = new Sign2(scene.Game, scene.currentSpriteBatch);
            gasStation = new GasStation(scene.Game, scene.currentSpriteBatch);
            checkpoint = false;
        }


        public override void Update(GameTime gameTime) 
        {
            //sign1.Location = new Vector2((float)roadsign, Location.Y + Bounds.Height / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)(Location.X + ajuste), (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);

            if (roadsign > 0 && !CheckPoint) 
            {
                sign1.Location = new Vector2((float)roadsign, Location.Y + Bounds.Height / 3);
                sign1.Draw(gameTime);
                sign1.Location = new Vector2((float)roadsign, Location.Y + Bounds.Height*2 / 3);
                sign1.Draw(gameTime);
            }

            if (distance  > 0  )
            {
                sign2.V = distance;
                foreach (float item in lanes.LanesList )
                {
                    sign2.Location = new Vector2(item, Location.Y + Bounds.Height / 2);
                    sign2.Draw(gameTime);
                    
                }
            }
            if (CheckPoint)
            {
                gasStation.Location = new Vector2( lanes.LanesList [lanes.LastIndex ] -31 , Location.Y);
                gasStation.Draw(gameTime);
            }
        }

        public StraightRoad Clone()
        {
            StraightRoad sr = new StraightRoad(scene, map, roadtype, this.Texture );
            return sr;
        }

        #region Implementação de estrada e pistas

        private ILanes lanes;
        public ILanes Lanes { get { return lanes; }  set { Lanes = value; } }


        public int GlobalPixelPosition { get; set; }
        public IRoad nextroad { get; set; }
        public IRoad prevroad { get; set; }

        public void UpdateRoadSequence(IRoad nextroad) 
        {
            if (nextroad == null)
            {
                GlobalPixelPosition = 0;
                return;
            }
            this.nextroad = nextroad;
            GlobalPixelPosition = nextroad.GlobalPixelPosition + ((IDrawableActor)nextroad).Bounds.Height + 1;
            nextroad.prevroad = this;

            //coloca a marcação de distancia do check point a cada 1000m e quando menor que 1000m coloca a marca de 500m também
            int Y1 = map.CheckPointPixelDistance - GlobalPixelPosition;
            int Y2 = map.CheckPointPixelDistance - GlobalPixelPosition - Bounds.Height;
            int div = 1000;
            if ( Y1 < (1000*map.RatioPxMt) ) div=500;

            int mark = (int) ( Y1 / (div * map.RatioPxMt) );
            int markpx = (int) ( mark * div * map.RatioPxMt );
            if (Y1 >= markpx && Y2 < markpx) { this.Distance = mark * div;}
        }

        public bool CheckPoint 
        { get { return checkpoint; }
          set {
                checkpoint = value;
                if (checkpoint) { lanes = new FourLanesCheckPoint(this, ajuste); }
                }
        }

        #endregion

        public static Texture2D RoadTexture (RoadTypes roadtype, Game game) 
        {
            switch (roadtype)
	        {
                case RoadTypes.Road4:
                    return game.Content.Load<Texture2D>("Textures/road1024_4");
                case RoadTypes.Road4to3:
                    return game.Content.Load<Texture2D>("Textures/road1024_4-3");
                case RoadTypes.Road3:
                    return game.Content.Load<Texture2D>("Textures/road1024_3");
                case RoadTypes.Road3to2:
                    return game.Content.Load<Texture2D>("Textures/road1024_3-2");
                case RoadTypes.Road2:
                    return game.Content.Load<Texture2D>("Textures/road1024_2");
                case RoadTypes.Road2to3:
                    return game.Content.Load<Texture2D>("Textures/road1024_2-3");
                case RoadTypes.Road3to4:
                    return game.Content.Load<Texture2D>("Textures/road1024_3-4");
                default:
                    return game.Content.Load<Texture2D>("Textures/road1024_4");
            }
        }

        public float ValorAjuste()
        {
            switch (roadtype)
            {
                case RoadTypes.Road4:
                    lanes = new FourLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road4to3:
                    lanes = new TreeLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road3:
                    lanes = new TreeLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road3to2:
                    lanes = new TwoLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road2:
                    lanes = new TwoLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road2to3:
                    lanes = new TreeLanes(this,ajuste);
                    return 0.0F;
                case RoadTypes.Road3to4:
                    lanes = new FourLanes(this,ajuste);
                    return 0.0F;
                default:
                    return 0.0F;
            }
        }

        public int RoadSign { get { return roadsign; } set { roadsign=value; } }

        public int Distance { get { return distance; } set { distance=value;} }


        private class Sign1 : BasicDrawingActor
        {
            public Sign1(Game game, SpriteBatch spriteBatch)
                : base(game, game.Content.Load<Texture2D>("Textures/setaPista"))
            {
                this.SpriteBatch = spriteBatch;
            }
            public override void Draw(GameTime gameTime)
            {
                SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
            }
        }

        private class Sign2 : BasicDrawingActor
        {
            SpriteFont Arial;

            int v ;
            public int V { set { v = value; } }

            public Sign2(Game game, SpriteBatch spriteBatch)
                : base(game, game.Content.Load<Texture2D>("Textures/sinalgasolina"))
            {
                this.SpriteBatch = spriteBatch;
                Arial = game.Content.Load<SpriteFont>("arial");
            }
            public override void Draw(GameTime gameTime)
            {
                SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
                SpriteBatch.DrawString(Arial, v + " m", new Vector2(Location.X, Location.Y + Bounds.Height), Color.White);
            }
        }

        private class GasStation : BasicDrawingActor
        {
            public GasStation(Game game, SpriteBatch spriteBatch)
                : base(game, game.Content.Load<Texture2D>("Textures/posto"))
            {
                this.SpriteBatch = spriteBatch;
            }
            public override void Draw(GameTime gameTime)
            {
                SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
            }
        }

    }

    #region RoadManager
    /// <summary>
    /// Tipo de estrada diferentes
    /// </summary>
    public enum RoadTypes { Road4, Road4to3, Road3, Road3to2, Road2, Road2to3, Road3to4 }

    /// <summary>
    /// Define a interface do sequenciador de estradas
    /// </summary>
    public interface IRoadManager 
    {
        StraightRoad NextRoad();
        StraightRoad NextRoadCheckPoint();
        RoadTypes CheckPointTargetRoad { get; set; }
        StraightRoad CurrentRoad { get; }
    }

    /// <summary>
    /// Implementa o gerenciamento de estadas. gerar as estradas aleatórias na sequência correta de transição entre as pistas
    /// </summary>
    public class RoadManager:GameComponent, IRoadManager
    {
        #region variaveis da classe
        
        //cena corrente
        private Scene scene;

        //Map
        private IMap map;

        //lista de estradas
        private Dictionary< RoadTypes, StraightRoad> RoadList;

        //Controle de coerencia de transição de estradas
        private Dictionary< RoadTypes, NodeRoad> NextList;

        //estrada corrente
        private RoadTypes current;

        //estrada alvo parada do checkpoint
        private RoadTypes checkPointTargetRoad;

        private Random random;

        #endregion 

        #region Inicialização

        /// <summary>
        /// construção
        /// </summary>
        /// <param name="scene"></param>
        public RoadManager(Scene scene , IMap map)
            : base(scene.Game)
        {
            this.scene = scene;

            this.map = map;

            RoadList = new Dictionary<RoadTypes, StraightRoad>();
            NextList = new Dictionary<RoadTypes, NodeRoad>();

            int TheSeed = (int)DateTime.Now.Ticks;
            random = new Random(TheSeed);

            //inicializa roads
            Init();
        }

        /// <summary>
        /// Inicialização. definição da transição das estradas
        /// </summary>
        private void Init()
        {
            //Carrega a lista de tipos de estrada
            foreach (RoadTypes roadtype in Enum.GetValues(typeof(RoadTypes)))
            {
                RoadList.Add(roadtype, new StraightRoad(scene, map, roadtype));
            }

            //define o tipo de estrada inicial
            current = RoadList.FirstOrDefault().Key;

            //define a pista para checkpoint
            checkPointTargetRoad = RoadTypes.Road4;

            #region sequencia das estradas
            //define a sequencia das estradas

            //estrada 4 pistas
            AddInNextList(RoadTypes.Road4, new RoadTypes[] { RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4, RoadTypes.Road4to3 });

            //estrada 4 p/ 3 pistas
            AddInNextList(RoadTypes.Road4to3, new RoadTypes[] { RoadTypes.Road3});

            //estrada 3 pistas
            AddInNextList(RoadTypes.Road3, new RoadTypes[] { RoadTypes.Road3, RoadTypes.Road3, RoadTypes.Road3, RoadTypes.Road3, RoadTypes.Road3, RoadTypes.Road3, RoadTypes.Road3to2, RoadTypes.Road3, RoadTypes.Road3to2, RoadTypes.Road3to4, RoadTypes.Road3to4, RoadTypes.Road3to4 });

            //estrada 3 p/ 2 pistas
            AddInNextList(RoadTypes.Road3to2, new RoadTypes[] { RoadTypes.Road2 });

            //estrada 2 pistas
            AddInNextList(RoadTypes.Road2, new RoadTypes[] { RoadTypes.Road2, RoadTypes.Road2, RoadTypes.Road2, RoadTypes.Road2, RoadTypes.Road2, RoadTypes.Road2to3 });

            //estrada 2 p/ 3 pistas
            AddInNextList(RoadTypes.Road2to3, new RoadTypes[] { RoadTypes.Road3});

            //estrada 3 p/ 4 pistas
            AddInNextList(RoadTypes.Road3to4, new RoadTypes[] { RoadTypes.Road4});
            //AddInNextList(RoadTypes.Road3to4, new RoadTypes[] { RoadTypes.Road4, RoadTypes.Road4to3 });

            #endregion

        }

        /// <summary>
        /// metodo auxiliar para incluir as transições das estradas
        /// </summary>
        /// <param name="roadtype"></param>
        /// <param name="nextroads"></param>
        private void AddInNextList(RoadTypes roadtype, RoadTypes[] nextroads)
        {
            NextList.Add(roadtype, new NodeRoad(RoadList[roadtype], new List<RoadTypes>(nextroads), roadtype));
        }

        #endregion

        #region metodos e propriedade públicas da classe
        /// <summary>
        /// gera aleatóriamente um novo trecho de estrada baseado na estrada corrente
        /// e atualiza a estrada corrente com a nova estrada
        /// </summary>
        /// <returns></returns>
        public StraightRoad NextRoad()
        {
            int index = random.Next ( NextList[ current ].NextRoads.Count);
            
            current = NextList[current].NextRoads[index];

            return RoadList[current].Clone();
        }

        public StraightRoad NextRoadCheckPoint()
        {
    
            current = NextList[current].NextRoads[0];

            if (current != checkPointTargetRoad)
            {
                foreach (RoadTypes road in NextList[current].NextRoads)
                {
                    if (road == checkPointTargetRoad)
                    {
                        current = road;
                        break;
                    }
                }
                if (current != checkPointTargetRoad)
                {
                    foreach (RoadTypes road in NextList[current].NextRoads)
                    {
                        if (RoadList[road].Lanes.Count > RoadList[current].Lanes.Count)
                        {
                            current = road;
                            break;
                        }
                    }
                }
            }
            return RoadList[current].Clone();
        }

        public StraightRoad CurrentRoad { get { return RoadList[current].Clone(); } }

        public RoadTypes CheckPointTargetRoad { get { return checkPointTargetRoad; } set { checkPointTargetRoad=value; } }

        #endregion 

        #region NodeRoad
        /// <summary>
        /// No de relacionamento para a transição de estradas
        /// </summary>
        private class NodeRoad
        {
            public StraightRoad road;
            public List<RoadTypes> NextRoads;
            public RoadTypes roadtype;

            public NodeRoad(StraightRoad road, List<RoadTypes> NextRoads, RoadTypes roadtype)
            {
                this.road = road;
                this.NextRoads = NextRoads;
                this.roadtype = roadtype;
            }
        }
        #endregion

    }

    #endregion

    #region Road & Lanes

    public interface IChangeLanelistener : IDrawableActor
    {
        ILanes NewLanes { set; }
        ILanes CurrentLanes { get; }
    }

    public interface IRoad
    {
        ILanes Lanes { get; set; }
        int RoadSign { get; set; }
        int Distance { get; set; }
        bool CheckPoint { get; set; }
        RoadTypes RoadType { get; }
        void UpdateRoadSequence(IRoad nextroad);
        int GlobalPixelPosition { get; set; }
        IRoad nextroad { get; set; }
        IRoad prevroad { get; set; }
    }

    public interface ILanes
    {
        IRoad Road { get; }
        List<int> LanesList { get; set; }
        int Count { get; set; }
        int StartIndex { get; set; }
        int LastIndex { get; set; }

    }
    public class FourLanes : ILanes
    {
        private IRoad road;
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 4;
        public FourLanes( IRoad road,int ajuste)
        {
            this.road = road;
            _laneslist = new List<int>();
            for (int i = _starindex; i < _count; i++)
            {
                _laneslist.Add(ajuste + 270 + 130 * i);
                _lastindex = i;
            }
        }

        public IRoad Road { get { return road; } }
        public List<int> LanesList { get { return _laneslist; } set { _laneslist = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }

    public class FourLanesCheckPoint : ILanes
    {
        private IRoad road;
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 5;
        public FourLanesCheckPoint(IRoad road, int ajuste)
        {
            this.road = road;
            _laneslist = new List<int>();
            for (int i = _starindex; i < _count; i++)
            {
                _laneslist.Add(ajuste + 270 + 130 * i);
                _lastindex = i;
            }
            //_laneslist[_lastindex] = _laneslist[_lastindex]-10
        }

        public IRoad Road { get { return road; } }
        public List<int> LanesList { get { return _laneslist; } set { _laneslist = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }


    public class TreeLanes : ILanes
    {
        private IRoad road;
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 3;

        public TreeLanes(IRoad road, int ajuste)
        {
            this.road = road;
            _laneslist = new List<int>();
            for (int i = _starindex; i < _count; i++)
            {
                _laneslist.Add(ajuste + 270 + 130 * i);
                _lastindex = i;
            }
        }
        public IRoad Road { get { return road; } }
        public List<int> LanesList { get { return _laneslist; } set { _laneslist = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }
    public class TwoLanes : ILanes
    {
        private IRoad road;
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 2;

        public TwoLanes(IRoad road, int ajuste)
        {
            this.road = road;
            _laneslist = new List<int>();
            for (int i = _starindex; i < _count; i++)
            {
                _laneslist.Add(ajuste + 270 + 130 * i);
                _lastindex = i;
            }
        }

        public IRoad Road { get { return road; } }
        public List<int> LanesList { get { return _laneslist; } set { _laneslist = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }

    #endregion

}

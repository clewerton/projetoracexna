using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Input;
using TangoGames.RoadFighter.Actors;

namespace TangoGames.RoadFighter.Levels
{
    public class Heroi : BasicDrawingActor, ICollidable 
    {
        private Game game;

        private Texture2D botaoEsquerda;
        private Texture2D botaoDireita;

        private Rectangle retEsquerda;
        private Rectangle retDireita;

        private float angulo = 0;
        //0.261
        //private IList<int> listadepistas;
        //private int numeroPistas = 4;

        private int faixaAnterior = 1;
        private int faixaAtual = 1;

        private int _fixY;

        private IInputService input;

        public Heroi(Game game, Vector2 dimensions, SpriteBatch spriteBatch)
            : base(game, game.Content.Load<Texture2D>("Textures/CarroHeroi"))
        {
            this.game = game;
            Collidable = true;

            //cria definicão de pistas inicial deve ser atualizado pelo método CurrentRoad
            _lanes = new FourLanes(); 

            //Move(new Vector2(listadepistas.ElementAt(1), 0));

            botaoEsquerda = game.Content.Load<Texture2D>("Widgets/botaoEsquerda");
            botaoDireita = game.Content.Load<Texture2D>("Widgets/botaoDireita");

            retEsquerda = new Rectangle(20, game.Window.ClientBounds.Height - game.Window.ClientBounds.Height /4, 120, 120);
            retDireita = new Rectangle(game.Window.ClientBounds.Width - 150, game.Window.ClientBounds.Height - game.Window.ClientBounds.Height / 4, 120, 120);

            input = (IInputService)game.Services.GetService(typeof(IInputService));

            _fixY = game.Window.ClientBounds.Height - (Texture.Height * 2);

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Location = new Vector2(this.Location.X, _fixY);

            controleHeroi();

            movimentaHeroi();

        }


        public override void Draw(GameTime gameTime)
        {
            //SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height),null, Color.White);

            //SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);

            //versão akochada do honorato
            //SpriteBatch.Draw(Texture, new Rectangle((int)Location.X + Texture.Width / 2, game.Window.ClientBounds.Height - Texture.Height, Bounds.Width, Bounds.Height), null, Color.White, (float)angulo, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);

            //versão tentando acertar com bounds
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X + Texture.Width / 2, (int)Location.Y + Texture.Height, Bounds.Width, Bounds.Height), null, Color.White, (float)angulo, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);

            //SpriteBatch.Draw(Texture, new Rectangle((int)Location.X + Texture.Width / 2, (int)Location.Y + Texture.Height, Bounds.Width, Bounds.Height), null, Color.White, (float)angulo, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
            //new Vector2(Texture.Width/2, Texture.Height)

            SpriteBatch.Draw(botaoEsquerda, retEsquerda, Color.White);
            SpriteBatch.Draw(botaoDireita, retDireita, Color.White);
        }

        private void controleHeroi()
        {
            if ((input.KeyPressOnce(Keys.Left) || input.MouseClick(retEsquerda)) && (faixaAtual > 0))
            {
                faixaAnterior = faixaAtual;
                faixaAtual--;
            }
            if ((input.KeyPressOnce(Keys.Right) || input.MouseClick(retDireita)) && (faixaAtual < _lanes.Count  - 1))
            {
                faixaAnterior = faixaAtual;
                faixaAtual++;
            }
        }


        private void movimentaHeroi()
        {
            if ((faixaAnterior <= faixaAtual) && (Location.X < _lanes.LanesList [faixaAtual]))
            {
                Move(new Vector2(3, 0));
                angulo = (float)0.1;

                if (Location.X >= _lanes.LanesList[faixaAtual])
                {
                    angulo = 0;
                    Location = new Vector2(_lanes.LanesList[faixaAtual], Location.Y);
                }
            }
            if ((faixaAnterior >= faixaAtual) && (Location.X > _lanes.LanesList[faixaAtual]))
            {
                Move(new Vector2(-3, 0));
                angulo = (float)-0.1;

                if (Location.X <= _lanes.LanesList[faixaAtual])
                {
                    angulo = 0;
                    Location = new Vector2(_lanes.LanesList[faixaAtual], Location.Y);
                }
            }

            //if (Location.X == listadepistas[faixaAtual]  )
            //{

                //angulo = 0.261;

           // }
        }

        #region Controle de Pistas

        private ILanes _lanes;
        public ILanes CurrentRoad 
        { get { return _lanes; } 
          set 
          { 
              _lanes = value;
              faixaAtual = XtoLane(Location.X);
          } 
        }

        private int XtoLane(float x)
        {
            int lane = 0;

            while ( ( x > _lanes.LanesList[lane] ) && ( lane < _lanes.LastIndex ) )
            {
                lane++;
            }

            return lane;
        }

        #endregion

        #region Collision implementation

        /// <summary>
        /// Teste de colisão por retangulo
        /// </summary>
        ICollider collider = new TangoGames.RoadFighter.Actors.BoundingBox();

        public bool Collided(ICollidable that)
        {
            return collider.TestCollision(this, that);
        }

        public ICollider Collider { get { return this.collider; } set { this.collider = value; } }

        public bool Collidable { get; set; }

        public void EnemyCollide(Enemy enemy, IMap map)
        {
            if (enemy.Location.Y < Location.Y)
            {
                map.Velocity = -(enemy.Velocity + new Vector2(0, 1));
            }
            else
            {
                if ((-enemy.Velocity.Y) > map.Velocity.Y)
                { 
                   enemy.Velocity = -map.Velocity;
                }
            }

            //heroi bateu do lado esquerdo do carro inimigo
            if ( (enemy.Location.X != Location.X) && ( XtoLane ( enemy.Location.X ) != faixaAnterior ) )
            {
                int temp = faixaAnterior;
                faixaAnterior = faixaAtual;
                faixaAtual = temp;
            }
        }


        #endregion

    }
}

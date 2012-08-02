using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TangoGames.RoadFighter.Actors
{
    public class StraightRoad : BasicDrawingActor, IRoad, ICollidable
    {
        public StraightRoad(Game game, Vector2 dimensions, Texture2D  texture)
            : base(game, texture)
        {
            lanes = new FourLanes();
            Collidable = true;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
        }

        #region Implementação de estrada e pistas

        private ILanes lanes;
        public ILanes Lanes
        {
            get
            {
                return lanes;
            }
            set
            {
                Lanes = value;
            }
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

        public ICollider Collider
        {
            get
            {
                return this.collider;
            }
            set
            {
                this.collider = value;
            }
        }

        public bool Collidable { get; set; }

        #endregion
    }

    public class StraightRoad4 : StraightRoad
    {
        public StraightRoad4(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_4")) { }
    }


    public class StraightRoad3 : StraightRoad
    {
        public StraightRoad3(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_3")) { }
    }

    public class StraightRoad2 : StraightRoad
    {
        public StraightRoad2(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_2")) { }
    }

    public class StraightRoad43 : StraightRoad
    {
        public StraightRoad43(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_4-3")) { }
    }

    public class StraightRoad34 : StraightRoad
    {
        public StraightRoad34(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_3-4")) { }
    }

    public class StraightRoad32 : StraightRoad
    {
        public StraightRoad32(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_3-2")) { }
    }

    public class StraightRoad23 : StraightRoad
    {
        public StraightRoad23(Game game, Vector2 dimensions)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/road1024_2-3")) { }
    }



    #region Road & Lanes

    public interface IRoad
    {
        ILanes Lanes { get; set; }
    }
    public interface ILanes
    {
        List<int> LanesList { get; set; }
        int Count { get; set; }
        int StartIndex { get; set; }
        int LastIndex { get; set; }
    }
    public class FourLanes : ILanes
    {
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 4;

        public FourLanes()
        {
            _laneslist = new List<int>();
            for (int i = _starindex; i < _count; i++)
            {
                _laneslist.Add(270 + 130 * i);
                _lastindex = i;
            }
        }

        public List<int> LanesList { get { return _laneslist; } set { _laneslist = value; } }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }
    #endregion
}

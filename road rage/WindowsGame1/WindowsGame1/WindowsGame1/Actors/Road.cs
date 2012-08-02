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
    public class StraightRoad : BasicDrawingActor, IRoad, ICollidable
    {
        private Scene scene;
        private RoadTypes roadtype;
        private float ajuste;

        public RoadTypes RoadType { get { return roadtype; } }

        public StraightRoad(Scene scene, RoadTypes roadtype)
            : base(scene.Game, RoadTexture(roadtype, scene.Game))
        {
            this.scene = scene;
            this.roadtype = roadtype;
            this.SpriteBatch = scene.currentSpriteBatch;
            this.Scrollable = true;
            this.Location = Vector2.Zero;
            Collidable = true;
            ajuste = ValorAjuste();
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)(Location.X + ajuste), (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
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

        public static Texture2D RoadTexture (RoadTypes roadtype, Game game) 
        {
            switch (roadtype)
	        {
                case RoadTypes.Road4:
                case RoadTypes.Road4Clone:
                    return game.Content.Load<Texture2D>("Textures/road1024_4");
                case RoadTypes.Road4to3:
                    return game.Content.Load<Texture2D>("Textures/road1024_4-3");
                case RoadTypes.Road3:
                case RoadTypes.Road3Clone:
                    return game.Content.Load<Texture2D>("Textures/road1024_3");
                case RoadTypes.Road3to2:
                    return game.Content.Load<Texture2D>("Textures/road1024_3-2");
                case RoadTypes.Road2:
                case RoadTypes.Road2Clone:
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
                case RoadTypes.Road4Clone:
                    lanes = new FourLanes();
                    return 0.0F;
                case RoadTypes.Road4to3:
                    lanes = new TreeLanes();
                    return 0.0F;
                case RoadTypes.Road3:
                case RoadTypes.Road3Clone:
                    lanes = new TreeLanes();
                    return 0.0F;
                case RoadTypes.Road3to2:
                    lanes = new TwoLanes();
                    return 0.0F;
                case RoadTypes.Road2:
                case RoadTypes.Road2Clone:
                    lanes = new TwoLanes();
                    return 0.0F;
                case RoadTypes.Road2to3:
                    lanes = new TwoLanes();
                    return 0.0F;
                case RoadTypes.Road3to4:
                    lanes = new TreeLanes();
                    return 0.0F;
                default:
                    return 0.0F;
            }
        }

    }


    #region RoadManager

    public enum RoadTypes { Road4, Road4Clone, Road4to3, Road3, Road3Clone, Road3to2, Road2, Road2Clone, Road2to3, Road3to4 }

    public interface IRoadManager 
    {
        StraightRoad NextRoad();
        StraightRoad CurrentRoad { get; }

    }

    public class RoadManager:GameComponent, IRoadManager

    {
        private Scene scene;
        private Dictionary< RoadTypes, StraightRoad> RoadList;
        private Dictionary< RoadTypes, NodeRoad> NextList;
        private Random random;

        private RoadTypes current;

        public RoadManager(Scene scene)
            : base(scene.Game)
        {
            this.scene = scene;
            RoadList = new Dictionary<RoadTypes, StraightRoad>();
            NextList = new Dictionary<RoadTypes, NodeRoad>();
            int TheSeed = (int)DateTime.Now.Ticks;
            random = new Random(TheSeed);

            //inicializa roads
            Init();
        }

        private void Init()
        {
            foreach (RoadTypes roadtype in Enum.GetValues(typeof(RoadTypes)))
            {
                RoadList.Add(roadtype, new StraightRoad(scene, roadtype));
            }

            current = RoadList.FirstOrDefault().Key;

            #region sequencia das estradas
            //define a sequencia das estradas

            //estrada 4 pistas
            AddInNextList(RoadTypes.Road4, new RoadTypes[] { RoadTypes.Road4Clone, RoadTypes.Road4to3 });

            //estrada 4 pistas(clone)
            AddInNextList(RoadTypes.Road4Clone, new RoadTypes[] { RoadTypes.Road4 });

            //estrada 4 p/ 3 pistas
            AddInNextList(RoadTypes.Road4to3, new RoadTypes[] { RoadTypes.Road3 });

            //estrada 3 pistas
            AddInNextList(RoadTypes.Road3, new RoadTypes[] { RoadTypes.Road3Clone, RoadTypes.Road3to2, RoadTypes.Road3to4 });

            //estrada 3 pistas(clone)
            AddInNextList(RoadTypes.Road3Clone, new RoadTypes[] { RoadTypes.Road3 });

            //estrada 3 p/ 2 pistas
            AddInNextList(RoadTypes.Road3to2, new RoadTypes[] { RoadTypes.Road2 });

            //estrada 2 pistas
            AddInNextList(RoadTypes.Road2, new RoadTypes[] { RoadTypes.Road2Clone });

            //estrada 2 pistas
            AddInNextList(RoadTypes.Road2Clone, new RoadTypes[] { RoadTypes.Road2, RoadTypes.Road2to3 });

            //estrada 2 p/ 3 pistas
            AddInNextList(RoadTypes.Road2to3, new RoadTypes[] { RoadTypes.Road3 });

            //estrada 3 p/ 4 pistas
            AddInNextList(RoadTypes.Road3to4, new RoadTypes[] { RoadTypes.Road4 });

            #endregion

        }


        private void AddInNextList(RoadTypes roadtype, RoadTypes[] nextroads)
        {
            NextList.Add(roadtype, new NodeRoad(RoadList[roadtype], new List<RoadTypes>(nextroads), roadtype));
        }

        public StraightRoad NextRoad()
        {
            int index = random.Next(NextList[current ].NextRoads.Count);
            current = NextList[current].NextRoads[index];

            return RoadList [current ] ;
        }

        public StraightRoad CurrentRoad { get { return RoadList [current ]  ; } }

        private class NodeRoad
        {
            public StraightRoad road;
            public List<RoadTypes> NextRoads;
            public RoadTypes roadtype;
            public bool CloneUsed;

            public NodeRoad(StraightRoad road, List<RoadTypes> NextRoads, RoadTypes roadtype)
            {
                this.road = road;
                this.NextRoads = NextRoads;
                this.roadtype = roadtype;
            }
        }
    }

    #endregion


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

    public class TreeLanes : ILanes
    {
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 3;

        public TreeLanes()
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
    public class TwoLanes : ILanes
    {
        private List<int> _laneslist;
        private int _starindex = 0;
        private int _lastindex = 0;
        private int _count = 2;

        public TwoLanes()
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TangoGames.RoadFighter.Actors
{
    public class StraightRoad : BasicDrawingActor,IRoad 
    {
        public StraightRoad(Game game, Vector2 dimensions, SpriteBatch spriteBatch)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/straight_road_4"))
        {
            lanes = new FourLanes();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
        }

        #region Implementação de estrada e pistas

        public ILanes Lanes { get { return lanes; } set { Lanes=value ; } }

        private ILanes lanes;

        #endregion
    }

    public interface IRoad
    { 
       ILanes Lanes { get; set; }
    }
    public interface ILanes
    {
        List<int> LanesList { get; set;}
        int Count { get; set;}
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
                _lastindex =i;
            }
        }

        public List<int> LanesList { get { return _laneslist; } set { _laneslist=value ;} }
        public int Count { get { return _count; } set { _count = value; } }
        public int StartIndex { get { return _starindex; } set { _starindex = value; } }
        public int LastIndex { get { return _lastindex; } set { _lastindex = value; } }

    }

}

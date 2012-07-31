using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TangoGames.RoadFighter.Actors
{
    public interface IDrawableActor : IActor
    {
        void Draw(GameTime gameTime);
        bool Visible { get; set; }
        bool Scrollable { get; set; }
        bool Outofscreen { get; set; }
        SpriteBatch SpriteBatch { get; set; }
    }

    public interface IDrawableActorGroup : IActorGroup<IDrawableActor>
    {
        void Draw(GameTime gameTime);
        bool Visible { set; }
    }

    public class DrawAbleActorCollection : ActorCollection<IDrawableActor>
    {
        public DrawAbleActorCollection(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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

        public void Draw(GameTime gameTime)
        {
            foreach (IDrawableActor actor in actors)
            {
                actor.Draw(gameTime);
            }
        }

        public bool Visible
        {
            set
            {
                foreach (IDrawableActor item in actors)
                {
                    item.Visible = value;
                }
            }
        }

    }

}

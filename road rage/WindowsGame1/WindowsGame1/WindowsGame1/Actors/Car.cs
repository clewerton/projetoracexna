using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TangoGames.RoadFighter.Actors
{
    class Car : BasicDrawingActor
    {
        public Car(Game game, Vector2 dimensions, SpriteBatch spriteBatch)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/carSprite"))
        {
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
        }

    }
}

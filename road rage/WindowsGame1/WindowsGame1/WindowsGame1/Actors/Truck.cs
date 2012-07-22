using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TangoGames.RoadFighter.Actors
{
    class Truck : BasicDrawingActor
    {
        public Truck(Game game, Rectangle bounds, SpriteBatch spriteBatch)
            : base(game, bounds, game.Content.Load<Texture2D>("Textures/truckSprite"))
        {
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Texture.Width, Texture.Height), Color.White);
        }

    }
}

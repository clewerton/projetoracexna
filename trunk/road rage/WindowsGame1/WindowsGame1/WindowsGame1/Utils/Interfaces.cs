using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Utils
{
    public interface ISimpleDrawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface ISimpleUpdateable
    {
        void Update(GameTime gameTime);
    }
}

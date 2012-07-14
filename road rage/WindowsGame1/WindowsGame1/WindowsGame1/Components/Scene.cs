using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TangoGames.RoadFighter.Components
{
    public class Scene : GameComponent, IScene
    {
        public Scene(Game game) : base(game)
        {
            Leave(); // comece com tudo desabilitado

            game.Components.Add(this);
        }

        public void Enter()
        {
            // o Update deste objeto passa a ser chamado
            Enabled = true;
        }

        public void Leave()
        {
            // desabilita o Update deste objeto
            Enabled = false;
        }
    }

    public class DrawableScene : DrawableGameComponent, IScene
    {
        public DrawableScene(Game game) : base(game)
        {
            Leave(); // comece com tudo desabilitado

            game.Components.Add(this);
        }

        public void Enter()
        {
            Enabled = true; // habilita o Update
            Visible = true; // habilita o Draw
        }

        public void Leave()
        {
            Enabled = false; // desabilita o Update
            Visible = false; // desabilita o Draw
        }
    }
}

using Microsoft.Xna.Framework;

namespace TangoGames.RoadFighter.Scenes
{
    public class Scene : GameComponent, IScene
    {
        public Scene(Game game) : base(game)
        {
            // comece com tudo desabilitado
            Disable();

            // registre este componente no ciclo de vida do Game
            game.Components.Add(this); 
        }

        public virtual void Enter()
        {
            Enable();
        }

        public virtual void Leave()
        {
            Disable();
        }

        private void Enable()
        {
            // habilita o Update deste objeto
            Enabled = true;
        }

        private void Disable()
        {
            // desabilita o Update deste objeto
            Enabled = false;
        }
    }

    public class DrawableScene : DrawableGameComponent, IScene
    {
        public DrawableScene(Game game) : base(game)
        {
            // comece com tudo desabilitado
            Disable();

            // registre este componente no ciclo de vida do Game
            game.Components.Add(this);
        }

        public virtual void Enter()
        {
            Enable();
        }

        public virtual void Leave()
        {
            Disable();
        }

        private void Enable()
        {
            Enabled = true; // habilita o Update
            Visible = true; // habilita o Draw
        }

        private void Disable()
        {
            Enabled = false; // desabilita o Update
            Visible = false; // desabilita o Draw
        }
    }
}

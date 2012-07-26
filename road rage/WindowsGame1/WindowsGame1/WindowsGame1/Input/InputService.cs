using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TangoGames.RoadFighter.Input
{
    interface IInputService
    {
        bool MouseOver(Rectangle ret);

        bool MouseClick(out Point ponto);

        bool MouseClick(Rectangle ret);

        bool KeyPressOnce(Keys key);

        KeyboardState teclado { get; }
        KeyboardState teclado_anterior { get; }

        MouseState mouse { get; }
        MouseState mouse_anterior { get; }

        GamePadState joystick { get; }
        GamePadState joystick_anterior { get; }

    }

    class InputService: GameComponent, IInputService
    {
        KeyboardState _teclado;
        MouseState _mouse;
        GamePadState _joystick;
        KeyboardState _teclado_anterior;
        MouseState _mouse_anterior;
        GamePadState _joystick_anterior;

        public InputService(Game game):base(game)
        {
            _teclado = Keyboard.GetState();
            _mouse = Mouse.GetState();
            _joystick = GamePad.GetState(PlayerIndex.One);
            _teclado_anterior = Keyboard.GetState();
            _mouse_anterior = Mouse.GetState();
            _joystick_anterior = GamePad.GetState(PlayerIndex.One);
        }

        public override void  Update(GameTime gameTime)
        {
 	        base.Update(gameTime);
            _teclado_anterior = _teclado;
            _mouse_anterior = _mouse;
            _joystick_anterior = _joystick;
            _teclado = Keyboard.GetState();
            _mouse = Mouse.GetState();
            _joystick = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Verifica se o mouse esta sobre uma area retangular
        /// </summary>
        /// <param name="ret">Àrea retangular pra testes</param>
        /// <returns></returns>
        public bool MouseOver(Rectangle ret)
        {
            return ret.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1));
        }

        public bool MouseClick(Rectangle ret)
        {

            if (_mouse_anterior.LeftButton != ButtonState.Pressed && _mouse.LeftButton == ButtonState.Pressed && ret.Intersects(new Rectangle(_mouse.X, _mouse.Y, 1, 1)))
                return true;
            return false;
        }

        public bool MouseClick(out Point ponto)
        {
            ponto = new Point();
            if (_mouse_anterior.LeftButton != ButtonState.Pressed && _mouse.LeftButton == ButtonState.Pressed)
            {
                ponto.X = _mouse.X;
                ponto.Y = _mouse.Y;
                return true;
            }
            return false;
        }

        public bool KeyPressOnce(Keys key)
        {
            if (!_teclado_anterior.IsKeyDown(key) && _teclado.IsKeyDown(key))
                return true;
            return false;
        }


        public KeyboardState teclado { get { return _teclado; } }
        public KeyboardState teclado_anterior { get { return _teclado_anterior; } }

        public MouseState mouse { get { return _mouse; } }
        public MouseState mouse_anterior { get { return _mouse_anterior; } }

        public GamePadState joystick { get { return _joystick; } }
        public GamePadState joystick_anterior { get { return _joystick_anterior; } }


    }

    public class ConfigKey
    {
        Keys _cima;
        Keys _baixo;
        Keys _esquerda;
        Keys _direita;
        Keys _turbo;

        public ConfigKey()
        {
            _cima = Keys.Up;
            _baixo = Keys.Down;
            _esquerda = Keys.Left;
            _direita = Keys.Right;
            _turbo = Keys.RightShift;
        }

        public Keys cima { get { return _cima; } }
        public Keys baixo { get { return _baixo; } }
        public Keys esquerda { get { return _esquerda; } }
        public Keys direita { get { return _direita; } }
        public Keys turbo { get { return _turbo; } }

    }

}

using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System.Diagnostics;

namespace BladeStory.Test.Services
{
    public class GameInputConsumer
    {
        private readonly IInputManager _inputService;

        public GameInputConsumer(IInputManager inputService) 
        {
            _inputService = inputService;
        }

        public bool IsJumping => _inputService.IsKeyPressed(Keys.Space);
        public bool IsMovingLeft => _inputService.IsKeyDown(Keys.A);
        public bool IsMovingRight => _inputService.IsKeyDown(Keys.D);
        public bool IsAttacking => _inputService.IsMouseButtonPressed(MouseButton.Left);

        public Vector2 GetMouseWorldPosition()
        {
            var mousePos = _inputService.GetMousePosition();
            if (_inputService.ViewportAdapter != null)
            {
                var screenPoint =_inputService.ViewportAdapter.PointToScreen(mousePos.ToPoint());
                return screenPoint.ToVector2();
            }
            return mousePos;
        }

        public void Update(GameTime gameTime)
        {
            _inputService.Update(gameTime);

            // 模拟游戏逻辑
            if (IsJumping)
            {
                Debug.WriteLine("我跳跃");
            }

            if (IsMovingLeft)
            {
                Debug.WriteLine("我左转");
            }

            if (IsMovingRight)
            {
                Debug.WriteLine("我右转");
            }

            if (IsAttacking)
            {
                Debug.WriteLine("我攻击");
            }
        }
    }
}

using BladeStory.Core.Commands;
using BladeStory.Core.Components;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Middlewares;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace BladeStory.Service.Middlewares
{
    public class PlayerCommandMiddleware : IUpdatable, IDisposable
    {
        private readonly IInputStateMiddleware _inputState;

        public event Action<MoveCommand>? OnMoveCommand;
        public event Action<InteractCommand>? OnInteractCommand;
        public event Action<DashCommand>? OnDashCommand;

        // 移动相关键位绑定（可配置）
        public Keys MoveUp { get; set; } = Keys.W;
        public Keys MoveDown { get; set; } = Keys.S;
        public Keys MoveLeft { get; set; } = Keys.A;
        public Keys MoveRight { get; set; } = Keys.D;

        // 交互键位
        public Keys InteractKey { get; set; } = Keys.Space;
        public MouseButton InteractMouseButton { get; set; } = MouseButton.Left;

        // 冲刺键位
        public Keys DashKey { get; set; } = Keys.LeftShift;

        public PlayerCommandMiddleware(IInputStateMiddleware inputStateMiddleware)
        {
            _inputState = inputStateMiddleware;
        }

        public void Update(GameTime gameTime)
        {
            // 1. 移动命令（持续状态）
            ProcessMoveCommand(gameTime);

            // 2. 交互命令（边缘触发）
            ProcessInteractCommand(gameTime);

            // 3. 冲刺命令（边缘触发，带冷却）
            ProcessDashCommand(gameTime);
        }

        private void ProcessMoveCommand(GameTime gameTime)
        {
            Vector2 direction = Vector2.Zero;

            if (_inputState.PressedKeys.Contains(MoveUp)) direction.Y -= 1;
            if (_inputState.PressedKeys.Contains(MoveDown)) direction.Y += 1;
            if (_inputState.PressedKeys.Contains(MoveLeft)) direction.X -= 1;
            if (_inputState.PressedKeys.Contains(MoveRight)) direction.X += 1;

            var command = direction != Vector2.Zero
                ? new MoveCommand() 
                { 
                    IsValid = true, 
                    GameTime = gameTime, 
                    MoveDirection = direction.NormalizedCopy() 
                }
                : new MoveCommand()
                {
                    IsValid = true,
                    GameTime = gameTime,
                    MoveDirection = Vector2.Zero
                };

            OnMoveCommand?.Invoke(command);
        }

        private void ProcessInteractCommand(GameTime gameTime)
        {
            // 键盘交互键按下 或 鼠标交互键按下（使用 IsKeyPressed 语义：按下瞬间触发）
            bool keyPressed = IsKeyPressed(InteractKey);
            bool mousePressed = IsMouseButtonPressed(InteractMouseButton);

            if (keyPressed || mousePressed)
            {
                var command = new InteractCommand() 
                { 
                    IsValid = true,
                    GameTime = gameTime, 
                    MousePosition = _inputState.MousePosition 
                };
                OnInteractCommand?.Invoke(command);
            }
        }

        private void ProcessDashCommand(GameTime gameTime)
        {
            bool currentDashKeyDown = _inputState.PressedKeys.Contains(DashKey);

            var command = new DashCommand()
            {
                IsValid = true,
                GameTime = gameTime,
                IsDashing = currentDashKeyDown
            };
            OnDashCommand?.Invoke(command);
        }

        /// <summary>
        /// 边缘触发的按键检测：本次按下且之前没按
        /// </summary>
        private bool IsKeyPressed(Keys key)
        {
            return _inputState.PressedKeys.Contains(key);
        }

        private bool IsMouseButtonPressed(MouseButton button)
        {
            return _inputState.PressedButtons.Contains(button);
        }

        public void Dispose()
        {
            OnMoveCommand = null;
            OnInteractCommand = null;
            OnDashCommand = null;
        }
    }
}

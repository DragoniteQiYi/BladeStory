using BladeStory.Core.Components;
using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace BladeStory.Service.Middlewares
{
    public class PlayerCommandMiddleware : ICommandMiddleware, IDisposable
    {
        private readonly ITimeManager _timeManager;
        private readonly IInputStateMiddleware _inputStateMiddleware;

        public event Action<ICommand>? OnMoveCommand;
        public event Action<ICommand>? OnInteractCommand;
        public event Action<ICommand>? OnDashCommand;

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

        public PlayerCommandMiddleware(IInputStateMiddleware inputStateMiddleware, 
            ITimeManager timeManager)
        {
            _inputStateMiddleware = inputStateMiddleware;
            _timeManager = timeManager;
        }

        public void ProcessInput()
        {

        }

        public void Dispose()
        {
            OnMoveCommand = null;
            OnInteractCommand = null;
            OnDashCommand = null;
        }
    }
}

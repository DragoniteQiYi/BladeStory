using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace BladeStory.Service.Middlewares
{
    public class InputStateMiddleware : IInputStateMiddleware, IUpdate
    {
        private readonly IInputManager _inputManager;

        // 缓存滚轮值用于计算 Delta
        private int _previousScrollValue;
        private int _currentScrollValue;

        public InputStateMiddleware(IInputManager inputManager)
        {
            _inputManager = inputManager;

            // 直接订阅 IInputManager 的事件来更新离散状态
            _inputManager.KeyPressed += OnKeyPressed;
            _inputManager.KeyReleased += OnKeyReleased;
            _inputManager.MouseButtonPressed += OnMousePressed;
            _inputManager.MouseButtonReleased += OnMouseReleased;
            _inputManager.MouseScrolled += OnMouseScrolled;
        }

        // ========== IInputStateMiddleware 实现 ==========
        public HashSet<Keys> PressedKeys { get; } = new();
        public HashSet<MouseButton> PressedButtons { get; } = new();
        public Point MousePosition { get; private set; }
        public Vector2 MouseDelta { get; private set; }
        public int ScrollWheelValue { get; private set; }
        public int ScrollWheelDelta { get; private set; }

        // ========== 事件处理（从 IInputManager 事件同步到此中间件状态） ==========
        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            PressedKeys.Add(e.Key);
        }

        private void OnKeyReleased(object sender, KeyboardEventArgs e)
        {
            PressedKeys.Remove(e.Key);
        }

        private void OnMousePressed(object sender, MouseEventArgs e)
        {
            PressedButtons.Add(e.Button);
        }

        private void OnMouseReleased(object sender, MouseEventArgs e)
        {
            PressedButtons.Remove(e.Button);
        }

        private void OnMouseScrolled(object sender, MouseEventArgs e)
        {
            _currentScrollValue += e.ScrollWheelDelta;
        }

        // ========== 连续状态更新（每帧调用） ==========
        public void Update(GameTime gameTime)
        {
            // 鼠标位置和 Delta：直接从 IInputManager 的状态获取
            var currentState = _inputManager.CurrentMouseState;
            var previousState = _inputManager.PreviousMouseState;

            MousePosition = currentState.Position;
            MouseDelta = new Vector2(
                currentState.Position.X - previousState.Position.X,
                currentState.Position.Y - previousState.Position.Y
            );

            // 滚轮值：通过事件累积（如果走轮询，改用下面的代码）
            // var currentScroll = _inputManager.CurrentMouseState.ScrollWheelValue;
            // ScrollWheelDelta = currentScroll - _previousScrollValue;
            // ScrollWheelValue = currentScroll;
            // _previousScrollValue = currentScroll;
        }

        public void EndOfFrame()
        {
            // 如果你用事件累积滚轮值，在这里结算 Delta
            ScrollWheelDelta = _currentScrollValue - ScrollWheelValue;
            ScrollWheelValue = _currentScrollValue;
        }
    }
}

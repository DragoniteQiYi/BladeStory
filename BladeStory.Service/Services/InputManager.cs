using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.ViewportAdapters;

namespace BladeStory.Service.Services
{
    public class InputManager : IInputManager, IUpdate
    {
        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState PreviousKeyboardState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public TimeSpan CurrentTime { get; set; }
        public bool IsEnabled { get; set; } = true;
        public ViewportAdapter ViewportAdapter => _viewportAdapter;

        public event EventHandler<KeyboardEventArgs>? KeyPressed;
        public event EventHandler<KeyboardEventArgs>? KeyReleased;
        public event EventHandler<MouseEventArgs>? MouseMoved;
        public event EventHandler<MouseEventArgs>? MouseButtonPressed;
        public event EventHandler<MouseEventArgs>? MouseButtonReleased;
        public event EventHandler<MouseEventArgs>? MouseScrolled;
        public event EventHandler<TextInputEventArgs>? TextInput; // 没有需求，暂不实现

        private ViewportAdapter _viewportAdapter;
        private bool _disposed = false;
        private readonly Queue<Action> _eventQueue = new();
        private readonly Lock _eventLock = new();

        public InputManager(ViewportAdapter viewportAdapter)
        {
            _viewportAdapter = viewportAdapter ?? 
                throw new ArgumentNullException(nameof(viewportAdapter));

            Console.WriteLine($"[InputManager]: 输入管理模块初始化成功");
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime = gameTime.TotalGameTime;

            // 保存上一帧状态
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;

            // 获取当前状态
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            if (!IsEnabled) return;

            // 处理键盘事件
            ProcessKeyboardEvents();
            // 处理鼠标事件
            ProcessMouseEvents();

            // 处理事件队列
            ProcessEvents();
        }

        public void ProcessEvents()
        {
            lock (_eventLock)
            {
                while (_eventQueue.Count > 0)
                {
                    var action = _eventQueue.Dequeue();
                    action?.Invoke();
                }
            }
        }

        private void ProcessKeyboardEvents()
        {
            var pressedKeys = CurrentKeyboardState.GetPressedKeys();
            var previousKeys = PreviousKeyboardState.GetPressedKeys();

            // 检查新按下的键
            var previousKeySet = new HashSet<Keys>(previousKeys);
            foreach (var key in pressedKeys)
            {
                if (!previousKeySet.Contains(key))
                {
                    OnKeyPressed(new KeyboardEventArgs(key, CurrentKeyboardState));
#if DEBUG
                    Console.WriteLine($"[InputManager]: 键盘{key}键被按下");
#endif
                }
            }

            // 检查释放的键
            var currentKeySet = new HashSet<Keys>(pressedKeys);
            foreach (var key in previousKeys)
            {
                if (!currentKeySet.Contains(key))
                {
                    OnKeyReleased(new KeyboardEventArgs(key, CurrentKeyboardState));
#if DEBUG
                    Console.WriteLine($"[InputManager]: 键盘{key}键被释放");
#endif
                }
            }
        }

        private void ProcessMouseEvents()
        {
            // 鼠标移动
            if (CurrentMouseState.Position != PreviousMouseState.Position)
            {
                var args = new MouseEventArgs(
                    ViewportAdapter,
                    CurrentTime,
                    PreviousMouseState,
                    CurrentMouseState);
                OnMouseMoved(args);
            }

            // 鼠标按钮
            ProcessMouseButton(MouseButton.Left,
                CurrentMouseState.LeftButton,
                PreviousMouseState.LeftButton);
            ProcessMouseButton(MouseButton.Right,
                CurrentMouseState.RightButton,
                PreviousMouseState.RightButton);
            ProcessMouseButton(MouseButton.Middle,
                CurrentMouseState.MiddleButton,
                PreviousMouseState.MiddleButton);
            ProcessMouseButton(MouseButton.XButton1,
                CurrentMouseState.XButton1,
                PreviousMouseState.XButton1);
            ProcessMouseButton(MouseButton.XButton2,
                CurrentMouseState.XButton2,
                PreviousMouseState.XButton2);

            // 鼠标滚轮
            if (CurrentMouseState.ScrollWheelValue != PreviousMouseState.ScrollWheelValue)
            {
                var args = new MouseEventArgs(
                    ViewportAdapter,
                    CurrentTime,
                    PreviousMouseState,
                    CurrentMouseState);
                OnMouseScrolled(args);
                Console.WriteLine($"[InputManager]: 鼠标滚轮操作");
            }
        }

        private void ProcessMouseButton(MouseButton button, ButtonState current, ButtonState previous)
        {
            if (current == ButtonState.Pressed && previous == ButtonState.Released)
            {
                var args = new MouseEventArgs(
                    ViewportAdapter,
                    CurrentTime,
                    PreviousMouseState,
                    CurrentMouseState,
                    button);
                OnMouseButtonPressed(args);
                Console.WriteLine($"[InputManager]: 鼠标{button}键被按下");
            }
            else if (current == ButtonState.Released && previous == ButtonState.Pressed)
            {
                var args = new MouseEventArgs(
                    ViewportAdapter,
                    CurrentTime,
                    PreviousMouseState,
                    CurrentMouseState,
                    button);
                OnMouseButtonReleased(args);
                Console.WriteLine($"[InputManager]: 鼠标{button}键被释放");
            }
        }

        // 事件触发器方法
        protected virtual void OnKeyPressed(KeyboardEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = KeyPressed;    
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        protected virtual void OnKeyReleased(KeyboardEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = KeyReleased;
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        protected virtual void OnMouseMoved(MouseEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = MouseMoved;
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        protected virtual void OnMouseButtonPressed(MouseEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = MouseButtonPressed;
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        protected virtual void OnMouseButtonReleased(MouseEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = MouseButtonReleased;
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        protected virtual void OnMouseScrolled(MouseEventArgs e)
        {
            lock (_eventLock)
            {
                var handler = MouseScrolled;
                _eventQueue.Enqueue(() => handler?.Invoke(this, e));
            }
        }

        // 工具方法
        public Vector2 GetMousePosition()
        {
            //return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            return _viewportAdapter
                .PointToScreen(CurrentMouseState.X, CurrentMouseState.Y)
                .ToVector2();
        }

        public bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
                   !PreviousKeyboardState.IsKeyDown(key);
        }

        public bool IsMouseButtonDown(MouseButton button)
        {
            return GetMouseButtonState(button) == ButtonState.Pressed;
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            return GetMouseButtonState(button) == ButtonState.Pressed &&
                   GetPreviousMouseButtonState(button) == ButtonState.Released;
        }

        private ButtonState GetMouseButtonState(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => CurrentMouseState.LeftButton,
                MouseButton.Right => CurrentMouseState.RightButton,
                MouseButton.Middle => CurrentMouseState.MiddleButton,
                MouseButton.XButton1 => CurrentMouseState.XButton1,
                MouseButton.XButton2 => CurrentMouseState.XButton2,
                _ => ButtonState.Released
            };
        }

        private ButtonState GetPreviousMouseButtonState(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => PreviousMouseState.LeftButton,
                MouseButton.Right => PreviousMouseState.RightButton,
                MouseButton.Middle => PreviousMouseState.MiddleButton,
                MouseButton.XButton1 => PreviousMouseState.XButton1,
                MouseButton.XButton2 => PreviousMouseState.XButton2,
                _ => ButtonState.Released
            };
        }

        public virtual void Dispose()
        {
            if (!_disposed)
            {
                // 清理托管资源
                lock (_eventLock)
                {
                    _eventQueue.Clear();
                }

                // 清理事件订阅者
                KeyPressed = null;
                KeyReleased = null;
                MouseMoved = null;
                MouseButtonPressed = null;
                MouseButtonReleased = null;
                MouseScrolled = null;
                TextInput = null;

                _disposed = true;
            }
        }
    }
}

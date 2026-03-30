using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.ViewportAdapters;

namespace BladeStory.Service.Interfaces
{
    public interface IInputManager
    {
        // 键盘事件
        event EventHandler<KeyboardEventArgs> KeyPressed;
        event EventHandler<KeyboardEventArgs> KeyReleased;

        // 鼠标事件
        event EventHandler<MouseEventArgs> MouseMoved;
        event EventHandler<MouseEventArgs> MouseButtonPressed;
        event EventHandler<MouseEventArgs> MouseButtonReleased;
        event EventHandler<MouseEventArgs> MouseScrolled;
        event EventHandler<TextInputEventArgs> TextInput;

        // 状态查询
        KeyboardState CurrentKeyboardState { get; }
        KeyboardState PreviousKeyboardState { get; }
        MouseState CurrentMouseState { get; }
        MouseState PreviousMouseState { get; }

        TimeSpan CurrentTime { get; set; }
        ViewportAdapter ViewportAdapter { get; }

        // 工具方法
        void Update(GameTime gameTime);
        Vector2 GetMousePosition();
        bool IsKeyDown(Keys key);
        bool IsKeyPressed(Keys key);
        bool IsMouseButtonDown(MouseButton button);
        bool IsMouseButtonPressed(MouseButton button);
    }
}

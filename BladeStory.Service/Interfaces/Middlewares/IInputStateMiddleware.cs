using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace BladeStory.Service.Interfaces.Middlewares
{
    public interface IInputStateMiddleware
    {
        HashSet<Keys> PressedKeys { get; }
        HashSet<MouseButton> PressedButtons { get; }
        Point MousePosition { get; }
        Vector2 MouseDelta { get; }
        int ScrollWheelValue { get; }
        int ScrollWheelDelta { get; }

        void EndOfFrame();
    }
}

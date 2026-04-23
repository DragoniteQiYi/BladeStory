using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Core.Commands
{
    public struct MoveCommand : ICommand
    {
        public bool IsValid { get; }

        public GameTime GameTime { get; }

        public Vector2 MoveDirection { get; set; }
    }
}

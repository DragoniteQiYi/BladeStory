using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Core.Commands
{
    public struct InteractCommand : ICommand
    {
        public bool IsValid { get; }

        public GameTime GameTime { get; }

        public bool Interact { get; set; }

        public InteractCommand(bool isValid, GameTime gameTime, bool interact)
        {
            IsValid = isValid;
            GameTime = gameTime;
            Interact = interact;
        }
    }
}

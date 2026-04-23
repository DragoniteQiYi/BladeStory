using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Core.Commands
{
    public struct DashCommand : ICommand
    {
        public bool IsValid { get; }

        public GameTime GameTime { get; }

        public bool Interact { get; set; }
    }
}

using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Core.Commands
{
    public struct DashCommand : ICommand
    {
        public bool IsValid { get; set; }

        public GameTime GameTime { get; set; }

        public bool IsDashing { get; set; }
    }
}

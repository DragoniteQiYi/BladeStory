using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Services
{
    public class TimeManager : ITimeManager, IUpdate
    {
        private readonly Game _game;

        public GameTime GameTime { get; private set; }

        public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;

        public TimeManager(Game game)
        {
            _game = game;
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}

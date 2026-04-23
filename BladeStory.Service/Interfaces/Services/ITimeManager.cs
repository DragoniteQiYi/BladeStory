using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Services
{
    public interface ITimeManager
    {
        GameTime GameTime { get; }

        float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;

        void Update(GameTime gameTime);
    }
}

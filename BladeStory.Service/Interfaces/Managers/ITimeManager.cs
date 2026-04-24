using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Managers
{
    public interface ITimeManager
    {
        GameTime GameTime { get; }

        float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;

        void Update(GameTime gameTime);
    }
}

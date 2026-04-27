using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces.Managers
{
    public interface IBackgroundManager
    {
        void LoadBackground(string backgroundId);

        void Draw(SpriteBatch spriteBatch);
    }
}

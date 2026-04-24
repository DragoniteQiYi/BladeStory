using BladeStory.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces.Managers
{
    public interface ISceneManager
    {
        Scene? CurrentScene { get; }

        event Action<Scene> OnSceneLoaded;
        event Action<Scene> OnSceneUnloaded;

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void LoadScene(string sceneId);

        void LoadSceneAsync(string sceneId, Action<Scene> onComplete);
    }
}

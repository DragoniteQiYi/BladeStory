using BladeStory.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces
{
    public interface ISceneManager
    {
        Scene CurrentScene { get; }

        event Action<Scene> OnSceneLoaded;
        event Action<Scene> OnSceneUnloaded;

        void LoadSceneConfigs();

        void LoadScene(string sceneId);

        void LoadScene(Scene scene);

        void LoadSceneAsync(string sceneId, Action<Scene> onComplete);

        void LoadSceneAsync(Scene scene, Action<Scene> onComplete);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}

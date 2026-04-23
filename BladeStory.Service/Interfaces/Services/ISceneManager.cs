using BladeStory.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces.Services
{
    public interface ISceneManager
    {
        Scene CurrentScene { get; }

        event Action<Scene> OnSceneLoaded;
        event Action<Scene> OnSceneUnloaded;

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void LoadSceneConfigs();

        void LoadScene(string sceneId);

        void LoadSceneAsync(string sceneId, Action<Scene> onComplete);

        void CreateGameObject(Texture2D texture, Vector2 position);
    }
}

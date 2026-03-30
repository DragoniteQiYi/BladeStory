using BladeStory.Core;
using BladeStory.Service.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Services
{
    public class SceneManager : ISceneManager
    {
        private Scene _currentScene;
        private bool _isTransitioning;

        public Scene CurrentScene => _currentScene;

        public event Action<Scene>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        public void LoadScene(Scene scene)
        {
            _currentScene?.UnloadContent();

            _currentScene = scene;
            _currentScene?.LoadContent();
        }

        public void LoadSceneAsync(Scene scene, Action<Scene> onComplete = null)
        {

        }

        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }
    }
}

using BladeStory.Core;
using BladeStory.Service.Interfaces;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Services
{
    public class SceneService : ISceneService
    {
        private readonly Game _game;
        private Scene _currentScene;
        private bool _isTransitioning;

        public required Scene CurrentScene { get; set; }

        public event Action<Scene> OnSceneLoaded;
        public event Action<Scene> OnSceneUnloaded;

        public SceneService(Game game)
        {
            _game = game;
        }

        public void LoadScene(Scene scene)
        {
            CurrentScene?.ReleaseContent();

            CurrentScene = scene;
            CurrentScene?.LoadContent();
        }

        public void LoadSceneAsync(Scene scene, Action<Scene> onComplete = null)
        {

        }

        public void Update(GameTime gameTime)
        {

        }
    }
}

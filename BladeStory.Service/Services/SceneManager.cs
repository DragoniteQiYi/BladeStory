using BladeStory.Configuration;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Services
{
    public class SceneManager : ISceneManager
    {
        private readonly IConfigManager _configManager;

        public Scene CurrentScene => _currentScene;

        public event Action<Scene>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        private Scene _currentScene;
        private bool _isTransitioning;
        private Dictionary<string, Scene> _scenes = [];
        private Dictionary<string, SceneConfig> _sceneConfigs = [];

        public SceneManager(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public void LoadSceneConfigs()
        {
            Task.Run(async () =>
            {
                _sceneConfigs = await _configManager
                .LoadConfig<Dictionary<string, SceneConfig>>("Content\\Configs\\Scene.json");

                Console.WriteLine($"[SceneManager]: 已经加载{_sceneConfigs.Count}条配置");
                foreach (var sceneConfig in _sceneConfigs)
                {
                    Console.WriteLine($"{sceneConfig.Key}");
                }
            });
        }

        public void LoadScene(string sceneId)
        {
            _currentScene?.UnloadContent();

            
        }

        public void LoadScene(Scene scene)
        {
            _currentScene?.UnloadContent();

            _currentScene = scene;
            _currentScene?.LoadContent();
        }

        public void LoadSceneAsync(string sceneId, Action<Scene> onComplete)
        {

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

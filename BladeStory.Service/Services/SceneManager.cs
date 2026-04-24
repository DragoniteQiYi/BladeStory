using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Services
{
    public class SceneManager : ISceneManager, IStartable
    {
        private readonly IConfigManager _configManager;
        private readonly ContentManager _contentManager;
        private readonly ISceneFactory _sceneFactory;
        private readonly ITiledMapRendererFactory _tiledMapRendererFactory;
        private readonly IEntityFactory _entityFactory;

        public Scene? CurrentScene 
        { 
            get => _currentScene;
            private set { } 
        }

        public event Action<Scene>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        private bool _isTransitioning;
        private Scene? _currentScene;
        private Dictionary<string, Scene> _scenes = [];
        private Dictionary<string, SceneConfig> _sceneConfigs = [];

        public SceneManager(IConfigManager configManager,
            ContentManager contentManager,
            ISceneFactory sceneFactory,
            ITiledMapRendererFactory tiledMapRendererFactory,
            IEntityFactory gameObjectFactory)
        {
            _configManager = configManager;
            _contentManager = contentManager;
            _sceneFactory = sceneFactory;
            _tiledMapRendererFactory = tiledMapRendererFactory;
            _entityFactory = gameObjectFactory;

            Console.WriteLine($"[SceneManager]: 场景管理模块初始化成功");
        }

        public void Initialize()
        {
            _sceneConfigs = _configManager
                .LoadConfig<Dictionary<string, SceneConfig>>("Content\\Configs\\Scene.json");

            Console.WriteLine($"[SceneManager]: 已经加载{_sceneConfigs.Count}条配置");
            foreach (var sceneConfig in _sceneConfigs)
            {
                Console.WriteLine($"{sceneConfig.Key}");
            }
        }

        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }

        public void LoadScene(string sceneId)
        {
            if (_isTransitioning) return;

            CurrentScene?.UnloadContent();
            _isTransitioning = true;

            var sceneConfig = _sceneConfigs[sceneId];
            var type = _sceneConfigs[sceneId].Type;
            var tiledMapId = _sceneConfigs[sceneId].TiledMap;
            TiledMap map;

            if (type == SceneType.Tiled)
            {
                map = _contentManager.Load<TiledMap>(tiledMapId);
                _currentScene = _sceneFactory.CreateTiledScene(sceneConfig, map);
            }

            _currentScene?.LoadContent(_contentManager);
            OnSceneLoaded?.Invoke(_currentScene);
            _isTransitioning = false;
        }

        public void LoadSceneAsync(string sceneId, Action<Scene> onComplete)
        {

        }
    }
}

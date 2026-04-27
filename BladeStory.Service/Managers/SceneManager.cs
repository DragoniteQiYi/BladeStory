using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Managers
{
    public class SceneManager : ISceneManager, IStartable
    {
        private readonly IConfigManager _configManager;
        private readonly ContentManager _contentManager;
        private readonly ISceneFactory _sceneFactory;
        private readonly ITiledMapRendererFactory _tiledMapRendererFactory;
        private readonly ITileMapManager _tileMapManager;
        private readonly IEntityManager _entityManager;
        private readonly IBackgroundManager _backgroundManager;

        public Scene? CurrentScene 
        { 
            get => _currentScene;
            private set { } 
        }

        public event Action<Scene?>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        private bool _isTransitioning;
        private Scene? _currentScene;
        private Dictionary<string, SceneConfig> _sceneConfigs = [];

        public SceneManager(IConfigManager configManager,
            ContentManager contentManager,
            ISceneFactory sceneFactory,
            ITiledMapRendererFactory tiledMapRendererFactory,
            ITileMapManager tileMapManager,
            IEntityManager entityManager,
            IBackgroundManager backgroundManager)
        {
            _configManager = configManager;
            _contentManager = contentManager;
            _sceneFactory = sceneFactory;
            _tiledMapRendererFactory = tiledMapRendererFactory;
            _tileMapManager = tileMapManager;
            _entityManager = entityManager;
            _backgroundManager = backgroundManager;

            Console.WriteLine($"[SceneManager]: 场景管理模块初始化成功");
        }

        public void Initialize()
        {
#if DEBUG
            _sceneConfigs = _configManager
                .LoadConfig<Dictionary<string, SceneConfig>>("Content\\Configs\\Scene.json");
#endif

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

            _tileMapManager.UnloadMap();
            CurrentScene?.UnloadContent();
            _isTransitioning = true;

            var sceneConfig = _sceneConfigs[sceneId];
            var type = sceneConfig.Type;
            var tiledMapId = sceneConfig.TiledMap;
            var backgroundId = sceneConfig.Background;

            if (!string.IsNullOrEmpty(backgroundId))
            {
                _backgroundManager.LoadBackground(backgroundId);
            }

        
            TiledMap map;

            if (type == SceneType.Tiled)
            {
                map = _tileMapManager.LoadMap(tiledMapId);
                _currentScene = _sceneFactory.CreateScene(sceneConfig);

                _entityManager.CurrentScene = _currentScene;

                // 生成地图实体
                var mapObjects = _tileMapManager.AllObjects;
                foreach (var mapObject in mapObjects)
                {
                    _entityManager.Spawn(mapObject.Id, mapObject.Position);
                }
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

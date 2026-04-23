using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Services
{
    public class SceneManager : ISceneManager
    {
        private readonly IConfigManager _configManager;
        private readonly ContentManager _contentManager;
        private readonly ISceneFactory _sceneFactory;
        private readonly ITiledMapRendererFactory _tiledMapRendererFactory;
        private readonly IEntityFactory _gameObjectFactory;

        public Scene? CurrentScene { get; private set; }

        public event Action<Scene>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        private bool _isTransitioning;
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
            _gameObjectFactory = gameObjectFactory;

            Console.WriteLine($"[SceneManager]: 场景管理模块初始化成功");
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene?.Draw(spriteBatch);
        }

        /*
         * 由于加载只发生在游戏初次启动
         * 所以不需要异步，避免出现时序问题
         */
        public void LoadSceneConfigs()
        {
            _sceneConfigs = _configManager
                .LoadConfig<Dictionary<string, SceneConfig>>("Content\\Configs\\Scene.json");

            Console.WriteLine($"[SceneManager]: 已经加载{_sceneConfigs.Count}条配置");
            foreach (var sceneConfig in _sceneConfigs)
            {
                Console.WriteLine($"{sceneConfig.Key}");
            }
        }

        public void LoadScene(string sceneId)
        {
            CurrentScene?.UnloadContent();

            var sceneConfig = _sceneConfigs[sceneId];
            var type = _sceneConfigs[sceneId].Type;
            var tiledMapId = _sceneConfigs[sceneId].TiledMap;
            TiledMap map;

            if (type == SceneType.Tiled)
            {
                map = _contentManager.Load<TiledMap>(tiledMapId);
                CurrentScene = _sceneFactory.CreateTiledScene(sceneConfig, map);
            }

            CurrentScene?.LoadContent(_contentManager);
            OnSceneLoaded?.Invoke(CurrentScene);
        }

        public void LoadSceneAsync(string sceneId, Action<Scene> onComplete)
        {

        }

        public void CreateGameObject(Texture2D texture, Vector2 position)
        {
            var gameObject = _gameObjectFactory.CreateEntity(texture, position);
            CurrentScene?.GameObjects.Add(gameObject);
        }
    }
}

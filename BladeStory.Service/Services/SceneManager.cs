using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
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

        public Scene? CurrentScene => _currentScene;

        public event Action<Scene>? OnSceneLoaded;
        public event Action<Scene>? OnSceneUnloaded;

        private Scene? _currentScene;
        private bool _isTransitioning;
        private Dictionary<string, Scene> _scenes = [];
        private Dictionary<string, SceneConfig> _sceneConfigs = [];

        public SceneManager(IConfigManager configManager,
            ContentManager contentManager,
            ISceneFactory sceneFactory,
            ITiledMapRendererFactory tiledMapRendererFactory)
        {
            _configManager = configManager;
            _contentManager = contentManager;
            _sceneFactory = sceneFactory;
            _tiledMapRendererFactory = tiledMapRendererFactory;

            Console.WriteLine($"[SceneManager]: 场景管理模块初始化成功");
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
            _currentScene?.UnloadContent();

            var sceneConfig = _sceneConfigs[sceneId];
            var type = _sceneConfigs[sceneId].Type;
            var tiledMapId = _sceneConfigs[sceneId].TiledMap;
            TiledMap map;

            if (type == SceneType.Tiled)
            {
                map = _contentManager.Load<TiledMap>(tiledMapId);
                _currentScene = _sceneFactory.CreateTiledScene(sceneConfig, map);
            }

            _currentScene.LoadContent(_contentManager);
        }

        public void LoadScene(Scene scene)
        {
            _currentScene?.UnloadContent();
            _currentScene = scene;
            _currentScene?.LoadContent(_contentManager);
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

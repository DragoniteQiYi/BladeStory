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
    public class SceneManager : ISceneManager, IStartable, IUpdatable
    {
        private readonly IConfigManager _configManager;
        private readonly ContentManager _contentManager;
        private readonly ISceneFactory _sceneFactory;
        private readonly ITiledMapRendererFactory _tiledMapRendererFactory;
        private readonly ITileMapManager _tileMapManager;
        private readonly IEntityManager _entityManager;
        private readonly IBackgroundManager _backgroundManager;
        private readonly IAudioManager _audioManager;
        private readonly IAssetManager _assetManager;

        public Scene? CurrentScene 
        { 
            get => _currentScene;
            private set => _currentScene = value;
        }

        public event Action<SceneConfig>? OnSceneLoad;
        public event Action<SceneConfig>? OnSceneUnload;

        private bool _isTransitioning;
        private Scene? _currentScene;
        private Dictionary<string, SceneConfig> _sceneConfigs = [];

        public SceneManager(IConfigManager configManager,
            ContentManager contentManager,
            ISceneFactory sceneFactory,
            ITiledMapRendererFactory tiledMapRendererFactory,
            ITileMapManager tileMapManager,
            IEntityManager entityManager,
            IBackgroundManager backgroundManager,
            IAudioManager audioManager,
            IAssetManager assetManager)
        {
            _configManager = configManager;
            _contentManager = contentManager;
            _sceneFactory = sceneFactory;
            _tiledMapRendererFactory = tiledMapRendererFactory;
            _tileMapManager = tileMapManager;
            _entityManager = entityManager;
            _backgroundManager = backgroundManager;
            _audioManager = audioManager;
            _assetManager = assetManager;

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

            if (!_sceneConfigs.TryGetValue(sceneId, out var sceneConfig))
            {
#if DEBUG
                Console.WriteLine($"[SceneManager]: 未找到场景配置: {sceneId}");
#endif
                return;
            }

            _isTransitioning = true;

            try
            {
                Scene previousScene = _currentScene;
                SceneConfig previousSceneConfig;
                if (_currentScene != null)
                {
                    previousScene = _currentScene;
                    _sceneConfigs.TryGetValue(previousScene.Id, out previousSceneConfig);
                    _tileMapManager.UnloadMap();
                    previousScene?.UnloadContent();
                    OnSceneUnload?.Invoke(previousSceneConfig);
                }
                
                var type = sceneConfig.Type;
                var tiledMapId = sceneConfig.TiledMap;
                var backgroundId = sceneConfig.Background;

                if (!string.IsNullOrEmpty(backgroundId))
                {
                    _backgroundManager.LoadBackground(backgroundId);
                }
                else
                {
                    _backgroundManager.ClearBackground();
                }

                if (type == SceneType.Tiled)
                {
                    _tileMapManager.LoadMap(tiledMapId);
                    _currentScene = _sceneFactory.CreateScene(sceneConfig, 
                        _tileMapManager.Width * _tileMapManager.TileWidth,
                        _tileMapManager.Height * _tileMapManager.TileHeight);
                    _entityManager.CurrentScene = _currentScene;

                    // 生成地图实体
                    var mapObjects = _tileMapManager.AllObjects;
                    foreach (var mapObject in mapObjects)
                    {
                        _entityManager.Spawn(mapObject.Id, mapObject.Position);
                    }
                }

                HandleMusicPlaying(sceneConfig.Bgm);

                _currentScene?.LoadContent(_contentManager);
                OnSceneLoad?.Invoke(sceneConfig);
                _isTransitioning = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[SceneManager]: 加载场景失败 - {ex.Message}");
#endif
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public void LoadSceneAsync(string sceneId, Action<Scene> onComplete)
        {

        }

        private void HandleMusicPlaying(string? bgmId)
        {
            if (!string.IsNullOrEmpty(bgmId))
            {
                if (!string.IsNullOrEmpty(_audioManager.PlayingMusicId)
                    && !_audioManager.PlayingMusicId.Equals(bgmId))
                {
                    _audioManager.PlayMusic(bgmId);
                }
                else if (!_audioManager.IsMusicPlaying)
                {
                    _audioManager.PlayMusic(bgmId);
                }
            }
            else
            {
                _audioManager.StopMusic();
            }
        }
    }
}

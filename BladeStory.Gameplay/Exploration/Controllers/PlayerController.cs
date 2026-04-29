using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Components;
using BladeStory.Gameplay.Exploration.Models;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using BladeStory.Service.Middlewares;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace BladeStory.Gameplay.Exploration.Controllers
{
    public class PlayerController : IDisposable, IStartable
    {
        private readonly PlayerCommandMiddleware _playerCommandMiddleware;
        private readonly ContentManager _contentManager;
        private readonly IEntityManager _entityManager;
        private readonly ISceneManager _sceneManager;

        public bool PlayerVisible { get; set; }

        private Player? _playerCharacter;

        public PlayerController(PlayerCommandMiddleware playerCommandMiddleware,
            ContentManager contentManager,
            IEntityManager entityManager,
            ISceneManager sceneManager)
        {
            _playerCommandMiddleware = playerCommandMiddleware;
            _contentManager = contentManager;
            _entityManager = entityManager;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            RegisterEvents();
        }

        private void HandleSceneLoaded(SceneConfig sceneConfig)
        {
            if (sceneConfig.Type == SceneType.Tiled)
            {
                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            var texture = _contentManager.Load<Texture2D>("Sprites/Characters/Hero");
            _playerCharacter = new Player(texture, new Vector2(10, 10));
            _entityManager.Spawn(_playerCharacter);
        }

        private void OnCommandReceived(ICommand command)
        {
            _playerCharacter?.ReceiveCommand(command);
        }

        //private void OnMove(MoveCommand command)
        //{
        //    _playerCharacter?.Move(command.MoveDirection);
        //}

        //private void OnInteract(InteractCommand command)
        //{
        //    _playerCharacter?.Interact();
        //}

        //private void OnDash(DashCommand command)
        //{
        //    _playerCharacter?.SetDashing(command.IsDashing);
        //}

        public void Dispose()
        {
            _playerCharacter = null;
            _playerCommandMiddleware.OnCommandSpawned -= OnCommandReceived;

            _sceneManager.OnSceneLoad -= HandleSceneLoaded;
        }

        private void RegisterEvents()
        {
            _playerCommandMiddleware.OnCommandSpawned += OnCommandReceived;

            _sceneManager.OnSceneLoad += HandleSceneLoaded;
        }
    }
}

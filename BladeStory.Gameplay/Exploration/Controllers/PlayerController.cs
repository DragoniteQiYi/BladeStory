using BladeStory.Core.Commands;
using BladeStory.Core.Components;
using BladeStory.Gameplay.Exploration.Models;
using BladeStory.Service.Interfaces.Middlewares;
using BladeStory.Service.Middlewares;


namespace BladeStory.Gameplay.Exploration.Controllers
{
    public class PlayerController : IDisposable
    {
        private readonly PlayerCommandMiddleware _playerCommandMiddleware;

        private Player _player;

        public PlayerController(PlayerCommandMiddleware playerCommandMiddleware)
        {
            _playerCommandMiddleware = playerCommandMiddleware;

            _playerCommandMiddleware.OnMoveCommand += OnMove;
            _playerCommandMiddleware.OnInteractCommand += OnInteract;
            _playerCommandMiddleware.OnDashCommand += OnDash;
        }

        private void OnMove(MoveCommand command)
        {
            _player?.Move(command.MoveDirection);
        }

        private void OnInteract(InteractCommand command)
        {
            _player?.Interact();
        }

        private void OnDash(DashCommand command)
        {
            _player.IsDashing = command.IsDashing;
        }

        public void Dispose()
        {
            _player = null;
            _playerCommandMiddleware.OnMoveCommand -= OnMove;
            _playerCommandMiddleware.OnInteractCommand -= OnInteract;
            _playerCommandMiddleware.OnDashCommand -= OnDash;
        }
    }
}

using BladeStory.Core.Components;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Gameplay.Exploration.Model
{
    public class Player : Entity, IControllable, IMoveable, IInteractor
    {
        public float Speed { get; private set; }
        public bool CanMove { get; private set; }
        public Vector2 Velocity { get; private set; }
        public bool InputEnabled { get; private set; }

        public Player(Texture2D texture) : base(texture) { }

        public Player(Texture2D texture, Vector2 position) : base(texture, position) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void EnableInput(bool state)
        {
            InputEnabled = state;
        }

        public void ReceiveCommand(ICommand command)
        {
            throw new NotImplementedException();
        }

        public void Move(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Vector2 targetPosition)
        {
            throw new NotImplementedException();
        }

        public void Interact(IInteractable target)
        {
            throw new NotImplementedException();
        }
    }
}

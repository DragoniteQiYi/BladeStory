using BladeStory.Core.Commands;
using BladeStory.Core.Components;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Gameplay.Exploration.Models
{
    public class Player : Entity, IControllable, IMoveable, IInteractor
    {
        private const float MOVE_SPEED = 200f;
        private const float ACCELERATION = 12f;
        private const float DECELERATION = 8f;

        public float Speed { get; private set; }
        public bool CanMove { get; private set; } = true;
        public Vector2 Velocity { get; private set; }
        public bool InputEnabled { get; private set; } = true;
        public bool IsDashing { get; set; }

        public Player(Texture2D texture) : base(texture) { }

        public Player(Texture2D texture, Vector2 position) : base(texture, position) { }

        public override void Initialize()
        {
            Speed = MOVE_SPEED;
            Velocity = Vector2.Zero;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (CanMove)
            {
                Position += Velocity * deltaTime;
            }

            base.Update(gameTime);
        }

        public void EnableInput(bool state)
        {
            InputEnabled = state;
        }

        public void ReceiveCommand(ICommand command)
        {
            if (!InputEnabled) return;

            if (command is MoveCommand)
            {
                var moveCommand = (MoveCommand)command;
                Move(moveCommand.MoveDirection);
            }
            else if (command is InteractCommand)
            {
                var interactCommand = (InteractCommand)command;
                Interact();
            }
            else if (command is DashCommand)
            {
                var dashCommand = (DashCommand)command;
                SetDashing(dashCommand.IsDashing);
            }
        }

        public void Move(Vector2 direction)
        {
            if (!CanMove) return;

            if (direction != Vector2.Zero)
            {
                // 有输入时，平滑加速到目标速度
                direction.Normalize();
                float currentSpeed = IsDashing ? Speed * 2f : Speed;
                Vector2 targetVelocity = direction * currentSpeed;
                Velocity = Vector2.Lerp(Velocity, targetVelocity, ACCELERATION * 0.016f);
            }
            else
            {
                // 无输入时，平滑减速到0
                Velocity = Vector2.Lerp(Velocity, Vector2.Zero, DECELERATION * 0.016f);

                // 速度很小时直接归零
                if (Velocity.Length() < 0.5f)
                {
                    Velocity = Vector2.Zero;
                }
            }
        }

        public void MoveTo(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - Position;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                Move(direction);
            }
        }

        public void SetDashing(bool state)
        {
            IsDashing = state;
        }

        public void Interact()
        {
            
        }

        public void InteractWith(IInteractable target)
        {

        }
    }
}

using BladeStory.Core.Commands;
using BladeStory.Core.Components;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BladeStory.Gameplay.Exploration.Models.Entities
{
    public class Player : Entity, IControllable, IMoveable, IInteractor
    {
        private const float MOVE_SPEED = 200f;

        public float Speed { get; private set; }
        public bool CanMove { get; private set; } = true;
        public Vector2 Velocity { get; private set; }
        public bool InputEnabled { get; private set; } = true;
        public bool IsDashing { get; set; }
        public override IShapeF? Bounds { get; protected set; }
        public override string LayerName { get; set; } = "default";

        public Player(Texture2D texture) : base(texture) 
        {
            _boundsOffset = new(-8f, -8f);
            _boundsWidth = 15f;
            _boundsHeight = 15f;

            Bounds = new RectangleF(_boundsOffset.X, _boundsOffset.Y, _boundsWidth, _boundsHeight);
        }

        public Player(Texture2D texture, Vector2 position) : base(texture, position) 
        {
            _boundsOffset = new(-8f, -8f);
            _boundsWidth = 15f;
            _boundsHeight = 15f;

            Vector2 basePoint = position + _boundsOffset;
            Bounds = new RectangleF(basePoint.X, basePoint.Y, _boundsWidth, _boundsHeight);
        }

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

            Bounds.Position = Position + _boundsOffset;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 1);
            base.Draw(spriteBatch);
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
                Velocity = direction * currentSpeed;
            }
            else
            {
                Velocity = Vector2.Zero;
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

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity = new Vector2(0, 0);
            Bounds.Position -= collisionInfo.PenetrationVector;
            Position -= collisionInfo.PenetrationVector;
        }
    }
}

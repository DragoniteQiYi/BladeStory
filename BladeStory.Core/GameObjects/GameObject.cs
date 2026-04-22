using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;

namespace BladeStory.Core.GameObjects
{
    /// <summary>
    /// 通用的游戏场景对象
    /// </summary>
    public class GameObject : ICollisionActor
    {
        // -----标识-----
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }

        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public Transform2 Transform { get; set; }
        public Sprite Sprite { get; set; }

        public GameObject? Parent { get; set; }
        public List<GameObject> Children { get; private set; } = [];

        // 使用 Transform2 管理变换
        public Vector2 Position
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public float Rotation
        {
            get => Transform.Rotation;
            set => Transform.Rotation = value;
        }

        public Vector2 Scale
        {
            get => Transform.Scale;
            set => Transform.Scale = value;
        }

        public SpriteEffects Effect
        {
            get => Sprite.Effect;
            set => Sprite.Effect = value;
        }

        public IShapeF Bounds { get; private set; }

        public GameObject(Texture2D texture)
        {
            Sprite = new Sprite(texture);
            Transform = new Transform2();
            Sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            foreach (var child in Children)
            {
                child?.Update(gameTime);
            }
        }

        public virtual void LoadContent() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive || !IsVisible) return;
            // 同步变换到 Sprite
            foreach (var child in Children)
            {
                child?.Draw(spriteBatch);
            }

            spriteBatch.Draw(Sprite, Transform.Position);
        }

        public Vector2 GetWorldPosition()
        {
            if (Parent != null)
            {
                return Parent.GetWorldPosition() + Position;
            }
            return Position;
        }

        public virtual Rectangle GetBounds()
        {
            var bounds = Sprite.GetBoundingRectangle(Transform);
            return new Rectangle(
                (int)bounds.X,
                (int)bounds.Y,
                (int)bounds.Width,
                (int)bounds.Height
            );
        }

        public bool OnCollisionEnter(GameObject other)
        {
            return GetBounds().Intersects(other.GetBounds());
        }

        public virtual void Destroy()
        {
            Sprite.TextureRegion.Texture?.Dispose();
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}

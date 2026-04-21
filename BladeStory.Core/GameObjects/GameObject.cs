using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;

namespace BladeStory.Core.GameObjects
{
    /// <summary>
    /// 通用的游戏场景对象
    /// </summary>
    public abstract class GameObject
    {
        // -----标识-----
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public GameObject? Parent { get; set; }
        public List<GameObject> Children { get; private set; } = [];

        protected Transform2 _transform;
        protected Sprite _sprite;

        // 使用 Transform2 管理变换
        public Vector2 Position
        {
            get => _transform.Position;
            set => _transform.Position = value;
        }

        public float Rotation
        {
            get => _transform.Rotation;
            set => _transform.Rotation = value;
        }

        public Vector2 Scale
        {
            get => _transform.Scale;
            set => _transform.Scale = value;
        }

        public SpriteEffects Effect
        {
            get => _sprite.Effect;
            set => _sprite.Effect = value;
        }

        public GameObject(Texture2D texture)
        {
            _sprite = new Sprite(texture);
            _transform = new Transform2();
            _sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            foreach (var child in Children)
            {
                child.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive || !IsVisible) return;
            // 同步变换到 Sprite

            spriteBatch.Draw(_sprite, _transform.Position);
            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
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
            var bounds = _sprite.GetBoundingRectangle(_transform);
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
            _sprite.TextureRegion.Texture?.Dispose();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Core
{
    /// <summary>
    /// 通用的游戏场景对象
    /// </summary>
    public abstract class GameObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public float Rotation { get; set; }
        public Color Color { get; set; } = Color.White;
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public GameObject? Parent { get; set; }

        protected Texture2D? _texture;
        protected Rectangle _sourceRectangle;
        protected Vector2 _originalPosition;

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            if (texture != null)
            {
                _originalPosition = new Vector2(texture.Width / 2f, texture.Height / 2f);
            }
        }

        // 更新逻辑
        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;
        }

        // 渲染
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive || !IsVisible || _texture == null) return;

            // 计算世界位置
            Vector2 worldPosition = GetWorldPosition();

            spriteBatch.Draw(
                _texture,
                worldPosition,
                _sourceRectangle,
                Color,
                Rotation,
                _originalPosition,
                Scale,
                SpriteEffects.None,
                0f
            );
        }

        // 获取世界坐标（考虑父对象）
        public Vector2 GetWorldPosition()
        {
            if (Parent != null)
            {
                return Parent.GetWorldPosition() + Position;
            }
            return Position;
        }

        // 获取包围盒（用于碰撞检测）
        public virtual Rectangle GetBounds()
        {
            if (_texture == null) return Rectangle.Empty;

            Vector2 worldPos = GetWorldPosition();
            int width = (int)(_texture.Width * Scale.X);
            int height = (int)(_texture.Height * Scale.Y);

            return new Rectangle(
                (int)(worldPos.X - _originalPosition.X * Scale.X),
                (int)(worldPos.Y - _originalPosition.Y * Scale.Y),
                width,
                height
            );
        }

        // 碰撞检测
        public bool OnCollisionEnter(GameObject other)
        {
            return GetBounds().Intersects(other.GetBounds());
        }

        // 清理资源
        public virtual void Destory()
        {
            if (_texture != null)
            {
                _texture = null;
            }
        }
    }
}

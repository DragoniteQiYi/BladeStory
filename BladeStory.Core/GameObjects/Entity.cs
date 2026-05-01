using BladeStory.Core.Components;
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
    public class Entity : IEntity, ICollisionActor
    {
        // -----标识-----
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        // -----组件-----
        public Transform2 Transform { get; set; }
        public Sprite? Sprite { get; set; }
        public virtual IShapeF? Bounds { get; protected set; } = new RectangleF();
        public virtual string LayerName { get; set; } = "entity";

        protected Vector2 _boundsOffset;
        protected float _boundsWidth;
        protected float _boundsHeight;

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

        public Entity() 
        {
            Id = Guid.NewGuid();
            Transform = new Transform2(Vector2.Zero);
        }

        public Entity(Vector2 position)
        {
            Id = Guid.NewGuid();
            Transform = new Transform2(Vector2.Zero);
        }

        public Entity(Texture2D texture)
        {
            Id = Guid.NewGuid();
            Sprite = new Sprite(texture);
            Transform = new Transform2(Vector2.Zero);
            Sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }


        public Entity(Texture2D texture, Vector2 position)
        {
            Id = Guid.NewGuid();
            Sprite = new Sprite(texture);
            Transform = new Transform2(position);
            Sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public Entity(string name, Texture2D texture, Vector2 position, 
            Vector2 boundsOffset, float width, float height)
        {
            Id = Guid.NewGuid();
            Name = name;
            Sprite = new Sprite(texture);
            Transform = new Transform2(position);
            Sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            _boundsOffset = boundsOffset;
            _boundsWidth = width;
            _boundsHeight = height;

            Vector2 basePoint = position + boundsOffset;
            Bounds = new RectangleF(basePoint.X, basePoint.Y, width, height);
        }

        public virtual void Initialize() { }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            if (Bounds != null)
            {
                Bounds.Position = Position + _boundsOffset;
            }
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive || !IsVisible) return;

            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 1);

            spriteBatch.Draw(Sprite, Transform.Position);
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo) 
        {

        }


        public virtual void Destroy()
        {
            Sprite?.TextureRegion.Texture?.Dispose();
        }
    }
}

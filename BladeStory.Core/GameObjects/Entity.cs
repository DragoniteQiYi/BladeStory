using BladeStory.Core.Components;
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
    public class Entity : IEntity
    {
        // -----标识-----
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        // -----组件-----
        public Transform2 Transform { get; set; }
        public Sprite Sprite { get; set; }

        public Entity? Parent { get; set; }
        public List<Entity> Children { get; private set; } = [];

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

        public Entity(string name, Texture2D texture, Vector2 position)
        {
            Id = Guid.NewGuid();
            Name = name;
            Sprite = new Sprite(texture);
            Transform = new Transform2(position);
            Sprite.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public virtual void Initialize() { }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            foreach (var child in Children)
            {
                child?.Update(gameTime);
            }
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

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


        public virtual void Destroy()
        {
            Sprite.TextureRegion.Texture?.Dispose();
        }

        public Vector2 GetWorldPosition()
        {
            if (Parent != null)
            {
                return Parent.GetWorldPosition() + Position;
            }
            return Position;
        }
    }
}

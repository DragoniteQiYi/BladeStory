using BladeStory.Configuration;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Collisions.QuadTree;

namespace BladeStory.Core.Scenes
{
    /*
     * 场景的唯一职责：负责持有并更新场景对象
     * 它不负责：加载或销毁资源和对象 
     */
    public class Scene
    {
        protected readonly SceneConfig _sceneConfig;

        public string Id { get; }

        public float Width { get; }

        public float Height { get; }

        public List<Entity> Entities { get; } = [];

        public readonly CollisionComponent _collisionComponent;
        private readonly QuadTreeSpace _quadTreeSpace;

        public Scene(SceneConfig sceneConfig, float width, float height)
        {
            _sceneConfig = sceneConfig;
            Id = sceneConfig.Id;
            Width = width;
            Height = height;
            _quadTreeSpace = new QuadTreeSpace(new RectangleF(0, 0, width, height));
            _collisionComponent = new CollisionComponent(
                new RectangleF(0, 0, width, height));

            var layer = new Layer(_quadTreeSpace);
            _collisionComponent.Add("entity", layer);
        }

        public virtual void LoadContent(ContentManager contentManager) 
        {
            foreach (var entity in Entities)
                entity.LoadContent();
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
                entity.Update(gameTime);

            _collisionComponent.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in Entities)
                entity.Draw(spriteBatch);
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            _collisionComponent.Insert(entity);
            Console.WriteLine($"Entity Layer: {entity.LayerName}");
            Console.WriteLine($"Entity Bounds: {entity.Bounds}");
        }
    }
}

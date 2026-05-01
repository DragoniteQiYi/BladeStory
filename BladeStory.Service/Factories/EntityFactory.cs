using BladeStory.Configuration;
using BladeStory.Core.GameObjects;
using BladeStory.Service.Interfaces.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Factories
{
    public class EntityFactory : IEntityFactory
    {
        private readonly ContentManager _contentManager;

        public EntityFactory(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public Entity CreateEntity(EntityConfig entityConfig, Vector2 position)
        {
            var name = entityConfig.Name;
            var texture = _contentManager.Load<Texture2D>(entityConfig.Texture);
            var boundsOffset = entityConfig.BoundsOffset;
            var width = entityConfig.Width;
            var height = entityConfig.Height;
            return new Entity(name, texture, position, boundsOffset, width, height);
        }
    }
}

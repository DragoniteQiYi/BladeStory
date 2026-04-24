using BladeStory.Configuration;
using BladeStory.Core.GameObjects;
using BladeStory.Service.Interfaces.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
            throw new NotImplementedException();
        }
    }
}

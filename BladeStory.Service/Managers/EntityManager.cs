using BladeStory.Configuration;
using BladeStory.Core.Components;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Managers
{
    public class EntityManager : IEntityManager, IStartable
    {
        private readonly ISceneManager _sceneManager;
        private readonly IEntityFactory _entityFactory;
        private readonly IConfigManager _configManager;

        private Dictionary<string, EntityConfig> _entityConfigs = [];

        public EntityManager(ISceneManager sceneManager, 
            IEntityFactory entityFactory, 
            IConfigManager configManager)
        {
            _sceneManager = sceneManager;
            _entityFactory = entityFactory;
            _configManager = configManager;
        }

        public void Initialize()
        {

        }

        public IEntity Spawn(string id, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void Destroy(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

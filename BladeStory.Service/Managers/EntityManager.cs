using BladeStory.Configuration;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Factories;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Managers
{
    /*
     * 只负责生成和销毁实体
     * 实体更新归场景管，它全听场景的
     */
    public class EntityManager : IEntityManager, IStartable
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IConfigManager _configManager;

        private Dictionary<string, EntityConfig> _entityConfigs = [];
        private Scene _currentScene;
        
        public Scene CurrentScene 
        { 
            get => _currentScene; 
            set => _currentScene = value; 
        } 

        public EntityManager(IEntityFactory entityFactory, 
            IConfigManager configManager)
        {
            _entityFactory = entityFactory;
            _configManager = configManager;
        }

        public void Initialize()
        {
            LoadConfig();
        }

        public void Spawn(string id, Vector2 position)
        {
            var entity = _entityFactory.CreateEntity(_entityConfigs[id], position);
            _currentScene.GameObjects.Add(entity);
        }

        public void Destroy(Guid id)
        {
            var entity = _currentScene.GameObjects
                .FirstOrDefault(x => x.Id == id);

            _currentScene.GameObjects.Remove(entity);
        }

        private void LoadConfig()
        {
#if DEBUG
            _entityConfigs = _configManager
                .LoadConfig<Dictionary<string, EntityConfig>>("Content\\Configs\\Entity.json");
#endif
        }
    }
}

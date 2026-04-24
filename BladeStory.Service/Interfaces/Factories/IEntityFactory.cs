using BladeStory.Configuration;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface IEntityFactory
    {
        Entity CreateEntity(EntityConfig entityConfig, Vector2 position);
    }
}

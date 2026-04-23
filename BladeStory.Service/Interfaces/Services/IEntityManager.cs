using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Services
{
    public interface IEntityManager
    {
        IEntity Spawn(Vector2 position);
    }
}

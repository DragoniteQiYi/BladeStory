using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface IEntityFactory
    {
        Entity CreateEntity(Texture2D texture, Vector2 position);
    }
}

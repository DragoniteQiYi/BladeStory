using BladeStory.Core.GameObjects;
using BladeStory.Service.Interfaces.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Factories
{
    public class EntityFactory : IEntityFactory
    {
        public Entity CreateEntity(Texture2D texture, Vector2 position)
        {
            if (position == Vector2.Zero)
            {
                return new Entity(texture);
            }
            return new Entity(texture, position);
        }
    }
}

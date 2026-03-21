using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Core
{
    // 场景
    public abstract class Scene
    {
        public List<GameObject> GameObjects { get; } = [];

        public abstract void LoadContent();
        public abstract void ReleaseContent();

        public virtual void Update(GameTime gameTime)
        {
            foreach (var obj in GameObjects)
                obj.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obj in GameObjects)
                obj.Draw(spriteBatch);
        }
    }
}

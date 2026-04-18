using BladeStory.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Core.Scenes
{
    // 场景
    public abstract class Scene(SceneConfig sceneConfig)
    {
        protected readonly SceneConfig _sceneConfig = sceneConfig;

        public string Id { get; } = sceneConfig.Id;

        public List<GameObject> GameObjects { get; } = [];

        public virtual void LoadContent(ContentManager contentManager) 
        {
            //foreach (var obj in GameObjects)
            //    obj.LoadContent();
        }

        public virtual void UnloadContent() { }

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

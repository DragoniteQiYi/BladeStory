using BladeStory.Configuration;
using BladeStory.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Core.Scenes
{
    /*
     * 场景的唯一职责：负责持有并更新场景对象
     * 它不负责：加载或销毁资源和对象 
     */
    public class Scene(SceneConfig sceneConfig)
    {
        protected readonly SceneConfig _sceneConfig = sceneConfig;

        public string Id { get; } = sceneConfig.Id;

        public List<Entity> GameObjects { get; } = [];

        public virtual void LoadContent(ContentManager contentManager) 
        {
            foreach (var obj in GameObjects)
                obj.LoadContent();
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

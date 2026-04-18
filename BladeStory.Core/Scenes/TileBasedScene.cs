using BladeStory.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Core.Scenes
{
    public class TileBasedScene(SceneConfig sceneConfig) : Scene(sceneConfig)
    {
        private TiledMap? _tiledMap;

        public override void LoadContent(ContentManager contentManager)
        {
            _tiledMap = contentManager
                .Load<TiledMap>(_sceneConfig.TiledMap);

            base.LoadContent(contentManager);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

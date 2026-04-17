using BladeStory.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace BladeStory.Core.Scenes
{
    public class TileBasedScene : Scene
    {
        private TiledMap? _tiledMap;

        private readonly ContentManager _contentManager;

        public TileBasedScene(string id,
            SceneConfig sceneConfig,
            ContentManager contentManager) : base(id, sceneConfig)
        {
            _contentManager = contentManager;
        }

        public override void LoadContent()
        {
            _tiledMap = _contentManager
                .Load<TiledMap>(_sceneConfig.TiledMap);

            base.LoadContent();
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

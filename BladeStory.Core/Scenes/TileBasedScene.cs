using BladeStory.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Core.Scenes
{
    public class TileBasedScene : Scene
    {
        private TiledMap _tiledMap;
        private readonly TiledMapRenderer _tiledMapRenderer;

        public TileBasedScene(SceneConfig sceneConfig,
            TiledMap tiledMap,
            TiledMapRenderer tiledMapRenderer) : base(sceneConfig)
        {
            _tiledMapRenderer = tiledMapRenderer;
            _tiledMap = tiledMap;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            //_tiledMap = contentManager
            //    .Load<TiledMap>(_sceneConfig.TiledMap);
            
            base.LoadContent(contentManager);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_tiledMap != null)
            {
                _tiledMapRenderer?.Draw();
            }

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

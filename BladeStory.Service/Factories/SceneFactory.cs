using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Factories
{
    public class SceneFactory(GraphicsDevice graphicsDevice, 
        ITiledMapRendererFactory tiledMapRendererFactory) : ISceneFactory
    {
        private readonly ITiledMapRendererFactory _tiledMapRendererFactory = tiledMapRendererFactory;

        public Scene CreateTiledScene(SceneConfig sceneConfig, TiledMap tiledMap)
        {
            if (sceneConfig.Type == SceneType.Tiled)
            {
                var tiledMapRenderer = tiledMapRendererFactory.CreateTiledMapRenderer(tiledMap);
                return new TileBasedScene(sceneConfig, tiledMap, tiledMapRenderer);
            }
            else
            {
                return new ScreenScene(sceneConfig);
            }
        }
    }
}

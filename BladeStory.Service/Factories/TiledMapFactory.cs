using BladeStory.Service.Interfaces.Factories;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Service.Factories
{
    public class TiledMapRendererFactory(GraphicsDevice graphicsDevice) : ITiledMapRendererFactory
    {
        public TiledMapRenderer CreateTiledMapRenderer
            (TiledMap tiledMap)
        {
            return new TiledMapRenderer(graphicsDevice, tiledMap);
        }
    }
}

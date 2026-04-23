using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface ITiledMapRendererFactory
    {
        TiledMapRenderer CreateTiledMapRenderer(TiledMap tiledMap);
    }
}

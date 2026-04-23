using BladeStory.Configuration;
using BladeStory.Core.Scenes;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface ISceneFactory
    {
        Scene CreateTiledScene(SceneConfig sceneConfig, TiledMap tiledMap);
    }
}

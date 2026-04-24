using BladeStory.Configuration;
using BladeStory.Core.Scenes;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface ISceneFactory
    {
        Scene CreateScene(SceneConfig sceneConfig);
    }
}

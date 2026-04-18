using BladeStory.Configuration;
using BladeStory.Core.Scenes;

namespace BladeStory.Service.Interfaces
{
    public interface ISceneFactory
    {
        Scene CreateScene(SceneConfig sceneConfig);
    }
}

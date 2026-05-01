using BladeStory.Configuration;
using BladeStory.Core.Scenes;

namespace BladeStory.Service.Interfaces.Factories
{
    public interface ISceneFactory
    {
        Scene CreateScene(SceneConfig sceneConfig);

        Scene CreateScene(SceneConfig sceneConfig, float width, float height);
    }
}

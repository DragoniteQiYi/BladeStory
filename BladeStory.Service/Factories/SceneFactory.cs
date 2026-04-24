using BladeStory.Configuration;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces.Factories;

namespace BladeStory.Service.Factories
{
    public class SceneFactory : ISceneFactory
    {
        public Scene CreateScene(SceneConfig sceneConfig)
        {
            return new Scene(sceneConfig);
        }
    }
}

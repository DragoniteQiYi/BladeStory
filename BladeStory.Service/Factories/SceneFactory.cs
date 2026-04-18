using BladeStory.Configuration;
using BladeStory.Constant;
using BladeStory.Core.Scenes;
using BladeStory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeStory.Service.Factories
{
    public class SceneFactory : ISceneFactory
    {
        public Scene CreateScene(SceneConfig sceneConfig)
        {
            if (sceneConfig.Type == SceneType.Tiled)
            {
                return new TileBasedScene(sceneConfig);
            }
            else
            {
                return new ScreenScene(sceneConfig);
            }
        }
    }
}

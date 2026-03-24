using BladeStory.Core;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces
{
    public interface ISceneService
    {
        public Scene CurrentScene { get; set; }

        public event Action<Scene> OnSceneLoaded;
        public event Action<Scene> OnSceneUnloaded;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScene(Scene scene);

        public void LoadSceneAsync(Scene scene, Action<Scene> onComplete);

        public void Update(GameTime gameTime);
    }
}

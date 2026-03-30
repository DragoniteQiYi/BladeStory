using BladeStory.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Interfaces
{
    public interface ISceneManager
    {
        Scene CurrentScene { get; }

        event Action<Scene> OnSceneLoaded;
        event Action<Scene> OnSceneUnloaded;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scene"></param>
        void LoadScene(Scene scene);

        void LoadSceneAsync(Scene scene, Action<Scene> onComplete);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}

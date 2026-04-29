using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using BladeStory.UI.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Myra.Graphics2D.UI;

namespace BladeStory.Service.Managers
{
    public class UIManager : IUIManager, IStartable, IUpdatable
    {
        private readonly ContentManager _contentManager;
        // 脏引用，要重构
        private readonly ISceneManager _sceneManager;
        private readonly Desktop _desktop;

        private MainMenu _mainMenu;

        public UIManager(ContentManager contentManager, ISceneManager sceneManager, Desktop desktop)
        {
            _desktop = desktop;
            _sceneManager = sceneManager;
            _contentManager = contentManager;
        }

        public void Initialize()
        {
            _mainMenu = new();
            _mainMenu._menuNewGame.Selected += (s, e) =>
            {
                Console.WriteLine("大傻逼");
                _sceneManager.LoadScene("Scenes/Town/Original");
                _desktop.Root = null;
            };
            _desktop.Root = _mainMenu;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}

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
        private readonly Desktop _desktop;

        private MainMenu _mainMenu;

        public UIManager(ContentManager contentManager, Desktop desktop)
        {
            _desktop = desktop;
            _contentManager = contentManager;
        }

        public void Initialize()
        {
            _mainMenu = new();
            _desktop.Root = _mainMenu;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}

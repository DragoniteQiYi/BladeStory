using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameGum;

namespace BladeStory.Service.Managers
{
    public class UIManager : IUIManager, IStartable, IUpdatable
    {
        private readonly ContentManager _contentManager;
        private readonly GumService _gumService = GumService.Default;
        

        public UIManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Initialize()
        {


            var screen = ObjectFinder.Self?.GumProjectSave?.Screens
                .Find(item => item.Name == "MainMenu");
            screen?.ToGraphicalUiElement().AddToRoot();

            // 获取 Component
            var component = ObjectFinder.Self?.GumProjectSave?.Components
                .Find(item => item.Name == "MainMenu");
            component?.ToGraphicalUiElement().AddToRoot();

            Console.WriteLine($"Root children count: {_gumService.Root.Children.Count}");
        }

        public void Update(GameTime gameTime)
        {
            _gumService.Update(gameTime);
        }
    }
}

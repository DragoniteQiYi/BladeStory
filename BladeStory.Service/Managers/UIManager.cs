using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.BitmapFonts;

namespace BladeStory.Service.Managers
{
    public class UIManager : IUIManager, IStartable, IUpdatable
    {
        private readonly ContentManager _contentManager;
        private readonly BitmapFont _font;

        public UIManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Initialize()
        {
            
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}

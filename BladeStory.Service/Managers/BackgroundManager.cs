using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladeStory.Service.Managers
{
    public class BackgroundManager : IBackgroundManager
    {
        private readonly ContentManager _contentManager;
        private readonly GraphicsDevice _graphicsDevice;

        private Texture2D? _currentBackground;
        
        public BackgroundManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
        }

        public void LoadBackground(string backgroundId)
        {
            _currentBackground = _contentManager.Load<Texture2D>(backgroundId);
        }

        public void ClearBackground()
        {
            _currentBackground = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentBackground == null) return;

            // 获取当前屏幕（窗口）尺寸
            int screenWidth = _graphicsDevice.Viewport.Width;
            int screenHeight = _graphicsDevice.Viewport.Height;

            // 计算缩放比例，使得图片完全填充屏幕，类似 CSS 的 "cover" 效果
            float scaleX = (float)screenWidth / _currentBackground.Width;
            float scaleY = (float)screenHeight / _currentBackground.Height;

            // 取较大的缩放值，以确保图片能填满整个屏幕，不会出现黑边
            float scale = MathHelper.Max(scaleX, scaleY);

            // 计算缩放后图片的尺寸
            int scaledWidth = (int)(_currentBackground.Width * scale);
            int scaledHeight = (int)(_currentBackground.Height * scale);

            // 计算居中位置：如果图片比屏幕宽/高，则裁剪左右或上下
            int posX = (screenWidth - scaledWidth) / 2;
            int posY = (screenHeight - scaledHeight) / 2;

            // 绘制背景图片
            spriteBatch.Draw(_currentBackground,
                             new Rectangle(posX, posY, scaledWidth, scaledHeight),
                             Color.White);
        }
    }
}

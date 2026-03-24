using BladeStory.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace BladeStory
{
    public class MainGame : Game
    {
        private SpriteBatch _spriteBatch;
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;

        private readonly IServiceCollection _services;
        private readonly GraphicsDeviceManager _graphics;
        private BoxingViewportAdapter _viewportAdapter;

        public MainGame(IServiceCollection services)
        {
            _services = services;
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // 注册 ViewportAdapter
            _viewportAdapter = new BoxingViewportAdapter(
                Window,
                _graphics.GraphicsDevice,
                640,
                360
            );
            _services.AddSingleton(_viewportAdapter);

            // 设置窗口大小
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _map = Content.Load<TiledMap>("Maps/Town/OriginalTown");
            _mapRenderer = new(GraphicsDevice, _map);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _mapRenderer.Draw();

            base.Draw(gameTime);
        }
    }
}

using BladeStory.Infrastructure.DI;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BladeStory
{
    public class MainGame : Game
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private SpriteBatch _spriteBatch;
        private TiledMap _map;

        // 基础服务
        private readonly IServiceCollection _services;
        private readonly GraphicsDeviceManager _graphics;
        private IServiceProvider _serviceProvider;
        private BoxingViewportAdapter _viewportAdapter;

        // 系统服务
        private ISceneManager _sceneManager;
        private IEnumerable<IStartable> _startables;
        private IEnumerable<IUpdatable> _updatables;

        public MainGame(IServiceCollection services)
        {
#if DEBUG
            AllocConsole();
            Console.WriteLine("[MainGame]: MonoGame 控制台已启动 - 调试模式");
#endif
            _services = services;
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /*
         *  由于ViewportAdapter依赖Game
         *  而InputManager又依赖ViewportAdapter
         *  所以它必须等游戏启动后再注册
         */
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // 1.初始化窗口
            InitializeGameWindow();

            // 2.注册系统级服务
            RegisterServices();

            // 3.获取必要服务
            GetRequiredServices();

            foreach (var startable in _startables)
            {
                startable.Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _sceneManager.LoadScene("Scenes/Town/Original");

            // _mapRenderer = new(GraphicsDevice, _map);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            foreach (var updatable in _updatables)
            {
                updatable.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _sceneManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitializeGameWindow()
        {
            // 设置窗口大小
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        private void RegisterServices()
        {
            // 注册 ViewportAdapter
            _viewportAdapter = new BoxingViewportAdapter(
                Window,
                _graphics.GraphicsDevice,
                640,
                360
            );
            _services.AddSingleton<ViewportAdapter>(_viewportAdapter);

            // 注册 GraphicsDevice
            _services.AddSingleton(GraphicsDevice);

            // 注册 ContentManager
            _services.AddSingleton(Content);

            // 注册游戏核心服务
            _services.AddCoreServices();
            _services.AddFactories();
            _services.AddGameManagers();
            _services.AddMiddlewares();
            _services.AddStartable();
            _services.AddUpdatable();

            // 构建服务
            _serviceProvider = _services.BuildServiceProvider();
        }

        // 获取必要服务
        private void GetRequiredServices()
        {
            _sceneManager = _serviceProvider.GetService<ISceneManager>();
            _startables = _serviceProvider.GetServices<IStartable>();
            _updatables = _serviceProvider.GetServices<IUpdatable>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
#if DEBUG
                var handle = GetConsoleWindow();
                if (handle != IntPtr.Zero)
                    ShowWindow(handle, SW_HIDE);
#endif
                _spriteBatch?.Dispose();
                // 如果 _serviceProvider 实现了 IDisposable，也应该释放
                (_serviceProvider as IDisposable)?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

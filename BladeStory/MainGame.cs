using BladeStory.Infrastructure.DI;
using BladeStory.Service.Interfaces;
using BladeStory.Service.Interfaces.Managers;
using FontStashSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.IO;
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


        // 基础服务
        private readonly IServiceCollection _services;
        private readonly GraphicsDeviceManager _graphics;
        private IServiceProvider _serviceProvider;
        private BoxingViewportAdapter _viewportAdapter;
        private OrthographicCamera _camera;
        private BitmapFont _bitmapFont;
        private Desktop _desktop;
        private FontSystem _fontSystem;

        // 系统服务
        private ISceneManager _sceneManager;
        private ITileMapManager _tileMapManager;
        private IBackgroundManager _backgroundManager;
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
            MyraEnvironment.Game = this;

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

            // 4.基础配置
            ConfigureBasics();

            foreach (var startable in _startables)
            {
                startable.Initialize();
            }

            _sceneManager.LoadScene("Scenes/Screen/MainMenu");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            
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
            // === 1.绘制屏幕空间背景（不受相机移动影响） ===
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                               SamplerState.LinearClamp, null, null, null, null);
            // 关键：这里不传入相机的变换矩阵（最后一个参数为 null）

            _backgroundManager.Draw(_spriteBatch);
            
            _spriteBatch.End(); // 结束屏幕空间绘制

            // 绘制相机相关
            var viewMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            _tileMapManager.Draw(_camera);
            _sceneManager.Draw(_spriteBatch);

            _spriteBatch.End();

            _desktop.Render();

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
            _camera = new OrthographicCamera(_viewportAdapter);
            _desktop = new();

            _services.AddSingleton<ViewportAdapter>(_viewportAdapter);

            // 注册 GraphicsDevice
            _services.AddSingleton(GraphicsDevice);

            // 注册 Camera
            _services.AddSingleton(_camera);

            _services.AddSingleton(_desktop);

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
            _tileMapManager = _serviceProvider.GetService<ITileMapManager>();
            _backgroundManager = _serviceProvider.GetService<IBackgroundManager>();
            _startables = _serviceProvider.GetServices<IStartable>();
            _updatables = _serviceProvider.GetServices<IUpdatable>();
        }

        private void ConfigureBasics()
        {
            _bitmapFont = Content.Load<BitmapFont>("Fonts/ChillBitmap");
            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes("Content\\Fonts\\ChillBitmap_16px.ttf"));

#if DEBUG
            Console.WriteLine(File.Exists("Content\\Fonts\\ChillBitmap_16px.ttf"));
#endif
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

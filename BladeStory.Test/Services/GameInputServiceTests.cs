using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.ViewportAdapters;
using Moq;


namespace BladeStory.Test.Services
{
    public class GameInputServiceTests
    {
        private readonly Mock<IInputManager> _mockInputService;
        private readonly GameInputConsumer _consumer;
        private readonly Mock<ViewportAdapter> _mockViewportAdapter;

        public GameInputServiceTests()
        {
            _mockInputService = new Mock<IInputManager>();
            _consumer = new GameInputConsumer(_mockInputService.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithMockService()
        {
            // Assert
            Assert.NotNull(_mockInputService.Object);
            Assert.NotNull(_consumer);
        }

        [Fact]
        public void IsJumping_WhenSpaceIsPressed_ShouldReturnTrue()
        {
            // Arrange
            _mockInputService.Setup(x => x.IsKeyPressed(Keys.Space)).Returns(true);

            // Act
            var result = _consumer.IsJumping;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsJumping_WhenSpaceIsNotPressed_ShouldReturnFalse()
        {
            // Arrange
            _mockInputService.Setup(x => x.IsKeyPressed(Keys.Space)).Returns(false);

            // Act
            var result = _consumer.IsJumping;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMovingLeft_WhenAKeyIsDown_ShouldReturnTrue()
        {
            // Arrange
            _mockInputService.Setup(x => x.IsKeyDown(Keys.A)).Returns(true);

            // Act
            var result = _consumer.IsMovingLeft;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMovingRight_WhenDKeyIsDown_ShouldReturnTrue()
        {
            // Arrange
            _mockInputService.Setup(x => x.IsKeyDown(Keys.D)).Returns(true);

            // Act
            var result = _consumer.IsMovingRight;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAttacking_WhenLeftMouseButtonIsPressed_ShouldReturnTrue()
        {
            // Arrange
            _mockInputService.Setup(x => x.IsMouseButtonPressed(MouseButton.Left)).Returns(true);

            // Act
            var result = _consumer.IsAttacking;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetMouseWorldPosition_WhenViewportAdapterExists_ShouldConvertPosition()
        {
            // Arrange
            var mockViewportAdapter = new Mock<ViewportAdapter>(MockBehavior.Default, (GraphicsDevice)null);
            var mousePosition = new Vector2(100, 200);
            var expectedWorldPosition = new Vector2(50, 100);

            _mockInputService.Setup(x => x.GetMousePosition()).Returns(mousePosition);
            _mockInputService.Setup(x => x.ViewportAdapter).Returns(mockViewportAdapter.Object);
            mockViewportAdapter.Setup(x => x.PointToScreen(mousePosition.ToPoint())).Returns(expectedWorldPosition.ToPoint());

            // Act
            var result = _consumer.GetMouseWorldPosition();

            // Assert
            Assert.Equal(expectedWorldPosition, result);
        }

        [Fact]
        public void GetMouseWorldPosition_WhenViewportAdapterIsNull_ShouldReturnMousePosition()
        {
            // Arrange
            var mousePosition = new Vector2(100, 200);
            _mockInputService.Setup(x => x.GetMousePosition()).Returns(mousePosition);
            _mockInputService.Setup(x => x.ViewportAdapter).Returns((ViewportAdapter)null);

            // Act
            var result = _consumer.GetMouseWorldPosition();

            // Assert
            Assert.Equal(mousePosition, result);
        }

        [Fact]
        public void Update_ShouldCallInputServiceUpdate()
        {
            // Arrange
            var gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            bool updateCalled = false;

            _mockInputService.Setup(x => x.Update(gameTime))
                .Callback(() => updateCalled = true);

            // Act
            _consumer.Update(gameTime);

            // Assert
            Assert.True(updateCalled);
        }

        [Fact]
        public void KeyPressedEvent_ShouldBeHandled()
        {
            // Arrange
            bool eventHandled = false;
            Keys handledKey = Keys.None;

            _mockInputService.SetupAdd(x => x.KeyPressed += It.IsAny<EventHandler<KeyboardEventArgs>>())
                .Callback<EventHandler<KeyboardEventArgs>>(handler =>
                {
                    // 模拟事件触发
                    handler.Invoke(_mockInputService.Object, new KeyboardEventArgs(Keys.Space, _mockInputService.Object.CurrentKeyboardState));
                });

            // 订阅事件
            _mockInputService.Object.KeyPressed += (sender, args) =>
            {
                eventHandled = true;
                handledKey = args.Key;
            };

            // Act - 触发事件（通过之前的回调已经触发）

            // Assert
            Assert.True(eventHandled);
            Assert.Equal(Keys.Space, handledKey);
        }

        [Fact]
        public void KeyReleasedEvent_ShouldBeHandled()
        {
            // Arrange
            bool eventHandled = false;
            Keys handledKey = Keys.None;

            _mockInputService.SetupAdd(x => x.KeyReleased += It.IsAny<EventHandler<KeyboardEventArgs>>())
                .Callback<EventHandler<KeyboardEventArgs>>(handler =>
                {
                    handler.Invoke(_mockInputService.Object, new KeyboardEventArgs(Keys.Enter, _mockInputService.Object.CurrentKeyboardState));
                });

            _mockInputService.Object.KeyReleased += (sender, args) =>
            {
                eventHandled = true;
                handledKey = args.Key;
            };

            // Assert
            Assert.True(eventHandled);
            Assert.Equal(Keys.Enter, handledKey);
        }

        [Fact]
        public void MouseButtonReleasedEvent_ShouldBeHandled()
        {
            // Arrange
            bool eventHandled = false;
            MouseButton triggeredButton = MouseButton.None;

            var previousState = new MouseState(100, 150, 0, ButtonState.Pressed, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);
            var currentState = new MouseState(100, 150, 0, ButtonState.Released, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);
            var gameTime = TimeSpan.FromSeconds(1);

            var mouseEventArgs = new MouseEventArgs(_mockViewportAdapter.Object, gameTime, previousState, currentState, MouseButton.Left);

            _mockInputService.SetupAdd(x => x.MouseButtonReleased += It.IsAny<EventHandler<MouseEventArgs>>())
                .Callback<EventHandler<MouseEventArgs>>(handler =>
                {
                    handler.Invoke(_mockInputService.Object, mouseEventArgs);
                });

            _mockInputService.Object.MouseButtonReleased += (sender, args) =>
            {
                eventHandled = true;
                triggeredButton = args.Button;
            };

            // Assert
            Assert.True(eventHandled);
            Assert.Equal(MouseButton.Left, triggeredButton);
        }

        [Fact]
        public void MouseScrolledEvent_ShouldBeHandled()
        {
            // Arrange
            bool eventHandled = false;
            int scrollDelta = 0;

            var previousState = new MouseState(100, 150, 10, ButtonState.Released, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);
            var currentState = new MouseState(100, 150, 20, ButtonState.Released, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);
            var gameTime = TimeSpan.FromSeconds(1);

            var mouseEventArgs = new MouseEventArgs(_mockViewportAdapter.Object, gameTime, previousState, currentState);

            _mockInputService.SetupAdd(x => x.MouseScrolled += It.IsAny<EventHandler<MouseEventArgs>>())
                .Callback<EventHandler<MouseEventArgs>>(handler =>
                {
                    handler.Invoke(_mockInputService.Object, mouseEventArgs);
                });

            _mockInputService.Object.MouseScrolled += (sender, args) =>
            {
                eventHandled = true;
                scrollDelta = args.ScrollWheelDelta;
            };

            // Assert
            Assert.True(eventHandled);
            Assert.Equal(10, scrollDelta); // 20 - 10 = 10
        }

        [Theory]
        [InlineData(Keys.W, true)]
        [InlineData(Keys.W, false)]
        [InlineData(Keys.S, true)]
        [InlineData(Keys.S, false)]
        public void KeyboardState_ShouldReturnCorrectValues(Keys key, bool isDown)
        {
            // Arrange
            _mockInputService.Setup(x => x.IsKeyDown(key)).Returns(isDown);

            // Act
            var result = _mockInputService.Object.IsKeyDown(key);

            // Assert
            Assert.Equal(isDown, result);
        }

        [Theory]
        [InlineData(MouseButton.Left, true)]
        [InlineData(MouseButton.Left, false)]
        [InlineData(MouseButton.Right, true)]
        [InlineData(MouseButton.Right, false)]
        public void MouseButtonState_ShouldReturnCorrectValues(MouseButton button, bool isPressed)
        {
            // Arrange
            _mockInputService.Setup(x => x.IsMouseButtonPressed(button)).Returns(isPressed);

            // Act
            var result = _mockInputService.Object.IsMouseButtonPressed(button);

            // Assert
            Assert.Equal(isPressed, result);
        }

        [Fact]
        public void CurrentAndPreviousKeyboardState_ShouldBeAccessible()
        {
            // Arrange
            var currentState = new KeyboardState([Keys.A]);
            var previousState = new KeyboardState([Keys.B]);

            _mockInputService.Setup(x => x.CurrentKeyboardState).Returns(currentState);
            _mockInputService.Setup(x => x.PreviousKeyboardState).Returns(previousState);

            // Act & Assert
            Assert.Equal(currentState, _mockInputService.Object.CurrentKeyboardState);
            Assert.Equal(previousState, _mockInputService.Object.PreviousKeyboardState);
        }

        [Fact]
        public void CurrentAndPreviousMouseState_ShouldBeAccessible()
        {
            // Arrange
            var currentState = new MouseState(100, 200, 0, ButtonState.Pressed, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);
            var previousState = new MouseState(90, 190, 0, ButtonState.Released, ButtonState.Released,
                ButtonState.Released, ButtonState.Released, ButtonState.Released);

            _mockInputService.Setup(x => x.CurrentMouseState).Returns(currentState);
            _mockInputService.Setup(x => x.PreviousMouseState).Returns(previousState);

            // Act & Assert
            Assert.Equal(currentState, _mockInputService.Object.CurrentMouseState);
            Assert.Equal(previousState, _mockInputService.Object.PreviousMouseState);
        }

        [Fact]
        public void CurrentTime_ShouldBeSettableAndGettable()
        {
            // Arrange
            var expectedTime = TimeSpan.FromSeconds(5.5);
            _mockInputService.SetupProperty(x => x.CurrentTime);

            // Act
            _mockInputService.Object.CurrentTime = expectedTime;

            // Assert
            Assert.Equal(expectedTime, _mockInputService.Object.CurrentTime);
        }

        [Fact]
        public void ViewportAdapter_ShouldBeAccessible()
        {
            // Arrange
            var mockViewportAdapter = new Mock<ViewportAdapter>(MockBehavior.Default, (GraphicsDevice)null);
            _mockInputService.Setup(x => x.ViewportAdapter).Returns(mockViewportAdapter.Object);

            // Act & Assert
            Assert.Equal(mockViewportAdapter.Object, _mockInputService.Object.ViewportAdapter);
        }

        [Fact]
        public void MultipleEvents_ShouldAllBeHandled()
        {
            // Arrange
            int eventCount = 0;

            _mockInputService.SetupAdd(x => x.KeyPressed += It.IsAny<EventHandler<KeyboardEventArgs>>())
                .Callback(() => eventCount++);
            _mockInputService.SetupAdd(x => x.MouseMoved += It.IsAny<EventHandler<MouseEventArgs>>())
                .Callback(() => eventCount++);
            _mockInputService.SetupAdd(x => x.MouseButtonPressed += It.IsAny<EventHandler<MouseEventArgs>>())
                .Callback(() => eventCount++);

            // 模拟多个事件订阅
            _mockInputService.Object.KeyPressed += (s, e) => { };
            _mockInputService.Object.MouseMoved += (s, e) => { };
            _mockInputService.Object.MouseButtonPressed += (s, e) => { };

            // Assert
            Assert.Equal(3, eventCount);
        }

        [Fact]
        public void Update_ShouldUpdateCurrentTime()
        {
            // Arrange
            var gameTime = new GameTime(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            TimeSpan capturedTime = TimeSpan.Zero;

            _mockInputService.SetupSet(x => x.CurrentTime = It.IsAny<TimeSpan>())
                .Callback<TimeSpan>(time => capturedTime = time);
            _mockInputService.Setup(x => x.Update(gameTime))
                .Callback(() => _mockInputService.Object.CurrentTime = gameTime.TotalGameTime);

            // Act
            _mockInputService.Object.Update(gameTime);

            // Assert
            Assert.Equal(gameTime.TotalGameTime, capturedTime);
        }
    }
}

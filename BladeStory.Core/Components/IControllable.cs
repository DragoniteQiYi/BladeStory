namespace BladeStory.Core.Components
{
    public interface IControllable
    {
        bool InputEnabled { get; }

        /// <summary>
        /// 将来自InputManager的输入转换为命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ReceiveCommand(ICommand command);

        /// <summary>
        /// 启用或禁用输入
        /// </summary>
        /// <param name="state"></param>
        void EnableInput(bool state);
    }
}

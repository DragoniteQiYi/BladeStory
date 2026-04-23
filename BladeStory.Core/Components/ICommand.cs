using Microsoft.Xna.Framework;

namespace BladeStory.Core.Components
{
    public interface ICommand
    {
        /// <summary>
        /// 输入是否合法
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// 输入时间戳
        /// </summary>
        GameTime GameTime { get; }
    }
}

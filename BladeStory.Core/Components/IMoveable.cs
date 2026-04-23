using Microsoft.Xna.Framework;

namespace BladeStory.Core.Components
{
    public interface IMoveable
    {
        /// <summary>
        /// 移动单位速度
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// 是否允许移动
        /// </summary>
        bool CanMove { get; }

        /// <summary>
        /// 移动速度向量
        /// </summary>
        Vector2 Velocity { get; }

        /// <summary>
        /// 向指定方向移动
        /// </summary>
        /// <param name="direction"></param>
        void Move(Vector2 direction);

        /// <summary>
        /// 向指定坐标移动
        /// </summary>
        /// <param name="targetPosition"></param>
        void MoveTo(Vector2 targetPosition);
    }
}

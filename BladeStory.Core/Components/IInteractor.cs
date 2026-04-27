namespace BladeStory.Core.Components
{
    public interface IInteractor
    {
        /// <summary>
        /// 实体交互动作
        /// </summary>
        /// <param name="target"></param>
        void InteractWith(IInteractable target);
    }
}

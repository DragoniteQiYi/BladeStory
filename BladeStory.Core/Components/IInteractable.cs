namespace BladeStory.Core.Components
{
    public interface IInteractable
    {
        /// <summary>
        /// 实体交互逻辑
        /// </summary>
        /// <param name="interactor"></param>
        void OnInteract(IInteractor interactor);
    }
}

namespace BladeStory.Core.Components
{
    public interface IEntity
    {
        /// <summary>
        /// 实体唯一标识符
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 实体标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 实体是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 实体是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}

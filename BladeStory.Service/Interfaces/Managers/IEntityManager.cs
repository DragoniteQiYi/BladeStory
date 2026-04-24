using BladeStory.Core.Components;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Managers
{
    /*
     *  不要让场景管理器生成实体
     *  职责分离很重要.......
     *  实体拿不到的DI服务从这里传
     */
    public interface IEntityManager
    {
        IEntity Spawn(string id, Vector2 position);

        void Destroy(Guid id);
    }
}

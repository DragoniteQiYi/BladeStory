using BladeStory.Core.Tilemap;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace BladeStory.Service.Interfaces.Managers
{
    public interface ITileMapManager
    {
        // 地图基本信息
        string? MapName { get; }
        int Width { get; }
        int Height { get; }
        int TileWidth { get; }
        int TileHeight { get; }
        int WidthInPixels { get; }
        int HeightInPixels { get; }
        Vector2 MapSize { get; }

        // 加载与卸载
        void LoadMap(string? mapPath);
        void UnloadMap();
        bool IsLoaded { get; }

        // 对象查询
        IReadOnlyList<MapObject> AllObjects { get; }
        IReadOnlyList<MapObject> GetObjectsByLayer(string layerName);
        IReadOnlyList<MapObject> GetObjectsByType(string type);
        IReadOnlyList<MapObject> GetObjectsByName(string name);
        IReadOnlyList<MapObject> GetObjectsByProperty(string propertyKey, string propertyValue);
        MapObject GetFirstObject(string layerName, string objectName);

        // 层信息
        IReadOnlyList<MapLayerInfo> LayerInfos { get; }
        string[] GetLayerNames();
        string[] GetObjectLayerNames();

        // 渲染
        void Update(GameTime gameTime);
        void Draw(OrthographicCamera camera);

        // 坐标转换
        Vector2 TileToWorldPosition(int x, int y);
        Point WorldToTilePosition(Vector2 worldPosition);
    }
}

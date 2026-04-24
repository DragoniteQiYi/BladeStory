using BladeStory.Constant;
using BladeStory.Core.Tilemap;
using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace BladeStory.Service.Managers
{
    public class TileMapManager : ITileMapManager
    {
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;

        private List<MapObject> _allObjects = [];
        private List<MapLayerInfo> _layerInfos = [];
        private Dictionary<string, List<MapObject>> _objectsByLayer = [];

        public string? MapName { get; private set; }
        public int Width => _tiledMap?.Width ?? 0;
        public int Height => _tiledMap?.Height ?? 0;
        public int TileWidth => _tiledMap?.TileWidth ?? 0;
        public int TileHeight => _tiledMap?.TileHeight ?? 0;
        public int WidthInPixels => _tiledMap?.WidthInPixels ?? 0;
        public int HeightInPixels => _tiledMap?.HeightInPixels ?? 0;
        public Vector2 MapSize => new Vector2(WidthInPixels, HeightInPixels);
        public bool IsLoaded => _tiledMap != null;

        public IReadOnlyList<MapObject> AllObjects => _allObjects.AsReadOnly();
        public IReadOnlyList<MapLayerInfo> LayerInfos => _layerInfos.AsReadOnly();

        public TileMapManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
        }

        public TiledMap LoadMap(string? mapPath)
        {
            // 卸载旧地图
            UnloadMap();

            // 加载Tiled地图
            _tiledMap = _contentManager.Load<TiledMap>(mapPath);
            _tiledMapRenderer = new TiledMapRenderer(_graphicsDevice, _tiledMap);
            MapName = mapPath;

            // 解析所有对象
            ParseAllObjects();

            // 收集层信息
            CollectLayerInfo();

            Console.WriteLine($"地图加载完成: {mapPath}");
            Console.WriteLine($"尺寸: {Width}x{Height} 瓦片");
            Console.WriteLine($"对象总数: {_allObjects.Count}");
            Console.WriteLine($"对象层数: {GetObjectLayerNames().Length}");

            return _tiledMap;
        }

        public void UnloadMap()
        {
            if (_tiledMapRenderer != null)
            {
                // TiledMapRenderer在5.5.1中不需要特殊卸载
                _tiledMapRenderer = null;
            }

            _tiledMap = null;
            _allObjects.Clear();
            _objectsByLayer.Clear();
            _layerInfos.Clear();
            MapName = null;
        }

        /// <summary>
        /// 遍历所有对象层，解析所有对象
        /// </summary>
        private void ParseAllObjects()
        {
            _allObjects.Clear();
            _objectsByLayer.Clear();

            if (_tiledMap == null) return;

            // 遍历所有对象层
            foreach (TiledMapObjectLayer objectLayer in _tiledMap.ObjectLayers)
            {
                var objectsInLayer = new List<MapObject>();

                Console.WriteLine($"解析对象层: {objectLayer.Name}, 对象数量: {objectLayer.Objects.Length}");

                // 遍历层中的每个对象
                foreach (TiledMapObject obj in objectLayer.Objects)
                {
                    // 跳过不可见对象
                    if (!obj.IsVisible) continue;

                    // 转换为通用对象数据
                    var mapObj = ConvertToMapObject(obj, objectLayer.Name);

                    if (mapObj != null)
                    {
                        _allObjects.Add(mapObj);
                        objectsInLayer.Add(mapObj);

                        Console.WriteLine($"  [{mapObj.Kind}] {mapObj.Name} - 位置:({mapObj.Position.X:F0},{mapObj.Position.Y:F0}) 类型:{mapObj.Type}");
                    }
                }

                _objectsByLayer[objectLayer.Name] = objectsInLayer;
            }
        }

        /// <summary>
        /// 将TiledMapObject转换为通用的MapObjectData
        /// </summary>
        private MapObject ConvertToMapObject(TiledMapObject obj, string layerName)
        {
            var data = new MapObject
            {
                Name = obj.Name ?? "",
                Type = obj.Type ?? "",
                Position = obj.Position,
                Properties = new Dictionary<string, string>()
            };

            // 复制属性
            foreach (var prop in obj.Properties)
            {
                data.Properties[prop.Key] = prop.Value;
            }

            // 根据对象类型设置Kind和特有数据
            switch (obj)
            {
                case TiledMapRectangleObject rect:
                    data.Kind = MapObjectKind.Rectangle;
                    data.Size = rect.Size;
                    break;

                case TiledMapEllipseObject ellipse:
                    data.Kind = MapObjectKind.Ellipse;
                    data.Size = ellipse.Radius * 2; // 用直径作为Size
                    break;

                //case TiledMapPointObject point:
                //    data.Kind = MapObjectKind.Point;
                //    data.Size = Vector2.Zero;
                //    break;

                case TiledMapPolygonObject polygon:
                    data.Kind = MapObjectKind.Polygon;
                    data.Points = new Vector2[polygon.Points.Length];
                    Array.Copy(polygon.Points, data.Points, polygon.Points.Length);

                    // 计算边界大小
                    if (polygon.Points.Length > 0)
                    {
                        var bounds = CalculateBounds(polygon.Points);
                        data.Size = new Vector2(bounds.Width, bounds.Height);
                    }
                    break;

                case TiledMapPolylineObject polyline:
                    data.Kind = MapObjectKind.Polyline;
                    data.Points = new Vector2[polyline.Points.Length];
                    Array.Copy(polyline.Points, data.Points, polyline.Points.Length);

                    if (polyline.Points.Length > 0)
                    {
                        var bounds = CalculateBounds(polyline.Points);
                        data.Size = new Vector2(bounds.Width, bounds.Height);
                    }
                    break;

                case TiledMapTileObject tileObj:
                    data.Kind = MapObjectKind.Tile;
                    data.Size = new Vector2(_tiledMap.TileWidth, _tiledMap.TileHeight);
                    data.GlobalTileId = tileObj.Identifier;
                    break;

                default:
                    data.Kind = MapObjectKind.Point;
                    data.Size = Vector2.Zero;
                    break;
            }

            return data;
        }

        /// <summary>
        /// 计算点集的边界框
        /// </summary>
        private RectangleF CalculateBounds(Vector2[] points)
        {
            if (points.Length == 0) return RectangleF.Empty;

            float minX = points[0].X;
            float minY = points[0].Y;
            float maxX = points[0].X;
            float maxY = points[0].Y;

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].X < minX) minX = points[i].X;
                if (points[i].Y < minY) minY = points[i].Y;
                if (points[i].X > maxX) maxX = points[i].X;
                if (points[i].Y > maxY) maxY = points[i].Y;
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// 收集所有层的信息
        /// </summary>
        private void CollectLayerInfo()
        {
            _layerInfos.Clear();

            if (_tiledMap == null) return;

            foreach (TiledMapLayer layer in _tiledMap.Layers)
            {
                string layerType = "unknown";

                if (layer is TiledMapTileLayer) layerType = "tile";
                else if (layer is TiledMapObjectLayer) layerType = "object";
                else if (layer is TiledMapImageLayer) layerType = "image";

                _layerInfos.Add(new MapLayerInfo
                {
                    Name = layer.Name,
                    Type = layerType,
                    IsVisible = layer.IsVisible,
                    Opacity = layer.Opacity
                });
            }
        }

        // ========== 对象查询方法 ==========

        public IReadOnlyList<MapObject> GetObjectsByLayer(string layerName)
        {
            return _objectsByLayer.TryGetValue(layerName, out var objects)
                ? objects.AsReadOnly()
                : new List<MapObject>().AsReadOnly();
        }

        public IReadOnlyList<MapObject> GetObjectsByType(string type)
        {
            return _allObjects.FindAll(obj => obj.Type == type).AsReadOnly();
        }

        public IReadOnlyList<MapObject> GetObjectsByName(string name)
        {
            return _allObjects.FindAll(obj => obj.Name == name).AsReadOnly();
        }

        public IReadOnlyList<MapObject> GetObjectsByProperty(string propertyKey, string propertyValue)
        {
            return _allObjects.FindAll(obj =>
                obj.Properties.TryGetValue(propertyKey, out var value) && value == propertyValue
            ).AsReadOnly();
        }

        public MapObject GetFirstObject(string layerName, string objectName)
        {
            if (_objectsByLayer.TryGetValue(layerName, out var objects))
            {
                return objects.Find(obj => obj.Name == objectName);
            }
            return null;
        }

        // ========== 层查询方法 ==========

        public string[] GetLayerNames()
        {
            if (_tiledMap == null) return Array.Empty<string>();

            var names = new string[_tiledMap.Layers.Count];
            for (int i = 0; i < _tiledMap.Layers.Count; i++)
            {
                names[i] = _tiledMap.Layers[i].Name;
            }
            return names;
        }

        public string[] GetObjectLayerNames()
        {
            if (_tiledMap == null) return Array.Empty<string>();

            var names = new List<string>();
            foreach (TiledMapObjectLayer layer in _tiledMap.ObjectLayers)
            {
                names.Add(layer.Name);
            }
            return names.ToArray();
        }

        // ========== 渲染方法 ==========

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer?.Update(gameTime);
        }

        public void Draw(OrthographicCamera camera)
        {
            if (!IsLoaded || camera == null) return;

            // 在绘制前设置点采样，避免纹理过滤导致的边缘混合
            var previousBlendState = _graphicsDevice.BlendState;
            var previousSamplerState = _graphicsDevice.SamplerStates[0];

            _graphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            if (camera is OrthographicCamera orthoCamera)
            {
                // 取整处理
                var originalPos = orthoCamera.Position;
                orthoCamera.Position = new Vector2(
                    (float)Math.Round(originalPos.X),
                    (float)Math.Round(originalPos.Y)
                );

                _tiledMapRenderer?.Draw(orthoCamera.GetViewMatrix());
                orthoCamera.Position = originalPos;
            }
            else
            {
                _tiledMapRenderer?.Draw(camera.GetViewMatrix());
            }

            // 恢复状态
            _graphicsDevice.BlendState = previousBlendState;
            _graphicsDevice.SamplerStates[0] = previousSamplerState;
        }

        // ========== 坐标转换 ==========

        public Vector2 TileToWorldPosition(int x, int y)
        {
            return new Vector2(x * TileWidth, y * TileHeight);
        }

        public Point WorldToTilePosition(Vector2 worldPosition)
        {
            return new Point(
                (int)(worldPosition.X / TileWidth),
                (int)(worldPosition.Y / TileHeight)
            );
        }
    }
}

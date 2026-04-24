using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BladeStory.Service.Managers
{
    public class AssetManager : IAssetManager
    {
        private readonly ContentManager _contentManager;

        private readonly Dictionary<string, Texture2D> _textures = [];
        private readonly Dictionary<string, SoundEffect> _soundEffects = [];
        private readonly Dictionary<string, Song> _songs = [];

        public AssetManager(ContentManager contentManager) 
        {
            _contentManager = contentManager;

            Console.WriteLine($"[AssetManager]: 资源管理模块初始化成功");
        }

        public Song GetSong(string id)
        {
            if (_songs.TryGetValue(id, out var song))
                return song;

            Console.WriteLine($"ERROR-[AssetManager]: 找不到Id为: {id} 的音乐");
            return null;
        }

        public SoundEffect GetSoundEffect(string id)
        {
            if (_soundEffects.TryGetValue(id, out var soundEffect))
                return soundEffect;

            Console.WriteLine($"ERROR-[AssetManager]: 找不到Id为: {id} 的音效");
            return null;
        }

        public Texture2D GetTexture(string id)
        {
            if (_textures.TryGetValue(id, out var texture)) 
                return texture;

            Console.WriteLine($"ERROR-[AssetManager]: 找不到Id为: {id} 的贴图");
            return null;
        }

        public void LoadMusic(string path)
        {
            if (_songs.ContainsKey(path))
            {
                return;
            }

            _songs[path] = _contentManager.Load<Song>(path);
        }

        public void LoadSoundEffect(string path)
        {
            if (_soundEffects.ContainsKey(path))
            {
                return;
            }

            _soundEffects[path] = _contentManager.Load<SoundEffect>(path);
        }

        public void LoadTexture(string path)
        {
            if (_textures.ContainsKey(path))
            {
                return;
            }

            _textures[path] = _contentManager.Load<Texture2D>(path);
        }
    }
}

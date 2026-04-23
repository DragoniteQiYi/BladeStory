using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BladeStory.Service.Interfaces.Services
{
    public interface IAssetManager
    {
        void LoadTexture(string path);

        void LoadSoundEffect(string path);

        void LoadMusic(string path);

        Texture2D GetTexture(string id);

        SoundEffect GetSoundEffect(string id);

        Song GetSong(string id);
    }
}

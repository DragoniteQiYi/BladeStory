using BladeStory.Service.Interfaces.Managers;
using Microsoft.Xna.Framework.Media;

namespace BladeStory.Service.Managers
{
    public class AudioManager : IAudioManager
    {
        private readonly IAssetManager _assetManager;

        public bool IsMusicPlaying { get; private set; }
        public string? PlayingMusicId { get; private set; }

        private Song? _currentPlayingMusic;

        public AudioManager(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public void PlayMusic(string musicId)
        {
            _currentPlayingMusic = _assetManager.GetSong(musicId);
            if (_currentPlayingMusic == null)
            {
                _assetManager.LoadMusic(musicId);
                _currentPlayingMusic = _assetManager.GetSong(musicId);
            }

            // 需要配置
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_currentPlayingMusic);
            IsMusicPlaying = true;
            PlayingMusicId = musicId;
        }

        public void PlaySoundEffect(string soundEffectId)
        {

        }

        public void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
            _currentPlayingMusic = null;
            IsMusicPlaying = false;
            PlayingMusicId = string.Empty;
        }
    }
}

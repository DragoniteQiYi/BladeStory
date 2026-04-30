namespace BladeStory.Service.Interfaces.Managers
{
    public interface IAudioManager
    {
        bool IsMusicPlaying { get; }

        string? PlayingMusicId { get; }

        void PlayMusic(string musicId);

        void PlaySoundEffect(string soundEffectId);

        void PauseMusic();

        void ResumeMusic();

        void StopMusic();
    }
}

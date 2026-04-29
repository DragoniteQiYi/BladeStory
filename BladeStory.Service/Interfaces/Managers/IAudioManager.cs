namespace BladeStory.Service.Interfaces.Managers
{
    public interface IAudioManager
    {
        void PlayMusic(string musicId);

        void PlaySoundEffect(string soundEffectId);

        void PauseMusic();

        void ResumeMusic();

        void StopMusic();
    }
}

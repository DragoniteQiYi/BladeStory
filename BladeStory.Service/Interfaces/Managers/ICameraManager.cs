using Microsoft.Xna.Framework;

namespace BladeStory.Service.Interfaces.Managers
{
    public interface ICameraManager
    {
        Vector2 Position { get; set; }

        float Zoom { get; set; }

        void Update();

        void Move(Vector2 direction);

        void ZoomIn(float factor);

        void ZoomOut(float factor);

        Matrix GetViewMatrix();

        void Shake(float intensity);
    }
}

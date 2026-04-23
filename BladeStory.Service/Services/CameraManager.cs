using BladeStory.Service.Interfaces.Services;
using Microsoft.Xna.Framework;

namespace BladeStory.Service.Services
{
    public class CameraManager : ICameraManager
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }

        public Matrix GetViewMatrix()
        {
            throw new NotImplementedException();
        }

        public void Move(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public void Shake(float intensity)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void ZoomIn(float factor)
        {
            throw new NotImplementedException();
        }

        public void ZoomOut(float factor)
        {
            throw new NotImplementedException();
        }
    }
}

using BladeStory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeStory.Service.Interfaces
{
    public interface ISceneService
    {
        public Scene CurrentScene { get; set; }

        public void LoadScene(Scene scene);
    }
}

using Sim.Rendering;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Renderables
{
    public class TextRenderable : IRenderable
    {
        public IList<IRenderable> RenderableChildren => null;
        public string Text { get; set; }
        public Vec3d Position { get; set; }
        public World.World World { get; set; }

        public TextRenderable(string text, Vec3d position, World.World world)
        {
            Text = text;
            Position = position;
            World = world;
        }
    }
}

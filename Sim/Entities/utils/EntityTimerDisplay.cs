using Sim.Logic;
using Sim.Renderables;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Entities.utils
{
    public class EntityTimerDisplay : BaseEntity
    {
        private Timer timer;
        private TextRenderable textRenderable = new TextRenderable("", Vec3d.Zero, null);

        public EntityTimerDisplay(Vec3d position, Timer timer)
        {
            Position = position;
            this.timer = timer;
            RenderableChildren.Add(textRenderable);
        }

        public override void Tick()
        {
            textRenderable.World = World;
            textRenderable.Position = Position;
            textRenderable.Text = timer.ActualTps.ToString();
        }
    }
}

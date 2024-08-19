using Sim.Entities;
using Sim.Entities.utils;
using Sim.Logic;
using Sim.Rendering;
using Sim.Structs;
using Sim.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Objects;
using Sim.World.Populator;

namespace Sim
{
    public class Game
    {
        public bool IsRunning { get; set; }
        IRenderer renderer = new TtyRenderer(Console.WindowWidth, Console.WindowHeight);
        IList<World.World> worldRegistry = new List<World.World>();
        Timer timer = new Timer(60);

        public void Run()
        {
            IsRunning = true;

            var mainWorld = new World.World(Console.WindowWidth, Console.WindowHeight);
            mainWorld.SpawnEntity(new EntityTimerDisplay(Vec3d.Zero, timer));

            new FillWorldPopulator(mainWorld).Populate();

            mainWorld.SpawnEntity(new EntityMiner(new Vec3d(1, 1, 0)));

            timer.FastForward = 1;

            worldRegistry.Add(mainWorld);

            while(IsRunning)
            {
                var tickCount = timer.ShouldTick();
                for(int i = 0; i < tickCount; i++)
                {
                    foreach(var world in worldRegistry)
                    {
                        world.Tick();
                    }

                    timer.Tick();
                }

                renderer.Render(mainWorld.RenderableChildren);

                var sleepTime = timer.GetNextTickInMs() * 0.75;
                if(sleepTime > 0)
                {
                    System.Threading.Thread.Sleep((int)(sleepTime));
                }
            }
        }
    }
}

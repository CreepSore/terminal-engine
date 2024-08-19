using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Logic
{
    public class Timer : ITickable
    {
        private int actualTps;
        private int currentTicks;
        private long lastTpsUpdate = 0;

        private int tps = 0;
        private double tickEveryMs = 0;
        private long lastTick = 0;
        private float speed = 1.0f;

        public int ActualTps => actualTps;

        public float Speed
        {
            get => speed;
            set {
                speed = value;
                updateTickEveryMs();
            }
        }

        public int Tps
        {
            get => tps;
            set
            {
                tps = value;
                updateTickEveryMs();
            }
        }

        public int FastForward { get; set; } = 0;

        public Timer(int tps)
        {
            Tps = tps;
        }

        public int ShouldTick()
        {
            if(FastForward > 0)
            {
                return FastForward;
            }

            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (lastTick == 0)
            {
                lastTick = currentTime;
            }

            return (int)(Math.Ceiling((currentTime - lastTick) / tickEveryMs));
        }

        public void Tick()
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (currentTime - lastTpsUpdate > 1000)
            {
                actualTps = currentTicks;
                currentTicks = 0;
                lastTpsUpdate = currentTime;
            }

            currentTicks += 1;
            lastTick = currentTime;
        }

        public int GetNextTickInMs()
        {
            if(FastForward > 0)
            {
                return 0;
            }

            return (int)Math.Floor((DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastTick - tickEveryMs) * -1.0d);
        }

        private void updateTickEveryMs()
        {
            tickEveryMs = 1000 / (tps * speed);
        }
    }
}

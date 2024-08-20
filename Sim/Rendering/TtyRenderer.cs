using Sim.Entities;
using Sim.Renderables;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sim.Objects;

namespace Sim.Rendering
{
    public class TtyRenderer : IRenderer
    {
        private bool isRendering = false;
        private int width;
        private int height;
        private char[,] oldBuffer;

        public int Width => width;
        public int Height => height;

        public TtyRenderer(int width, int height)
        {
            Console.Clear();
            this.width = width;
            this.height = height;
        }

        public void Render(IList<IRenderable> renderables)
        {
            if (isRendering)
            {
                return;
            }

            isRendering = true;

            Task.Run(() =>
            {
                char[,] buffer = new char[width, height];

                IList<IRenderable> toRender;

                lock(renderables)
                {
                    toRender = new List<IRenderable>(renderables);
                }

                foreach (var renderable in toRender)
                {
                    renderRenderable(renderable, buffer);
                }

                printBuffer(buffer);
                Console.SetCursorPosition(width - 1, height - 1);
                oldBuffer = buffer;
                isRendering = false;
            });
        }

        private void renderRenderable(IRenderable renderable, char[,] buffer)
        {
            if (renderable == null)
            {
                return;
            }

            if(renderable is TextRenderable textRenderable)
            {
                // ! We have to do this because if the text gets changed in the tick loop
                // ! there's a chance that the length changes too.
                // ! If this happens during rendering, we will crash.
                var text = textRenderable.Text;
                for(int i = 0; i < text.Length; i++)
                {
                    renderChar(text[i], textRenderable.Position + new Vec3d(i, 0, 0), buffer);
                }
            }

            if (renderable is ObjectTree)
            {
                renderChar('T', renderable.Position, buffer);
            }

            if(renderable is EntityMiner)
            {
                renderChar('M', renderable.Position, buffer);
            }

            if (renderable is ObjectItemBag)
            {
                renderChar('I', renderable.Position, buffer);
            }

            if (renderable is ObjectChest)
            {
                renderChar('C', renderable.Position, buffer);
            }

            if(renderable.RenderableChildren != null)
            {
                foreach(var renderableChild in renderable.RenderableChildren)
                {
                    renderRenderable(renderableChild, buffer);
                }
            }
        }

        private void renderChar(char c, Vec3d position, char[,] buffer)
        {
            if(position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
            {
                return;
            }

            buffer[(int)position.X, (int)position.Y] = c;
        }

        private void printBuffer(char[,] buffer)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(oldBuffer == null || oldBuffer[x, y] != buffer[x, y])
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(buffer[x, y]);
                    }
                }
            }
        }
    }
}

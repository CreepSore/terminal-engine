using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Const;

namespace Sim.Pathfinding
{
    public class AStarPathFinder : IPathFinder
    {
        private readonly CostMatrix costMatrix;

        public AStarPathFinder(CostMatrix costMatrix)
        {
            this.costMatrix = costMatrix;
        }

        public Path FindPath(Vec3d from, Vec3d to, Func<Vec3d, Vec3d, bool> matcher = null)
        {
            Path path = new Path();
            var visited = new HashSet<Vec3d>();
            var nodes = new Dictionary<Vec3d, AStarNode>()
            {
                { from, new AStarNode(from, 0, Heuristics(from, to)) }
            };
            var queue = new List<Vec3d>() { from };

            while(queue.Count > 0)
            {
                queue.Sort((a, b) =>
                {
                    var heuristics = Heuristics(a, to) - Heuristics(b, to);

                    if (heuristics < 0) return -1;
                    if (heuristics > 0) return 1;

                    return 0;
                });

                var currentPosition = queue.First();
                queue.RemoveAt(0);
                var currentNode = nodes[currentPosition];

                if(currentPosition == to || matcher?.Invoke(currentPosition, to) == true)
                {
                    var backtrackNode = currentNode;
                    do
                    {
                        if (backtrackNode == null)
                        {
                            break;
                        }

                        path.Positions.Add(backtrackNode.position);
                        backtrackNode = backtrackNode.cameFrom;
                    } while (backtrackNode != null && backtrackNode.position != from);

                    var reversedPath = path.Positions.Reverse().ToList();
                    return new Path() { Positions = reversedPath };
                }

                foreach(var neighbor in GetWalkableNeighbors(currentPosition))
                {
                    var cummulativeGScore = currentNode.gScore + 1;
                    nodes.TryGetValue(neighbor, out var neighborNode);
                    
                    if(cummulativeGScore >= (neighborNode?.gScore ?? int.MaxValue))
                    {
                        continue;
                    }

                    if(neighborNode != null)
                    {
                        neighborNode.gScore = cummulativeGScore;
                        neighborNode.fScore = cummulativeGScore + Heuristics(neighbor, to);
                        neighborNode.cameFrom = currentNode;
                    }
                    else
                    {
                        nodes.Add(neighbor, new AStarNode(neighbor, cummulativeGScore, cummulativeGScore + Heuristics(neighbor, to))
                        {
                            cameFrom = currentNode,
                        });
                    }

                    if(visited.Contains(neighbor))
                    {
                        continue;
                    }

                    queue.Add(neighbor);
                }
            }

            return null;
        }

        private int Heuristics(Vec3d current, Vec3d to)
        {
            return (int)Math.Ceiling(current.Distance2d(to));
        }

        private IList<Vec3d> GetWalkableNeighbors(Vec3d position)
        {
            return GetNeighbors(position).Where(pos => costMatrix.IsWalkable(pos)).ToList();
        }

        private IList<Vec3d> GetNeighbors(Vec3d position)
        {
            return new List<Vec3d>() {
                position + Vec3d.Up,
                position + Vec3d.Right,
                position + Vec3d.Down,
                position + Vec3d.Left,
            };
        }

        private class AStarNode
        {
            public Vec3d position;
            public AStarNode cameFrom;
            public int gScore = int.MaxValue;
            public int fScore = 0;

            public AStarNode(Vec3d position, int gScore = int.MaxValue, int fScore = 0)
            {
                this.position = position;
                this.gScore = gScore;
                this.fScore = fScore;
            }
        }
    }
}

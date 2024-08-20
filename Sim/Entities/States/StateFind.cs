using Sim.Logic.State;
using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Sim.Logic;

namespace Sim.Entities.states
{
    public class StateFind : IState
    {
        public IWithPosition PositionObject => Entity != null ? (IWithPosition)Entity : Object;

        public IEntity Entity { get; set; }
        public IObject Object { get; set; }

        private readonly Action<IObject> onObjectFound;
        private readonly Action<IEntity> onEntityFound;
        private readonly Action onSearchFailed;
        private readonly Func<IObject, bool> filterObject;
        private readonly Func<IEntity, bool> filterEntity;

        public StateFind(Func<IObject, bool> filterObject, Action<IObject> onObjectFound, Action onSearchFailed = null)
        {
            this.onObjectFound = onObjectFound;
            this.filterObject = filterObject;
            this.onSearchFailed = onSearchFailed;
        }

        public StateFind(Func<IEntity, bool> filterEntity, Action<IEntity> onEntityFound, Action onSearchFailed = null)
        {
            this.onEntityFound = onEntityFound;
            this.filterEntity = filterEntity;
            this.onSearchFailed = onSearchFailed;
        }

        public void Tick()
        {
            if(onObjectFound != null && filterObject != null)
            {
                var foundObject = FindObject();
                if (foundObject != null)
                {
                    onObjectFound(foundObject);
                }
                else
                {
                    onSearchFailed?.Invoke();
                }
            }
            else if(onEntityFound != null && filterEntity != null)
            {
                var foundEntity = FindEntity();
                if (foundEntity != null)
                {
                    onEntityFound(foundEntity);
                }
                else
                {
                    onSearchFailed?.Invoke();
                }
            }
        }

        private IObject FindObject()
        {
            return new List<IObject>(PositionObject.World.GetObjects())
                .OrderBy(a => a.Position.Distance2d(PositionObject.Position))
                .Where(filterObject)
                .FirstOrDefault();
        }

        private IEntity FindEntity()
        {
            return new List<IEntity>(PositionObject.World.GetEntities())
                .OrderBy(a => a.Position.Distance2d(PositionObject.Position))
                .Where(filterEntity)
                .FirstOrDefault();
        }
    }
}

using Sim.Capabilities;
using Sim.Const;
using Sim.Logic.State;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Entities.states;
using Sim.Entities.States;
using Sim.Objects;
using Sim.Renderables;

namespace Sim.Entities
{
    public class EntityMiner : BaseEntity
    {
        private TextRenderable debugText = new TextRenderable("", new Vec3d(10, 0, 0), null);
        private readonly StateMachine stateMachine;

        private ObjectTree target;

        public EntityMiner(Vec3d position)
        {
            Position = position;

            RenderableChildren.Add(debugText);

            AddCapability(new CapabilityCollision(new List<CollisionLayers>() { CollisionLayers.Default }));
            AddCapability(new CapabilityLiving(100));
            AddCapability(new CapabilityWalking());
            AddCapability(new CapabilityAttack(1.0f, 1.0f));
            AddCapability(new CapabilityInventory(32));

            stateMachine = new StateMachine(this);
            stateMachine.AddState(new StateFind(FilterObjects, OnObjectFound));
            stateMachine.AddState(new StateWalking(OnDestinationReached));
            stateMachine.AddState(new StateAttack());
        }

        public override void Tick()
        {
            TickCapabilities();


            if (target?.World == null)
            {
                stateMachine.SwitchState<StateFind>();
            }

            debugText.World = World;
            debugText.Text = stateMachine.GetCurrentState<IState>().GetType().Name;

            stateMachine.Tick();
        }

        private void OnDestinationReached()
        {
            var attack = GetCapability<CapabilityAttack>();
            if (attack == null)
            {
                return;
            }

            stateMachine.SwitchState<StateAttack>().GetState<StateAttack>().SetTarget(target);
        }

        private bool FilterObjects(IObject obj)
        {
            return obj is ObjectTree && !(
                World.HasPositionObjectAt(obj.Position + Vec3d.Up, po => po != this)
                && World.HasPositionObjectAt(obj.Position + Vec3d.Right, po => po != this)
                && World.HasPositionObjectAt(obj.Position + Vec3d.Down, po => po != this)
                && World.HasPositionObjectAt(obj.Position + Vec3d.Left, po => po != this)
            );
        }

        private void OnObjectFound(IObject obj)
        {
            target = obj as ObjectTree;
            if (target != null)
            {
                stateMachine
                    .SwitchState<StateWalking>()
                    .GetState<StateWalking>()
                    .SetTargetPosition(target.Position)
                    .SetMaximumDistanceToTarget(1);
            }
        }
    }
}

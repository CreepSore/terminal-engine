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
        private readonly TextRenderable debugText = new TextRenderable("", new Vec3d(10, 0, 0), null);
        private readonly StateMachine stateMachine;

        private ObjectTree target;

        private readonly CapabilityCollision collision = new CapabilityCollision(new List<CollisionLayers>() { CollisionLayers.Default });
        private readonly CapabilityLiving living = new CapabilityLiving(100);
        private readonly CapabilityWalking walking = new CapabilityWalking() {NoLimits = true};
        private readonly CapabilityAttack attack = new CapabilityAttack(1.0f, 1000.0f) {NoLimits = true};
        private readonly CapabilityInventory inventory = new CapabilityInventory(32);
        private readonly CapabilityCrafting craft = new CapabilityCrafting(Items.Items.GetAll());
        private readonly CapabilityBuild build = new CapabilityBuild();

        public EntityMiner(Vec3d position)
        {
            Position = position;

            RenderableChildren.Add(debugText);

            AddCapability(collision);
            AddCapability(living);
            AddCapability(walking);
            AddCapability(attack);
            AddCapability(inventory);
            AddCapability(craft);
            AddCapability(build);

            stateMachine = new StateMachine(this);
            stateMachine.AddState(new StateIdle());
            stateMachine.AddState(new StateFind(FilterObjects, OnObjectFound, OnSearchFailed));
            stateMachine.AddState(new StateWalking(OnDestinationReached));
            stateMachine.AddState(new StateAttack());
            stateMachine.SwitchState<StateFind>();
        }

        public override void Tick()
        {
            TickCapabilities();

            if (inventory.Stacks.Count / (double)inventory.MaximumStacks > 0.75)
            {
                if (craft.CraftItems(Items.Items.ItemChest) != null)
                {
                    if (build.PlaceItem(Items.Items.ItemChest, Position, false) is ObjectChest placedObject)
                    {
                        inventory.TransferFullInventory(placedObject.GetCapability<CapabilityInventory>());
                    }
                }
            }

            debugText.World = World;
            debugText.Text = stateMachine.GetCurrentState<IState>().GetType().Name + " " + inventory.Stacks.Count;

            stateMachine.Tick();

            if (target?.World == null && !(stateMachine.GetCurrentState<IState>() is StateIdle))
            {
                stateMachine.SwitchState<StateFind>();
            }
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
            var collision = GetCapability<CapabilityCollision>();

            return obj is ObjectTree && !(
                World.HasCapabilityObjectAt<CapabilityCollision>(obj.Position + Vec3d.Up, co => co != this && co.GetCapability<CapabilityCollision>().CanCollideWith(collision)).Any()
                && World.HasCapabilityObjectAt<CapabilityCollision>(obj.Position + Vec3d.Right, co => co != this && co.GetCapability<CapabilityCollision>().CanCollideWith(collision)).Any()
                && World.HasCapabilityObjectAt<CapabilityCollision>(obj.Position + Vec3d.Down, co => co != this && co.GetCapability<CapabilityCollision>().CanCollideWith(collision)).Any()
                && World.HasCapabilityObjectAt<CapabilityCollision>(obj.Position + Vec3d.Left, co => co != this && co.GetCapability<CapabilityCollision>().CanCollideWith(collision)).Any()
            );
        }

        private void OnObjectFound(IObject obj)
        {
            target = obj as ObjectTree;
            if (target != null)
            {
                if (attack.InRangeTo(target.Position))
                {
                    stateMachine
                        .SwitchState<StateAttack>()
                        .GetState<StateAttack>()
                        .SetTarget(target);

                    return;
                }

                stateMachine
                    .SwitchState<StateWalking>()
                    .GetState<StateWalking>()
                    .SetTargetPosition(target.Position)
                    .SetMaximumDistanceToTarget(attack.Range);
            }
        }

        private void OnSearchFailed()
        {
            stateMachine.SwitchState<StateIdle>();
        }
    }
}

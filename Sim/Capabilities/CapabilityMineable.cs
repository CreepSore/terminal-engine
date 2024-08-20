using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Items;
using Sim.Objects;

namespace Sim.Capabilities
{
    public class CapabilityMineable : BaseCapability
    {
        private readonly LootTable lootTable;

        public CapabilityMineable(LootTable lootTable)
        {
            this.lootTable = lootTable;
        }

        public override void OnAttached()
        {
            var living = CapabilityObject.GetCapability<CapabilityLiving>();
            if (living == null)
            {
                return;
            }

            living.AfterDamage += AfterDamage;
        }

        private void AfterDamage(object sender, DamageEventArgs e)
        {
            var living = CapabilityObject.GetCapability<CapabilityLiving>();
            if (living == null || !living.IsDead)
            {
                return;
            }

            ICapabilityObject attacker = e.EntityAttacker != null
                ? (ICapabilityObject)e.EntityAttacker
                : e.ObjectAttacker;

            var inventoryCapability = attacker.GetCapability<CapabilityInventory>();

            var drops = lootTable.GetDrops();
            if (inventoryCapability == null)
            {
                attacker.World.SpawnObject(new ObjectItemBag(drops, PositionObject.Position), null, false);
                return;
            }

            var stacksToDrop = new List<ItemStack>();
            foreach (var drop in drops)
            {
                var failedStack = inventoryCapability.AddStack(drop);
                if (failedStack != null)
                {
                    stacksToDrop.Add(failedStack);
                }
            }

            if (stacksToDrop.Count > 0)
            {
                attacker.World.SpawnObject(new ObjectItemBag(drops, PositionObject.Position), null, false);
            }
        }
    }
}

using Sim.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Const;
using Sim.Items;
using Sim.Structs;

namespace Sim.Objects
{
    public class ObjectTree : BaseObject
    {
        public ObjectTree(Vec3d position)
        {
            Position = position;

            var living = new CapabilityLiving(100);
            living.AfterDamage += AfterDamage;
            AddCapability(living);
            
            AddCapability(new CapabilityCollision(new List<CollisionLayers>() { CollisionLayers.Default }));
            AddCapability(new CapabilityMineable(
                new LootTable()
                    .AddEntry(Items.Items.ItemWood, 1, 10, 10, 1)
                    .AddEntry(Items.Items.ItemWood, 2, 20, 5, 1)
                    .AddEntry(Items.Items.ItemWood, 3, 50, 2, 1)
                    .AddEntry(Items.Items.ItemWood, 4, 100, 1, 1)
            ));
        }

        private void AfterDamage(object sender, DamageEventArgs e)
        {
            if (GetCapability<CapabilityLiving>().IsDead)
            {
                World.DespawnObject(this);
            }
        }
    }
}

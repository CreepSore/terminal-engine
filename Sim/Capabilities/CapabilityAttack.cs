﻿using Sim.Const;
using Sim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Objects;

namespace Sim.Capabilities
{
    public class CapabilityAttack : BaseCapability
    {
        public float AttackSpeed { get; set; }
        public float Range { get; set; }

        private int idleTicks = 0;

        public CapabilityAttack(float attackSpeed = 1.0f, float range = 1.0f)
        {
            AttackSpeed = attackSpeed;
            Range = range;
        }

        public override void Tick()
        {
            if(idleTicks > 0)
            {
                idleTicks -= (int)(1 * AttackSpeed);

                if (idleTicks < 0)
                {
                    this.idleTicks = 0;
                }
            }
        }

        public bool Attack(ICapabilityObject obj, double damage, DamageType damageType = DamageType.None)
        {
            if (!CanAttackNow())
            {
                return false;
            }

            if (obj.Position.Distance2d(PositionObject.Position) > Range)
            {
                return false;
            }

            var entityLiving = obj.GetCapability<CapabilityLiving>();

            if (entityLiving == null)
            {
                return false;
            }

            if (Entity != null)
            {
                entityLiving.Damage(damage, damageType, Entity);
                // ! We only set the idle ticks when attacking with an object.
                // ! Objects do not get ticked, thus we couldn't attack anymore after one attack.
                idleTicks = 20;
            }
            else if (Object != null)
            {
                entityLiving.Damage(damage, damageType, Object);
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool CanAttackNow()
        {
            return idleTicks <= 0;
        }
    }
}

using Sim.Const;
using Sim.Entities;
using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Sim.Capabilities
{
    public class CapabilityLiving : BaseCapability
    {
        private bool isDead = false;
        private double maximumHealth;
        private double health;

        public bool IsDead => isDead;
        public double MaximumHealth => maximumHealth;
        public double Health => health;

        public EventHandler<DamageEventArgs> BeforeDamage;
        public EventHandler<HealEventArgs> BeforeHeal;
        public EventHandler<RevivalEventArgs> BeforeRevival;

        public EventHandler<DamageEventArgs> AfterDamage;
        public EventHandler<HealEventArgs> AfterHeal;
        public EventHandler<RevivalEventArgs> AfterRevival;

        public CapabilityLiving(double maximumHealth, double? health = null)
        {
            this.maximumHealth = maximumHealth;

            this.health = health != null
                ? (double)health
                : maximumHealth;
        }

        public override void Tick()
        {

        }

        public bool Damage(double damage, DamageType damageType, IEntity attacker)
        {
            BeforeDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType, EntityAttacker = attacker });
            var result = InternalDamage(damage, damageType);
            AfterDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType, EntityAttacker = attacker });
            return result;
        }

        public bool Damage(double damage, DamageType damageType, IObject attacker)
        {
            BeforeDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType, ObjectAttacker = attacker });
            var result = InternalDamage(damage, damageType);
            AfterDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType, ObjectAttacker = attacker });
            return result;
        }

        public bool Damage(double damage, DamageType damageType)
        {
            BeforeDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType });
            var result = InternalDamage(damage, damageType);
            AfterDamage?.Invoke(this, new DamageEventArgs { Damage = damage, DamageType = damageType });
            return result;
        }

        private bool InternalDamage(double damage, DamageType damageType)
        {
            if (isDead)
            {
                return false;
            }

            var newHealth = health - damage;
            if (newHealth <= 0)
            {
                isDead = true;
                newHealth = 0;
            }

            health = newHealth;

            return true;
        }

        public bool Heal(double value, HealType healType, IEntity healer)
        {
            BeforeHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType, EntityHealer = healer });
            var result = InternalHeal(value, healType);
            AfterHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType, EntityHealer = healer });
            return result;
        }

        public bool heal(double value, HealType healType, IObject healer)
        {
            BeforeHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType, ObjectHealer = healer });
            var result = InternalHeal(value, healType);
            AfterHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType, ObjectHealer = healer });
            return result;
        }

        public bool Heal(double value, HealType healType)
        {
            BeforeHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType });
            var result = InternalHeal(value, healType);
            AfterHeal?.Invoke(this, new HealEventArgs { Value = value, HealType = healType });
            return result;
        }

        private bool InternalHeal(double value, HealType healType)
        {
            if (IsDead)
            {
                return false;
            }

            health = Math.Min(health + value, maximumHealth);

            return true;
        }

        public bool Revive(RevivalType revivalType, IEntity reviver)
        {
            BeforeRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType, EntityReviver = reviver });
            var result = InternalRevive(revivalType);
            AfterRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType, EntityReviver = reviver });
            return result;
        }

        public bool Revive(RevivalType revivalType, IObject reviver)
        {
            BeforeRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType, ObjectReviver = reviver });
            var result = InternalRevive(revivalType);
            AfterRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType, ObjectReviver = reviver });
            return result;
        }

        public bool Revive(RevivalType revivalType)
        {
            BeforeRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType });
            var result = InternalRevive(revivalType);
            AfterRevival?.Invoke(this, new RevivalEventArgs { RevivalType = revivalType });
            return result;
        }

        private bool InternalRevive(RevivalType revivalType)
        {
            if (!IsDead)
            {
                return false;
            }

            if (maximumHealth <= 0)
            {
                return false;
            }

            health = 1;
            isDead = false;

            return true;
        }
    }

    #region EventArgs
    public class DamageEventArgs : EventArgs
    {
        public double Damage { get; set; }
        public DamageType DamageType { get; set; }
        public IEntity EntityAttacker { get; set; }
        public IObject ObjectAttacker { get; set; }
    }

    public class HealEventArgs : EventArgs
    {
        public double Value { get; set; }
        public HealType HealType { get; set; }
        public IEntity EntityHealer { get; set; }
        public IObject ObjectHealer { get; set; }
    }

    public class RevivalEventArgs : EventArgs
    {
        public RevivalType RevivalType { get; set; }
        public IEntity EntityReviver { get; set; }
        public IObject ObjectReviver { get; set; }
    }
    #endregion
}

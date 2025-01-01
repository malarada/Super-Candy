using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp330;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SuperCandy.API
{
    public enum PowerRarity
    {
        None,
        Legendary,
        Mythical,
        Rare,
        Uncommon,
        Common,
    }

    public class Wizard
    {
        public Player Player { get; internal set; }
        public List<CustomPower> Powers { get; set; } = new List<CustomPower>();
        public static List<Wizard> List { get; } = new List<Wizard>();

        public static Wizard TryGet(Player player)
        {
            return List.FirstOrDefault(obj => obj.Player.Equals(player));
        }
    }

    public class ReceivePowerEventArgs
    {
        public Wizard Wizard;

        public ReceivePowerEventArgs(Wizard wizard)
        {
            this.Wizard = wizard;
        }
    }

    public class PowerSpecialEventArgs
    {
        public Wizard Wizard;

        public PowerSpecialEventArgs(Wizard wizard)
        {
            this.Wizard = wizard;
        }
    }

    public abstract class CustomPower
    {
        public byte Intensity = 0;
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract CandyKindID Candy { get; }
        public abstract PowerRarity Rarity { get; }
        public virtual bool Register { get; } = true;

        public virtual void Enabled()
        {
        }

        public virtual void Disabled()
        {
        }

        public virtual void Recieve(ReceivePowerEventArgs ev)
        {
        }

        public virtual void Special(PowerSpecialEventArgs ev)
        {
        }

        public virtual void IncreaseIntensity(Player player, byte amount = 1)
        {
            this.Intensity += amount;
            OnReceive(player);
            if (this.Intensity == 1)
            {
                Enabled();
            }
        }

        public virtual CustomPower TryGet(Player player)
        {
            return Wizard.TryGet(player).Powers.FirstOrDefault(t => t.Equals(this));
        }

        public virtual CustomPower TryGet(Wizard wizard)
        {
            return wizard.Powers.FirstOrDefault(t => t.Equals(this));
        }

        public virtual bool Check(Player player)
        {
            var power = Wizard.TryGet(player).Powers.FirstOrDefault(t => t.Equals(this));
            return power != null && power.Intensity > 0;
        }

        public virtual bool Check(Wizard wizard)
        {
            var power = wizard.Powers.FirstOrDefault(t => t.Equals(this));
            return power != null && power.Intensity > 0;
        }

        public static IEnumerable<CustomPower> RegisterPowers(Assembly assembly = null)
        {
            List<CustomPower> powers = new List<CustomPower>();
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes().Where(t => t.BaseType == typeof(CustomPower) && !t.IsAbstract).ToList())
            {
                CustomPower power = null;
                power = (CustomPower)Activator.CreateInstance(type);
                if (!power.Register) continue;
                if (power.TryRegister())
                    powers.Add(power);
            }

            return powers;
        }

        public static HashSet<CustomPower> Registered { get; } = new HashSet<CustomPower>();

        internal bool TryRegister()
        {
            if (!Registered.Contains(this))
            {
                Enabled();
                Registered.Add(this);
                Log.Debug($"Power {Name} registered");
                return true;
            }
            return false;
        }
        
        internal void OnReceive(Player player)
        {
            ReceivePowerEventArgs eventArgs = new ReceivePowerEventArgs(Wizard.TryGet(player));
            Recieve(eventArgs);
        }

        internal void OnSpecial(Player player)
        {
            if (!Check(player)) return;
            PowerSpecialEventArgs eventArgs = new PowerSpecialEventArgs(Wizard.TryGet(player));
            Special(eventArgs);
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomPower other)
            {
                return Name.Equals(other.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public CustomPower Clone()
        {
            return (CustomPower)MemberwiseClone();
        }
    }

    public abstract class CustomUltimate : CustomPower
    {
        public abstract bool CheckUltimate();

        public static IEnumerable<CustomUltimate> RegisterUltimates(Assembly assembly = null)
        {
            List<CustomUltimate> powers = new List<CustomUltimate>();
            assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes().Where(t => t.BaseType == typeof(CustomUltimate)).ToList())
            {
                CustomUltimate power = null;
                power = (CustomUltimate)Activator.CreateInstance(type);
                if (!power.Register) continue;
                if (power.TryRegister())
                    powers.Add(power);
            }

            return powers;
        }

        internal new bool TryRegister()
        {
            if (!Registered.Contains(this))
            {
                Registered.Add(this);
                Log.Debug($"Ultimate {Name} registered.");
                return true;
            }
            return false;
        }

        public virtual new CustomUltimate TryGet(Player player)
        {
            return (CustomUltimate)Wizard.TryGet(player).Powers.FirstOrDefault(t => t.Equals(this));
        }
    }
}

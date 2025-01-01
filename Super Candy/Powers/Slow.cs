using Exiled.API.Enums;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    internal class Slow : CustomPower
    {
        public override string Name => "Slow";
        public override string Description => "Anti-207.";

        public override CandyKindID Candy => CandyKindID.Green;

        public override PowerRarity Rarity => PowerRarity.Rare;

        public override void Recieve(ReceivePowerEventArgs ev)
        {
            ev.Wizard.Player.EnableEffect(EffectType.AntiScp207, intensity: this.Intensity);
        }
    }
}

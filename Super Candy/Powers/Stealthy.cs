using Exiled.API.Enums;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    internal class Stealthy : CustomPower
    {
        public override bool Register => false;
        public override string Name => "Stealthy";
        public override string Description => "Stealthy.";

        public override CandyKindID Candy => CandyKindID.Rainbow;

        public override PowerRarity Rarity => PowerRarity.Uncommon;

        public override void Recieve(ReceivePowerEventArgs ev)
        {
            ev.Wizard.Player.EnableEffect(EffectType.SilentWalk, (byte)((byte)5 * this.Intensity));
        }
    }
}

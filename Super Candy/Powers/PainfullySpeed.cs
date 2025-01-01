using Exiled.API.Enums;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    internal class PainfullySpeed : CustomPower
    {
        public override string Name => "PainfullySpeed";
        public override string Description => "207.";

        public override CandyKindID Candy => CandyKindID.Red;

        public override PowerRarity Rarity => PowerRarity.Rare;

        public override void Recieve(ReceivePowerEventArgs ev)
        {
            ev.Wizard.Player.EnableEffect(EffectType.Scp207, intensity: this.Intensity);
        }
    }
}

using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    public class Tank : CustomPower
    {
        public override string Name => "Tank";
        public override string Description => "More health.";

        public override CandyKindID Candy => CandyKindID.Red;

        public override PowerRarity Rarity => PowerRarity.Common;

        public override void Recieve(ReceivePowerEventArgs ev)
        {
            ev.Wizard.Player.MaxHealth += 10;
        }
    }
}

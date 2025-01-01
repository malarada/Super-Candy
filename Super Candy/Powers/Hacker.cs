using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    public class Hacker : CustomPower
    {
        public override string Name => "Hacker";
        public override string Description => "Activate generators faster.";

        public override CandyKindID Candy => CandyKindID.Blue;

        public override PowerRarity Rarity => PowerRarity.Uncommon;

        public override void Enabled()
        {
            Exiled.Events.Handlers.Player.ActivatingGenerator += PlayerActivatingGenerator;
        }

        private void PlayerActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            ev.Generator.ActivationTime = 120 / (this.Intensity + 1) + 4;
        }
    }
}

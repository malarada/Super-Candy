using System;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy.Powers
{
    internal class AntiSealant : CustomPower
    {
        public override bool Register => false;
        public override string Name => "AntiSealant";

        public override string Description => "";

        public override CandyKindID Candy => CandyKindID.Red;

        public override PowerRarity Rarity => PowerRarity.Rare;

        public override void Enabled()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += InteractingDoor;
        }

        private void InteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (new Random().Next(10) != 0) return;


        }
    }
}

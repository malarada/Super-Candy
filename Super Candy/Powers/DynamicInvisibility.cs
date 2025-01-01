using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using SuperCandy.API;

namespace SuperCandy
{
    internal class DynamicInvisibility : CustomPower
    {
        public override string Name => "DynamicInvisibility";
        public override string Description => "Invisible when slow walking.";
        public override bool Register => false;

        public override CandyKindID Candy => CandyKindID.Yellow;
        public override PowerRarity Rarity => PowerRarity.Uncommon;

        public override void Enabled()
        {
            Exiled.Events.Handlers.Player.ChangingMoveState += PlayerChanginMoveState;
        }

        private void PlayerChanginMoveState(ChangingMoveStateEventArgs ev)
        {
            Log.Debug(this.Intensity);
            if (ev.NewState == PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking)
            {
                ev.Player.EnableEffect(EffectType.Invisible, intensity: this.Intensity);
            }
            else if (ev.OldState == PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking)
            {
                ev.Player.DisableEffect(EffectType.Invisible);
            }
        }
    }
}

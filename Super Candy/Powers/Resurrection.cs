using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using SuperCandy.API;
using System;
using UnityEngine;

namespace SuperCandy.Powers
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Resurrect : Resurrection, ICommand
    {
        public string Command => "resurrect";

        public string[] Aliases => new string[] {
            "res"
        };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player.TryGet(sender, out Player player);
            var power = base.TryGet(player);
            if (power.CheckUltimate())
            {
                power.OnSpecial(player);

                foreach (var lePlayer in Player.List)
                {
                    if (lePlayer == player) continue;
                    if (lePlayer.IsDead) continue;

                    Log.Debug(Vector3.Distance(lePlayer.Position, player.Position));
                    if (Vector3.Distance(lePlayer.Position, player.Position) <= 10)
                    {
                        lePlayer.EnableEffect(Exiled.API.Enums.EffectType.Flashed, 5, true);
                    }
                }

                response = "Yes";
                return true;
            }

            response = $"No. ";
            return false;
        }
    }

    internal class Resurrection : CustomUltimate
    {
        internal int SoulsConsumed = 0;
        internal RoleTypeId LastRole;

        public override string Name => "Resurrection";
        public override string Description => "Collect five souls, then resurrect after death.";

        public override CandyKindID Candy => CandyKindID.Rainbow;

        public override PowerRarity Rarity => PowerRarity.Legendary;

        public override void Enabled()
        {
            Exiled.Events.Handlers.Player.Died += Died;
            base.Enabled();
        }

        public override bool CheckUltimate()
        {
            Log.Debug($"Souls: {SoulsConsumed}");
            if (SoulsConsumed >= 5)
                return true;

            return false;
        }

        public override void Special(PowerSpecialEventArgs ev)
        {
            if (!ev.Wizard.Player.IsDead) return;
            ev.Wizard.Player.Role.Set(LastRole, Exiled.API.Enums.SpawnReason.Respawn, RoleSpawnFlags.None);
        }

        private void Died(DiedEventArgs ev)
        {
            Log.Debug($"Player: {ev.Player}");
            Log.Debug($"Wizard: {Wizard.TryGet(ev.Player)}");
            Log.Debug($"Check: {Check(ev.Player)}");
            if (Check(ev.Player))
            {
                Log.Debug(ev.TargetOldRole);
                LastRole = ev.TargetOldRole;
            }
            else if (Check(ev.Attacker))
            {
                SoulsConsumed++;
                ev.Player.Broadcast(1, $"{SoulsConsumed} souls.");
            }
        }
    }
}

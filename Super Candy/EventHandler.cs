using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp330;
using PlayerRoles;
using SuperCandy.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCandy
{
    internal class EventHandler
    {
        internal static void PlayerVerified(VerifiedEventArgs ev)
        {
            Wizard.List.Add(new Wizard()
            {
                Player = ev.Player,
                Powers = new List<CustomPower>()
            });
        }

        internal static void PlayerSpawning(SpawningEventArgs ev)
        {
            if (Wizard.TryGet(ev.Player) == null)
                Wizard.List.Add(new Wizard()
                {
                    Player = ev.Player,
                    Powers = new List<CustomPower>()
                });

            Log.Debug($"Player: {ev.Player}");
            if (ev.Player.Role != RoleTypeId.Spectator)
            {
                Wizard.TryGet(ev.Player).Powers.Clear();
            }

            /*
            Log.Debug($"Player role: {ev.Player.Role.Type}");
            Log.Debug($"IsLobby: {Round.IsLobby}");
            Log.Debug($"Is spectator: {ev.Player.Role.Type == RoleTypeId.Spectator}");
            Log.Debug($"Was dead: {ev.OldRole.IsDead}");
            Log.Debug($"Count: {Wizard.TryGet(ev.Player).Powers.Count}");
            // When ev.OldRole is finally set to an instance, this is what should happen.
            if (Round.IsLobby || ev.Player.Role.Type == RoleTypeId.Spectator) return;
            if (!(SuperCandyPlugin.Instance.Config.KeepPowersOnEscape && !ev.OldRole.IsDead) && Wizard.TryGet(ev.Player).Powers.Count > 0)
            {
                Wizard.TryGet(ev.Player).Powers.Clear();
            }
            */
        }

        internal static void PlayerLeft(LeftEventArgs ev)
        {
            Wizard.List.Remove(Wizard.TryGet(ev.Player));
        }
        
        internal static void ToggleNoClip(TogglingNoClipEventArgs ev)
        {
            foreach (CustomPower power in Wizard.TryGet(ev.Player).Powers)
            {
                if (power is CustomUltimate ultimate)
                {
                    if (!ultimate.CheckUltimate())
                        continue;
                }
                power.OnSpecial(ev.Player);
            }
        }

        internal static void Eaten330(EatenScp330EventArgs ev)
        {
            List<CustomPower> colorPowers = CustomPower.Registered.Where(t => t.Candy == ev.Candy.Kind).ToList();

            List<CustomPower> weightedPowers = new List<CustomPower>();

            foreach (var colorPower in colorPowers)
            {
                for (int i = 0; i < (int)colorPower.Rarity; i++)
                {
                    weightedPowers.Add(colorPower);
                }
            }

            Wizard wizard = Wizard.TryGet(ev.Player);
            if (colorPowers.Count == 0) return;
            Random random = new Random();
            CustomPower power = weightedPowers[random.Next(weightedPowers.Count)];
            CustomPower customPower;

            if (!wizard.Powers.Contains(power))
            {
                customPower = power.Clone();
                wizard.Powers.Add(customPower);
            }
            else
            {
                customPower = wizard.Powers.FirstOrDefault(t => t.Name == power.Name);
            }
            customPower.IncreaseIntensity(ev.Player);

            ev.Player.Broadcast(5, $"You gained {power.Name}.\n{power.Description}");
        }
    }
}

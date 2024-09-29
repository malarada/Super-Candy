using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp330;
using InventorySystem;
using System.Collections.Generic;

using UnityEngine;

using MEC;

namespace Super_Candy_Plugin
{
    public class EventHandler
    {
        private static Dictionary<int, Dictionary<string, int>> PowerIntensities = new Dictionary<int, Dictionary<string, int>>();
        private static Dictionary<string, int> DefaultPowerIntensities = new Dictionary<string, int>
        {
            {
                "DynamicInvisibility", 0
            },
            {
                "Hacker", 0
            },
            {
                "Tank", 0
            },
            {
                "Wraith", 0
            },
            {
                "Stealthy", 0
            },
            {
                "Slow", 0
            },
            {
                "Healing", 0
            },
            {
                "Speed", 0
            },
        };
        private static System.Random Random = new System.Random();

        private static void ResetPlayerStats(Player player)
        {
            if (!PowerIntensities.ContainsKey(player.Id))
            {
                PowerIntensities.Add(player.Id, new Dictionary<string, int>());
                foreach (var kvp in DefaultPowerIntensities)
                {
                    PowerIntensities[player.Id].Add(kvp.Key, kvp.Value);
                }
            }
        }

        internal static void PlayerVerified(VerifiedEventArgs ev)
        {
            ResetPlayerStats(ev.Player);
        }

        public static Dictionary<int, Dictionary<ushort, InventorySystem.Items.ItemBase>> PlayerItems = new Dictionary<int, Dictionary<ushort, InventorySystem.Items.ItemBase>>();
        public static Dictionary<int, Dictionary<ItemType, ushort>> PlayerAmmo = new Dictionary<int, Dictionary<ItemType, ushort>>();

        internal static void TogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (PowerIntensities[ev.Player.Id]["Wraith"] > 0)
            {
                ev.Player.EnableEffect(EffectType.Ghostly, duration: 5);
                if (!PlayerItems.ContainsKey(ev.Player.Id))
                    PlayerItems.Add(ev.Player.Id, new Dictionary<ushort, InventorySystem.Items.ItemBase>());
                else
                    PlayerItems[ev.Player.Id].Clear();
                foreach (var kvp in ev.Player.Inventory.UserInventory.Items)
                {
                    PlayerItems[ev.Player.Id].Add(kvp.Key, kvp.Value);
                }
                if (!PlayerAmmo.ContainsKey(ev.Player.Id))
                    PlayerAmmo.Add(ev.Player.Id, new Dictionary<ItemType, ushort>());
                else
                    PlayerAmmo[ev.Player.Id].Clear();
                foreach (var kvp in ev.Player.Inventory.UserInventory.ReserveAmmo)
                {
                    PlayerAmmo[ev.Player.Id].Add(kvp.Key, kvp.Value);
                }
                ev.Player.ClearInventory();
                ev.Player.Handcuff();
                Timing.CallDelayed(5, () =>
                {
                    ev.Player.RemoveHandcuffs();
                    foreach (var kvp in EventHandler.PlayerItems[ev.Player.Id])
                    {
                        ev.Player.Inventory.ServerAddItem(kvp.Value.ItemTypeId);
                    }
                    foreach (var kvp in EventHandler.PlayerAmmo[ev.Player.Id])
                    {
                        ev.Player.Inventory.UserInventory.ReserveAmmo.Add(kvp.Key, kvp.Value);
                    }
                });
            }
        }

        internal static void PlayerChangingMoveState(ChangingMoveStateEventArgs ev)
        {
            if (Round.IsLobby) return;
            if (PowerIntensities[ev.Player.Id]["DynamicInvisibility"] > 0)
            {
                if (ev.NewState == PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking)
                {
                    ev.Player.EnableEffect(EffectType.Invisible, intensity: (byte)PowerIntensities[ev.Player.Id]["DynamicInvisibility"]);
                }
                else if (ev.OldState == PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking)
                {
                    ev.Player.DisableEffect(EffectType.Invisible);
                }
            }
        }

        internal static void PlayerActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            ev.Generator.ActivationTime = 120 / (PowerIntensities[ev.Player.Id]["Hacker"] + 1) + 4;
        }

        internal static void PlayerSpawning(SpawningEventArgs ev)
        {
            if (Round.IsLobby)
                return;

            if (!(ev.Player.Role.Type != PlayerRoles.RoleTypeId.NtfSpecialist || ev.Player.Role.Type != PlayerRoles.RoleTypeId.ChaosConscript))
            {
                ev.Player.MaxHealth += 10 * PowerIntensities[ev.Player.Id]["Tank"];
                ev.Player.Health = ev.Player.MaxHealth;
            }
        }

        internal static void EatenScp330(EatenScp330EventArgs ev)
        {
            int power;
            if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Rainbow)
            {
                PowerIntensities[ev.Player.Id]["Stealthy"] += 1;
                ev.Player.EnableEffect(EffectType.SilentWalk, (byte)((byte)5 * PowerIntensities[ev.Player.Id]["Stealthy"]));
            }
            else if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Yellow)
            {
                PowerIntensities[ev.Player.Id]["DynamicInvisibility"] += 1;
            }
            else if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Purple)
            {
                PowerIntensities[ev.Player.Id]["Wraith"] += 1;
            }
            else if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Red)
            {
                power = Random.Next(2);
                if (power == 0)
                {
                    PowerIntensities[ev.Player.Id]["Tank"] += 1;
                    ev.Player.MaxHealth += 10;
                }
                else if (power == 1)
                {
                    PowerIntensities[ev.Player.Id]["Speed"] += 1;
                    ev.Player.EnableEffect(EffectType.Scp207);
                }
            }
            else if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Green)
            {
                power = Random.Next(2);
                if (power == 0)
                {
                    PowerIntensities[ev.Player.Id]["Slow"] += 1;
                    ev.Player.EnableEffect(EffectType.AntiScp207);
                }
                else if (power == 1)
                {
                    PowerIntensities[ev.Player.Id]["Healing"] += 1;
                    ev.Player.MaxArtificialHealth += 25;
                }
            }
            else if (ev.Candy.Kind == InventorySystem.Items.Usables.Scp330.CandyKindID.Blue)
            {
                PowerIntensities[ev.Player.Id]["Hacker"] += 1;
            }
        }
    }
}

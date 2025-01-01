using Exiled.API.Enums;
using InventorySystem;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using SuperCandy.API;
using System.Collections.Generic;

namespace SuperCandy.Powers
{
    public class Wraith : CustomPower
    {
        public static Dictionary<int, Dictionary<ushort, InventorySystem.Items.ItemBase>> PlayerItems = new Dictionary<int, Dictionary<ushort, InventorySystem.Items.ItemBase>>();
        public static Dictionary<int, Dictionary<ItemType, ushort>> PlayerAmmo = new Dictionary<int, Dictionary<ItemType, ushort>>();

        public override string Name => "Wraith";
        public override string Description => "Faze through walls.";

        public override CandyKindID Candy => CandyKindID.Purple;

        public override PowerRarity Rarity => PowerRarity.Rare;

        public override void Special(PowerSpecialEventArgs ev)
        {
            ev.Wizard.Player.EnableEffect(EffectType.Ghostly, duration: 5);
            if (!PlayerItems.ContainsKey(ev.Wizard.Player.Id))
                PlayerItems.Add(ev.Wizard.Player.Id, new Dictionary<ushort, InventorySystem.Items.ItemBase>());
            else
                PlayerItems[ev.Wizard.Player.Id].Clear();
            foreach (var kvp in ev.Wizard.Player.Inventory.UserInventory.Items)
            {
                PlayerItems[ev.Wizard.Player.Id].Add(kvp.Key, kvp.Value);
            }
            if (!PlayerAmmo.ContainsKey(ev.Wizard.Player.Id))
                PlayerAmmo.Add(ev.Wizard.Player.Id, new Dictionary<ItemType, ushort>());
            else
                PlayerAmmo[ev.Wizard.Player.Id].Clear();
            foreach (var kvp in ev.Wizard.Player.Inventory.UserInventory.ReserveAmmo)
            {
                PlayerAmmo[ev.Wizard.Player.Id].Add(kvp.Key, kvp.Value);
            }
            ev.Wizard.Player.ClearInventory();
            ev.Wizard.Player.Handcuff();
            Timing.CallDelayed(5, () =>
            {
                ev.Wizard.Player.RemoveHandcuffs();
                foreach (var kvp in PlayerItems[ev.Wizard.Player.Id])
                {
                    ev.Wizard.Player.Inventory.ServerAddItem(kvp.Value.ItemTypeId, InventorySystem.Items.ItemAddReason.Undefined);
                }
                foreach (var kvp in PlayerAmmo[ev.Wizard.Player.Id])
                {
                    ev.Wizard.Player.Inventory.UserInventory.ReserveAmmo.Add(kvp.Key, kvp.Value);
                }
            });
        }
    }
}

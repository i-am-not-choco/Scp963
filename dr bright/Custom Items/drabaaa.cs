
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using GameCore;
using InventorySystem;
using InventorySystem.Items;
using PlayerRoles;
using CustomItem = Exiled.CustomItems.API.Features.CustomItem;
using EPlayer = Exiled.Events.Handlers.Player;
using Item = Exiled.API.Features.Items.Item;
using MEC;
using UnityEngine;

namespace dr_bright.Custom_Items
{

    [CustomItem(ItemType.SCP1344)]
    public class Googles : CustomItem
    {
        public override string Name { get; set; } = "Scp963";
        public override string Description { get; set; } = "DrBrights Hamlet";
        public override uint Id { get; set; } = Class1.Instance.Config.ID;
        public override float Weight { get; set; } = 0;
        protected override void ShowPickedUpMessage(Exiled.API.Features.Player player){ }
        protected override void ShowSelectedMessage(Exiled.API.Features.Player player) { }

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = Class1.Instance.Config.limit,
            RoomSpawnPoints =  Class1.Instance.Config.SpawnPoints
           
            //DynamicSpawnPoints =
         //   [
            //    new DynamicSpawnPoint
          //      {
         //           Chance = 100,
           //         Location = SpawnLocationType.InsideEscapePrimary
            //    },
               
                
           // ]
            


        };


        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            EPlayer.UsingItem += UsingItem;
            EPlayer.DroppingItem += OnDrop;
            EPlayer.Verified += BeforeRoundCheck;
            EPlayer.Dying += Idk;
            EPlayer.Escaping += noescape;
            EPlayer.ChangingRole += nospawn;
            EPlayer.Left += left;
        }

        protected override void UnsubscribeEvents()
        {
            EPlayer.Dying -= Idk;
            EPlayer.Verified -= BeforeRoundCheck;
            EPlayer.UsingItem -= UsingItem;
            EPlayer.DroppingItem -= OnDrop;
            EPlayer.Escaping -= noescape;
            EPlayer.Left -= left;
            EPlayer.ChangingRole -= nospawn;
            base.UnsubscribeEvents();
        }

        //private static RoleTypeId OldRole { get; set; }
        private static bool Used { get; set; }
        private static Exiled.API.Features.Player DrBat1 {get; set;}
         private static string DrBat2 {get; set;}
        private static RoleTypeId StartRole {get; set;}
        private void BeforeRoundCheck(VerifiedEventArgs ev)
        {
            if (RoundStart.RoundStarted == false)
            {
                Used = false;
              
            }
            else
            {
                if (ev.Player.UserId == DrBat2)
                {
                    DrBat1 = ev.Player;
                    Used = true;
                }
                
            }

        }

     


        private void OnDrop(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
           if (ev.Player == DrBat1)
           {
                ev.Player.Kill("Sudden cessation of life. No clear trauma");
           }
        }

        private void UsingItem(UsingItemEventArgs ev)
        {

            if (!Check(ev.Item))
            { return; }

            ev.IsAllowed = false;
          

            if (Used == !true)
            {
                StartRole = ev.Player.Role.Type;
                DrBat1 = ev.Player;
                DrBat2 = ev.Player.UserId; 
                ev.Player.CustomName = "Scp963(" + ev.Player.Nickname + ")";
                Used = true;
            }
            else
            {
                if (ev.Player == DrBat1) return;

              DrBat2 = DrBat1.UserId;



                ev.Player.RemoveItem(ev.Player.CurrentItem);
                Get(Class1.Instance.Config.ID).Give(DrBat1);
                List<ItemBase> inventory = new List<ItemBase>(ev.Player.Inventory.UserInventory.Items.Values);
                switch (Class1.Instance.Config.OldRole_Setstheroletotheoldplayerrole)
                {
                    case 0:

                        DrBat1.Role.Set(RoleTypeId.Scientist);
                        break;

                    case 1:

                        DrBat1.Role.Set(ev.Player.Role.Type);
                        break;

                    case 2:
                        DrBat1.Role.Set(StartRole);
                        break;
                    
                    default:
                        DrBat1.Role.Set(RoleTypeId.Scientist);
                        break;
                }

                DrBat1.ClearInventory();

                DrBat1.CustomName = "Scp963(" + ev.Player.Nickname + ")";
                if(Class1.Instance.Config.showasoldrole)
                {
                    Timing.CallDelayed(1f, () =>
                    {
                        DrBat1.ChangeAppearance(ev.Player.Role.Type);
                    });

                }



                DrBat1.Position = ev.Player.Position;
               DrBat1.Rotation = ev.Player.Rotation;
                foreach (var item in inventory)
                {
                    if (TryGet(item.ItemSerial, out CustomItem citem))
                    {
                        Get(citem.Id).Give(DrBat1);
                        ev.Player.Inventory.UserInventory.Items.Remove(item.ItemSerial);
                    }
                    else
                    {
                        ev.Player.Inventory.UserInventory.Items.Remove(item.ItemSerial);
                        ev.Player.Inventory.UserInventory.Items.Add(item.ItemSerial, item);
                        Item.Get(item).ChangeItemOwner(ev.Player, DrBat1);
                    }
                }
                DrBat1.AddAmmo(AmmoType.Nato9, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo9x19));
                DrBat1.AddAmmo(AmmoType.Nato556, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo556x45));
                DrBat1.AddAmmo(AmmoType.Nato762, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo762x39));
                DrBat1.AddAmmo(AmmoType.Ammo44Cal, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo44cal));
                DrBat1.AddAmmo(AmmoType.Ammo12Gauge, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo12gauge));
                
                
                ev.Player.ClearInventory();
                ev.Player.Role.Set(RoleTypeId.Spectator, SpawnReason.ItemUsage);
               

            }
        }

        private void Idk(DyingEventArgs ev)
        {
            if (ev.Player == DrBat1)
            {
                 Used = true;
                DrBat1.CustomName = null;
            }
        }
        private void left(LeftEventArgs ev)
        {
            if (ev.Player == DrBat1)
            {
                Used = false;
            }
        }
        private void noescape(EscapingEventArgs ev)
        {
            if (ev.Player == DrBat1)
            {
                ev.IsAllowed = false;
            }
        }
        private void nospawn(ChangingRoleEventArgs ev)
        {
            if (ev.Player == DrBat1)
            {
                if (ev.Reason == SpawnReason.Respawn)
                {
                    if (Class1.Instance.Config.spawninwave == false)
                    {
                        ev.IsAllowed = false;
                    }
                    else
                    {
                        DrBat2 = null;
                        Used = false;
                    }
                }
            }
        }
    }
}
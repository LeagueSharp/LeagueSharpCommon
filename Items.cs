﻿#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System.Collections.Generic;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class Items
    {
        /// <summary>
        /// Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(string name, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Name == name);
        }

        /// <summary>
        /// Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(int id, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Id == (ItemId)id);
        }

        /// <summary>
        /// Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        public static bool CanUseItem(string name)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell =>
                    (int)spell.Slot == slot.Slot + (int)SpellSlot.Item1);
                return inst != null && inst.State == SpellState.Ready;
            }

            return false;
        }

        /// <summary>
        /// Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        private static bool CanUseItem(int id)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell =>
                    (int)spell.Slot == slot.Slot + (int)SpellSlot.Item1);
                return inst != null && inst.State == SpellState.Ready;
            }

            return false;
        }

        /// <summary>
        /// Casts the item on the target.
        /// </summary>
        public static bool UseItem(string name, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                return target != null ? ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target) : ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
            }

            return false;
        }

        /// <summary>
        /// Casts the item on the target.
        /// </summary>
        private static bool UseItem(int id, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                return target != null ? ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target) : ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
            }

            return false;
        }

        /// <summary>
        /// Casts the item on a Vector2 position.
        /// </summary>
        private static bool UseItem(int id, Vector2 position)
        {
            return UseItem(id, position.To3D());
        }

        /// <summary>
        /// Casts the item on a Vector3 position.
        /// </summary>
        private static bool UseItem(int id, Vector3 position)
        {
            if (position == Vector3.Zero)
            {
                return false;
            }
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, position);
            }

            return false;
        }

        /// <summary>
        /// Returns the ward slot.
        /// </summary>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[] { 3340, 3350, 3361, 3154, 2045, 2049, 2050, 2044 };
            return (from wardId in wardIds
                    where CanUseItem(wardId)
                    select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)wardId))
                .FirstOrDefault();
        }

        public class Item
        {
            public int Id { get; private set; }
            private float _range;

            private float Range
            {
                get { return _range; }
                set
                {
                    _range = value;
                    RangeSqr = value * value;
                }
            }

            private float RangeSqr { get; set; }

            public List<SpellSlot> Slots
            {
                get
                {
                    return ObjectManager.Player.InventoryItems.Where(slot =>
                        slot.Id == (ItemId)Id).Select(slot => slot.SpellSlot).ToList();
                }
            }

            public Item(int id, float range = 0)
            {
                Id = id;
                Range = range;
            }

            private bool IsInRange(Obj_AI_Base target)
            {
                return IsInRange(target.ServerPosition);
            }

            private bool IsInRange(Vector2 target)
            {
                return IsInRange(target.To3D());
            }

            private bool IsInRange(Vector3 target)
            {
                return ObjectManager.Player.ServerPosition.Distance(target, true) < RangeSqr;
            }

            public bool IsOwned(Obj_AI_Hero target = null)
            {
                return HasItem(Id, target);
            }

            public bool IsReady()
            {
                return CanUseItem(Id);
            }

            public bool Cast()
            {
                return UseItem(Id);
            }

            public bool Cast(Obj_AI_Base target)
            {
                return UseItem(Id, target);
            }

            public bool Cast(Vector2 position)
            {
                return UseItem(Id, position);
            }

            public bool Cast(Vector3 position)
            {
                return UseItem(Id, position);
            }

            public void Buy()
            {
                ObjectManager.Player.BuyItem((ItemId)Id);
            }
        }
    }
}
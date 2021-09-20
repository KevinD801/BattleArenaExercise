using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;

        public override float AttackPower
        {
            get
            {
                if (_currentItem.ItemType == 1)
                {
                    return base.AttackPower + CurrentItem.StatBoost;
                }
                return base.AttackPower;
            }
        }

        public override float DefensePower
        {
            get
            {
                if (_currentItem.ItemType == 0)
                {
                    return base.DefensePower + CurrentItem.StatBoost;
                }
                return base.DefensePower;
            }

        }

        public Item CurrentItem
        {
            get
            {
                return _currentItem;
            }
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
        }

        /// <summary>
        /// Sets the item at given index to be the current item
        /// </summary>
        /// <param name="index">The index of the item in the array</param>
        /// <returns>False if the index is outside the bounds of the array</returns>
        public bool TryEquipItem(int index)
        {
            // 
            if (index >= _items.Length || index < 0)
            {
                return false;
            }

            // Set
            _currentItem = _items[index];

            return true;
        }

        /// <summary>
        /// Set the current item to be nothing
        /// </summary>
        /// <returns></returns>
        public bool TryRemoveCurrentItem()
        {
            // If the current is set to nothing...
            if (CurrentItem.Name == "Nothing")
            {
                //... return false
                return false;
            }

            // Set item to be nothing
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }

        /// <returns>Gets the names of all items in the player inventory</returns>
        public string[] GetItemNames()
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i++)
            {
                itemNames[i] = _items[i].Name;
            }
            return itemNames;
        }
    }
}

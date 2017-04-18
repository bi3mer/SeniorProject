using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : Inventory 
{
    private const string equipedAttributeName = "equiped";

    /// <summary>
    /// Creates an instance of PlayerInventory
    /// </summary>
    /// <param name="name">Name of inventory.</param>
    /// <param name="inventoryFile">File to load invetory from.</param>
    public PlayerInventory(string name, string inventoryFile, int size) : base(name, inventoryFile, size)
    {
        // TODO: load previous state
        equipedItem = null;
    }

    /// <summary>
    /// Subscription triggered when an item is equipped.
    /// </summary>
    public event ItemEquippedDelegate ItemEquippedSubscription;
    public delegate void ItemEquippedDelegate(BaseItem item);

    /// <summary>
    /// Subscription triggered when an equipped item is unequipped.
    /// </summary>
    public event ItemUnequippedDelegate ItemUnequippedSubscription;
    public delegate void ItemUnequippedDelegate();

    private BaseItem equipedItem;

    /// <summary>
    /// Get or set the currently equipped item.
    /// 
    /// Null represents no currently equipped tool.
    /// </summary>
    public BaseItem EquipedItem
    {
        get
        {
            return equipedItem;
        }

        set
        {
            // Check if this is an equippable item
            if (value != null && !value.Types.Contains(ItemTypes.Equipable))
            {
                Debug.LogError("Cannot set EquipedItem to a non-equipable item.");
                return;
            }

            // Unequip previous item
            if (equipedItem != null)
            {
                equipedItem.GetItemAttribute(equipedAttributeName).Value = 0f;

                // Notify unequipped subscribers if the new tool is empty
                if (value == null && ItemUnequippedSubscription != null) {
                    ItemUnequippedSubscription();
                }
            }

            equipedItem = value;

            // Equip new item
            if (value != null) 
            {
                value.GetItemAttribute(equipedAttributeName).Value = 1f;

                // Notify equipped subscribers
                if (ItemEquippedSubscription != null)
                {
                    ItemEquippedSubscription(value);
                }
            }
        }
    }
}
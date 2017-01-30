using UnityEngine;

public class PlayerInventory : Inventory 
{
    private const string equipedAttributeName = "equiped";
    private const string equipableItemTag = "equipable";

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
    /// Subscription triggered when an item is added to the inventory.
    /// </summary>
    public event ItemAddedDelegate ItemAddedSubscription;
    public delegate void ItemAddedDelegate(BaseItem item);

    /// <summary>
    /// Subscription triggered when an item is removed from the inventory.
    /// </summary>
    public event ItemRemovedDelegate ItemRemovedSubscription;
    public delegate void ItemRemovedDelegate(BaseItem item);

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

	/// <summary>
    /// Add item to inventory and notify subscribers.
    /// </summary>
    /// <returns>The added item.</returns>
    /// <param name="newItem">New item.</param>
    /// <param name="amount">Amount.</param>
    public Stack AddItem(BaseItem newItem, int amount)
    {
        Stack newStack = base.AddItem(newItem, amount);

        if (ItemAddedSubscription != null) 
        {
            ItemAddedSubscription(newItem);
        }

        return newStack;
    }

    /// <summary>
    /// Removes the item from the inventory and notifies subscribers.
    /// </summary>
    /// <param name="stack">Item to remove.</param>
    public void RemoveStack(Stack stack)
    {
        base.RemoveStack(stack);

        if (ItemRemovedSubscription != null)
        {
            ItemRemovedSubscription(stack.Item);
        }
    }

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
            if (value != null && !value.Types.Contains(equipableItemTag))
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

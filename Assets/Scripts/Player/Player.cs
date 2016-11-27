﻿using UnityEngine;

public class Player
{
    private const int maxHealth = 100;
    private const int maxWarmth = 100;
    private const int maxHunger = 100;
    private const string playerInventoryName = "player";
    private const string inventoryFileName = "InventoryYaml.yml";

    private int health;
    private int warmth;
    private int hunger;

    /// <summary>
    /// Player constructor.
    /// Initializes all fields to full.
    /// </summary>
    public Player ()
    {
        Health = MaxHealth = maxHealth;
        Warmth = MaxWarmth = maxWarmth;
        Hunger = MaxHunger = maxHunger;
		Inventory = new Inventory (playerInventoryName, inventoryFileName);
		EquippedTool = "";
    }

	/// <summary>
	/// Gets or sets the player controller if we are in a scene with the player controller.
	/// </summary>
	/// <value>The controller</value>
	public PlayerController Controller 
	{
		get;
		set;
	}

    /// <summary>
    /// Returns true if the player is instantiated in the game scene.
    /// </summary>
    public bool IsInWorld
    {
        get
        {
            return (WorldTransform != null);
        }
    }

    /// <summary>
    /// Player's transform in the game world.
    /// Returns null if the player is not in the scene, check IsInWorld before using the value directly.
    /// </summary>
    public Transform WorldTransform
    {
        get;
        set;
    }

    /// <summary>
    /// The player's current health. 
    /// Used for physical dammage.
    /// </summary>
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            value = Mathf.Clamp(value, 0, MaxHealth);

            if (value <= 0)
            {
                // TODO: Game over
            }

            health = value;
        }
    }

    /// <summary>
    /// The player's maximum health. 
    /// Used for physical dammmage.
    /// </summary>
    public int MaxHealth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public int Warmth
    {
        get
        {
            return warmth;
        }
        set
        {
            value = Mathf.Clamp(value, 0, MaxWarmth);

            if (value <= 0)
            {
                // TODO: Decrease health?
            }

            warmth = value;
        }
    }

    /// <summary>
    /// The player's maximum warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public int MaxWarmth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public int Hunger
    {
        get
        {
            return hunger;
        }
        set
        {
            value = Mathf.Clamp(value, 0, MaxHunger);

            if (value <= 0)
            {
                // TODO: Decrease health?
            }

            hunger = value;
        }
    }

    /// <summary>
    /// The player's maximum hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public int MaxHunger
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the player's on-person intenvory.
    /// </summary>
    /// <value>The player's intenvory.</value>
    public Inventory Inventory
    {
    	get;
    	private set;
    }

    /// <summary>
    /// Gets or sets the equipped tool.
    /// </summary>
    /// <value>The equipped tool.</value>
    public string EquippedTool
    {
    	get;
    	set;
    }

    /// <summary>
    /// Gets a value indicating whether this instance has a tool equipped.
    /// </summary>
    /// <value><c>true</c> if this instance has tool; otherwise, <c>false</c>.</value>
    public bool HasTool
    {
    	get
    	{
    		return !EquippedTool.Equals("");
    	}
    }
}

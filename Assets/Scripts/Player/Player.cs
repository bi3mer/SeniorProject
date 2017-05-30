using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Types of possible player health statuses
/// </summary>
public enum PlayerHealthStatus
{
    None,
    FoodPoisoning,
    Pneumonia
};

public class Player
{
    private const int maxHealth = 100;
    private const int maxWarmth = 100;
    private const int maxHunger = 100;
    private const string playerInventoryName = "player";
    private const string inventoryFileName = "InventoryYaml.yml";
    private const int playerInventorySize = 20;
    private int health;
    private int warmth;
    private int hunger;
    private PlayerHealthStatus healthStatus;
    private Vector3 worldPosition;

    public float FoodPoisoningChance = 0.3f;

    /// <summary>
    /// Player constructor.
    /// Initializes all fields to full.
    /// </summary>
    public Player ()
    {
        ResetStatus();
    }

    public Player(PlayerInventory inventory)
    {
    	Inventory = inventory;
		Health = MaxHealth = maxHealth;
        Warmth = MaxWarmth = maxWarmth;
        Hunger = MaxHunger = maxHunger;

        HealthStatus = PlayerHealthStatus.None;

        // set the transform if in unity editor so scenes without a player will
        // still work
		this.WorldPosition = Vector3.zero;
    }

    /// <summary>
    /// Resets player to initial status.
    /// </summary>
    public void ResetStatus ()
    {
        Health = MaxHealth = maxHealth;
        Warmth = MaxWarmth = maxWarmth;
        Hunger = MaxHunger = maxHunger;

        HealthStatus = PlayerHealthStatus.None;

        Inventory = new PlayerInventory(playerInventoryName, inventoryFileName, playerInventorySize);

        // set the transform if in unity editor so scenes without a player will
        // still work
		this.WorldPosition = Vector3.zero;
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
    /// Gets the position of the player in the world
    /// </summary>
    /// <value>The world position.</value>
    public Vector3 WorldPosition
    {
    	get
    	{
    		if (IsInWorld)
    		{
    			return WorldTransform.position;
    		}
    		else
    		{
    			return worldPosition;
    		}
    	}
    	set
    	{
    		worldPosition = value;

            if (IsInWorld)
            {
                WorldTransform.position = value;
            }
    	}
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
        	health = Mathf.Clamp(value, 0, MaxHealth);

        	if(health <= 0)
        	{
        		Game.Instance.DeathManagerInstance.Death();
        	}

			if(Controller != null && Controller.WarmthUpdatedEvent != null)
        	{
        		Controller.HealthUpdatedEvent.Invoke();
        	}
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
        	warmth = Mathf.Clamp(value, 0, MaxWarmth);

        	if(Controller != null && Controller.WarmthUpdatedEvent != null)
        	{
				Controller.WarmthUpdatedEvent.Invoke();
			}
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
        	hunger = Mathf.Clamp(value, 0, MaxHunger);

			if(Controller != null && Controller.WarmthUpdatedEvent != null)
        	{
				Controller.HungerUpdatedEvent.Invoke();
			}
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
    public PlayerInventory Inventory
    {
    	get;
    	private set;
    }

    /// <summary>
    /// Controls the player's tools.
    /// </summary>
    public PlayerTools Toolbox
    {
        get;
        set;
    }

    /// <summary>
    /// The player's current health status.
    /// </summary>
    public PlayerHealthStatus HealthStatus
    {
        get
        {
        	return healthStatus;
        }
        set
        {
        	healthStatus = value;

        	// this is null when the game first starts
        	if(GuiInstanceManager.PlayerNotificationInstance != null)
        	{
	        	if(healthStatus == PlayerHealthStatus.FoodPoisoning)
	        	{
					GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.STOMACH);
	        	}
	        	else if(healthStatus == PlayerHealthStatus.Pneumonia)
	        	{
					GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.PNEUMONIA);
	        	}
	        	else if(healthStatus == PlayerHealthStatus.None)
	        	{
					GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.CURE);
	        	}
	        }
        }
    }
}

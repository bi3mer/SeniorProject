using UnityEngine;

public class HealthRateManager
{
	/// <summary>
	/// Gets the health delay.
	/// </summary>
	/// <value>The health delay.</value>
	public int HealthDelay
	{
		get 
		{
			int delay = 0;
			if (Game.Instance.PlayerInstance.Hunger == 0) 
			{
				delay += Game.Instance.PlayerInstance.Controller.StatSettings.ZeroHungerDelay;
			}
			else
			{
				delay = Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDelay;
			}
			if (Game.Instance.PlayerInstance.Warmth == 0) 
			{
				delay += Game.Instance.PlayerInstance.Controller.StatSettings.ZeroWarmthDelay;
			}
			else
			{
				delay = Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDelay;
			}

			return delay;
		}
	}

	/// <summary>
	/// The amount to change the Health by on each update.
	/// </summary>
	/// <value>The health amount.</value>
	public int HealthAmount 
	{
		get 
		{
			int healthAmount = 0;
			if (Game.Instance.PlayerInstance.Hunger == 0)  
			{
				healthAmount -= Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDecrease;
			}
			if (Game.Instance.PlayerInstance.Warmth == 0) 
			{
				healthAmount -= Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDecrease;
			}

			return healthAmount;
		}
	}

	/// <summary>
	/// Takes the fall damage.
	/// </summary>
	/// <param name="fallDamageAmount">Fall damage amount.</param>
	public void TakeFallDamage(int fallDamageAmount)
	{
		Game.Instance.PlayerInstance.Health = Mathf.Clamp (
			Game.Instance.PlayerInstance.Health - fallDamageAmount, 
			0, 
			Game.Instance.PlayerInstance.MaxHealth);
	}

	/// <summary>
	/// Uses the health energy.
	/// </summary>
	/// <param name="amountOfHealthUnitsAffected">Amount of health units affected.</param>
	public void AffectHealthByGivenAmount(int amountOfHealthUnitsAffected)
	{
		Game.Instance.PlayerInstance.Health = Game.Instance.PlayerInstance.Health = Mathf.Clamp (
			Game.Instance.PlayerInstance.Health + amountOfHealthUnitsAffected, 
			0, 
			Game.Instance.PlayerInstance.MaxHealth);
	}
}



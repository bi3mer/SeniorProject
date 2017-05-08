using UnityEngine;

public class HungerRateManager
{
	/// <summary>
	/// Gets the hunger amount.
	/// </summary>
	/// <value>The hunger amount.</value>
	public int HungerAmount
	{
		get 
		{
			if (Game.Instance.PlayerInstance.HealthStatus == PlayerHealthStatus.FoodPoisoning) 
			{
				return -Game.Instance.PlayerInstance.Controller.StatSettings.FoodPoisonDecrease;
			} 
			else 
			{
				return -Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHungerDecrease;
			}
		}
	}

	/// <summary>
	/// Gets the hunger delay.
	/// </summary>
	/// <value>The hunger delay.</value>
	public int HungerDelay
	{
		get 
		{
			if (Game.Instance.PlayerInstance.HealthStatus == PlayerHealthStatus.FoodPoisoning) 
			{
				return Game.Instance.PlayerInstance.Controller.StatSettings.FoodPoisonDelay;
			} 
			else 
			{
				return Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHungerDelay;
			}
		}
	}

	/// <summary>
	/// Uses the food energy.
	/// </summary>
	/// <param name="amountOfHungerUnitsAffected">Amount of hunger units affected.</param>
	public void UseFoodEnergy(int amountOfHungerUnitsAffected)
	{
		Game.Instance.PlayerInstance.Hunger += amountOfHungerUnitsAffected;
	}
}



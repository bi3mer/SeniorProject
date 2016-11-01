using UnityEngine;
using System.Collections;

// TODO: Find a way to add a pause functionality to this. For now
//       this update is out of scope. But this should be figured
//       out before the alpha is released. It may be that a second
//       by second update will be required instead of this every 30
//       second option.
public class Clock : MonoBehaviour
{
	/// <summary>
	/// 30 seconds make up half an hour in game time
	/// </summary>
	public const int HalfAnHour = 30;

	/// <summary>
	/// Twenty four hours in game time
	/// </summary>
	public const int TwentyFourHours = 1440;

	public const float HourDivisor = 60.0f;

	private int currentTime = 0;
	public int CurrentTime
	{
		get
		{
			return this.currentTime;
		}
	}

	public float CurrentGameTimeInHours
	{
		get
		{
			return this.currentTime / Clock.HourDivisor;
		}
	}

	private bool isHalfHour = true;

	// delegates for updates
	public delegate void HalfHourDelegateUpdate();
	public event HalfHourDelegateUpdate HalfHourUpdate;

	public delegate void HourDelegateUpdate();
	public event HourDelegateUpdate HourUpdate;

	/// <summary>
	/// Start the update time loop. 
	/// </summary>
	public void Start () 
	{
		StartCoroutine(this.updateTime());
	}

	/// <summary>
	/// Update delegates for every half hour and hour 
	/// long cycle.
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator updateTime()
	{
		while(true)
		{
			yield return new WaitForSeconds(Clock.HalfAnHour);

			// update time
			this.currentTime += Clock.HalfAnHour;
			if(this.currentTime == Clock.TwentyFourHours)
			{
				this.currentTime = 0;
			}
				
			// notify hour long subscribed delegates
			if(!this.isHalfHour && this.HourUpdate != null)
			{
				this.HourUpdate();
			}

			// Notify thirty minute long subscribed delegates
			if(this.HalfHourUpdate != null)
			{
				this.HalfHourUpdate();
			}

			// update half hour boolean
			this.isHalfHour = !this.isHalfHour;
		}
	}

	// TODO: the subscribed delegates will probably have to be 
	//       notified for both of these functions. So it's looking
	//       like the clock will have a boolean for if its running 
	//       or not. On each delegate call, the delegates will check
	//       if the clock is running. If it is not, they'll know to 
	//       shutdown until they are told otherwise by the same method.
	/// <summary>
	/// Pause the clock running.
	/// </summary>
	public void Pause()
	{
		// to be implemented
	}

	/// <summary>
	/// Resume the clock running.
	/// </summary>
	public void Resume()
	{
		// to be implemented
	}
}

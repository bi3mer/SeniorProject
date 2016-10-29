public class Clock
{
	/// <summary>
	/// 30 seconds make up half an hour in game time
	/// </summary>
	public const int HalfAnHour = 30;

	/// <summary>
	/// Twenty four hours in game time
	/// </summary>
	public const int TwentyFourHours = 1440;

	private int currentTime = 0;
	public int CurrentTime
	{
		get
		{
			return this.currentTime;
		}
		set
		{
			// pass
		}
	}

	private bool isHalfHour = true;

	// delegates for updates
	public delegate void HalfHourDelegateUpdate();
	public event HalfHourDelegateUpdate HalfHourUpdate;

	public delegate void HourDelegateUpdate();
	public event HourDelegateUpdate HourUpdate;

	/// <summary>
	/// Create object to access default monobehavior scripts and
	/// Start the update time loop. 
	/// 
	/// Note: For this class to work DefaultMonoBehavior must be 
	///       in the scene.
	/// </summary>
	public void Start () 
	{
		DefaultMonoBehavior.Instance.StartCoroutine(this.updateTime());
	}

	/// <summary>
	/// Update delegates for every half hour and hour 
	/// long cycle.
	/// </summary>
	/// <returns>The time.</returns>
	private System.Collections.IEnumerator updateTime()
	{
		while(true)
		{
			yield return new UnityEngine.WaitForSeconds(Clock.HalfAnHour);

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
}

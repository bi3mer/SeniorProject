public class PauseSystem 
{
	// delegate pause and resume for updates
	public delegate void PauseDelegateUpdate();
	public event PauseDelegateUpdate PauseUpdate;
	public delegate void ResumeDelegateUpdate();
	public event ResumeDelegateUpdate ResumeUpdate;

	/// <summary>
	/// Gets a value indicating whether this game is paused.
	/// </summary>
	/// <value><c>true</c> if this instance is paused; otherwise, <c>false</c>.</value>
	public bool IsPaused
	{
		get;
		private set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PauseSystem"/> class.
	/// and starts the instance.
	/// </summary>
	public PauseSystem()
	{
		this.Resume();
	}

	/// <summary>
	/// Notify delegates to pause
	/// </summary>
	public void Pause()
	{
		this.IsPaused = true;
		if(this.PauseUpdate != null)
		{
			this.PauseUpdate();
		}
	}

	/// <summary>
	/// Notify delegates that game is resumed
	/// </summary>
	public void Resume()
	{
		this.IsPaused = false;
		if(this.ResumeUpdate != null)
		{
			this.ResumeUpdate();
		}
	}
}

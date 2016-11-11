public class PauseSystem 
{
	// delegate pause and resume for updates
	public delegate void PauseDelegateUpdate();
	public event PauseDelegateUpdate PauseUpdate;
	public delegate void ResumeDelegateUpdate();
	public event ResumeDelegateUpdate ResumeUpdate;

	public bool IsPaused
	{
		get;
		private set;
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

	/// <summary>
	/// Initializes a new instance of the <see cref="PauseSystem"/> class.
	/// and starts the instance.
	/// </summary>
	public PauseSystem()
	{
		this.Resume();
	}
}

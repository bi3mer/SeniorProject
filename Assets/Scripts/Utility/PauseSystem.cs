public class PauseSystem 
{
	// delegate pause and resume for updates
	public delegate void PauseDelegateUpdate();

	/// <summary>
	/// Occurs when game is paused.
	/// </summary>
	public event PauseDelegateUpdate PauseUpdate;
	public delegate void MenuPauseDelegateUpdate();

	/// <summary>
	/// Occurs when a menu is open.
	/// </summary>
	public event MenuPauseDelegateUpdate MenuPauseUpdate;
	public delegate void ResumeDelegateUpdate();

	/// <summary>
	/// Occurs when unpaused/menu closed update.
	/// </summary>
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
	///  Pause that occurs when a menu is open.
	/// </summary>
	public void MenuPause()
	{
		this.IsPaused = true;

		if(this.MenuPauseUpdate != null)
		{
			this.MenuPauseUpdate();
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

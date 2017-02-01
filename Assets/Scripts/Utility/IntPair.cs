/// <summary>
/// Literally here so I can have a way of having int pairs without converting back and forth from Vector2.
/// </summary>

public class IntPair
{
	public int X
	{
		get;
		set;
	}

	public int Y
	{
		get;
		set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IntPair"/> class.
	/// </summary>
	/// <param name="xVal">X value.</param>
	/// <param name="yVal">Y value.</param>
	public IntPair(int xVal, int yVal)
	{
		X = xVal;
		Y = yVal;
	}
}

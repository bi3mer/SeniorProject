/// <summary>
/// Literally here so I can have a way of having int pairs without converting back and forth from Vector2.
/// </summary>
public class Tuple<T1, T2>
{
	public T1 X
	{
		get;
		set;
	}

	public T2 Y
	{
		get;
		set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IntPair"/> class.
	/// </summary>
	/// <param name="xVal">X value.</param>
	/// <param name="yVal">Y value.</param>
	internal Tuple(T1 xVal, T2 yVal)
	{
		X = xVal;
		Y = yVal;
	}
}

public static class Tuple
{
	public static Tuple<T1, T2> New<T1, T2>(T1 xVal, T2 yVal)
	{
		var tuple = new Tuple<T1, T2>(xVal, yVal);
		return tuple;
	}
}

public class BooleanOperator 
{
	/// <summary>
	/// Boolean operator delegate.
	/// </summary>
	public delegate bool BooleanOperatorDelegate(float a, float b);

	/// <summary>
	/// Returns true if a is less than b.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool Less(float a, float b)
	{
		return a < b;
	}

	/// <summary>
	/// Returns true if a is less than or equal to b.
	/// </summary>
	/// <returns><c>true</c>, if or equal was lessed, <c>false</c> otherwise.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool LessOrEqual(float a, float b)
	{
		return a <= b;
	}

	/// <summary>
	/// Returns true if a is greater than b.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool Greater (float a, float b)
	{
		return a > b;
	}

	/// <summary>
	/// Returns true if a is greater than or equal to b.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool GreaterOrEqual(float a, float b)
	{
		return a >= b;
	}

	/// <summary>
	/// Returns true if a is not equal to b.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool NotEqual(float a, float b)
	{
		return a != b;
	}

	/// <summary>
	/// Returns true if a is equal to b.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool Equal(float a, float b)
	{
		return a == b;
	}
}

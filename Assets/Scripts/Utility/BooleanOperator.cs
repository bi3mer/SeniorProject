public class BooleanOperator 
{
	public delegate bool BooleanOperatorDelegate(float a, float b);

	public static bool Less(float a, float b)
	{
		return a < b;
	}

	public static bool LessOrEqual(float a, float b)
	{
		return a <= b;
	}

	public static bool More (float a, float b)
	{
		return a > b;
	}

	public static bool MoreOrEqual(float a, float b)
	{
		return a >= b;
	}

	public static bool NotEqual(float a, float b)
	{
		return a != b;
	}

	public static bool Equal(float a, float b)
	{
		return a = b;
	}
}

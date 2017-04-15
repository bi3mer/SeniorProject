public class ItemCondition
{
	BooleanOperator.BooleanOperatorDelegate booleanOperator; 

	/// <summary>
	/// Initializes a new instance of the <see cref="ItemCondition"/> class.
	/// </summary>
	/// <param name="attribute">Attribute.</param>
	/// <param name="threshold">Threshold.</param>
	/// <param name="boolOp">Bool op.</param>
	public ItemCondition(string attribute, float threshold, BooleanOperator.BooleanOperatorDelegate boolOp)
	{
		AttributeName = attribute;
		ThresholdValue = threshold;
		booleanOperator = boolOp;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ItemCondition"/> class.
	/// </summary>
	/// <param name="conditionVariable">Condition variable.</param>
	/// <param name="threshold">Threshold.</param>
	/// <param name="boolOp">Bool op.</param>
	public ItemCondition(float condition, float threshold, BooleanOperator.BooleanOperatorDelegate boolOp)
	{
		ConditionValue = condition;
		ThresholdValue = threshold;
		booleanOperator = boolOp;
	}

	/// <summary>
	/// Gets or sets the name of the attribute.
	/// </summary>
	/// <value>The name of the attribute.</value>
	public string AttributeName
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the threshold value.
	/// </summary>
	/// <value>The threshold value.</value>
	public float ThresholdValue
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the condition value.
	/// </summary>
	/// <value>The condition value.</value>
	public float ConditionValue
	{
		get;
		set;
	}

	/// <summary>
	/// Checks to see if the condition is met. Compares the value against the ThresholdValue according to the boolean operator.
	/// </summary>
	/// <returns><c>true</c>, if condition was checked, <c>false</c> otherwise.</returns>
	/// <param name="value">Value.</param>
	public bool CheckCondition(float value)
	{
        return booleanOperator (value, ThresholdValue);
	}

	/// <summary>
	/// Checks to see if the condition is met. Compares the ConditionValue to the ThresholdValue.
	/// </summary>
	/// <returns><c>true</c>, if condition was checked, <c>false</c> otherwise.</returns>
	public bool CheckCondition()
	{
		return booleanOperator(ConditionValue, ThresholdValue);
	}
}

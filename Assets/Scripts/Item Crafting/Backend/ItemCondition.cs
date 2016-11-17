public class ItemCondition
{
	private string attributeName;
	private float thresholdValue;
	BooleanOperator.BooleanOperatorDelegate booleanOperator; 

	public ItemCondition(string attribute, float threshold, BooleanOperator.BooleanOperatorDelegate boolOp)
	{
		attributeName = attribute;
		thresholdValue = threshold;
		booleanOperator = boolOp;
	}

	public string AttributeName
	{
		get
		{
			return attributeName;
		}
		set
		{
			attributeName = value;
		}
	}

	public float Threshold
	{
		get
		{
			return thresholdValue;
		}
		set
		{
			thresholdValue = value;
		}
	}

	public bool checkCondition(float value)
	{
		return booleanOperator (value, thresholdValue);
	}
}

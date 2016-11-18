public class Regression 
{
	/// <summary>
	/// Uses linear regression from the coefficients, powwers,
	/// and inputs to get a prediction. 
	/// 
	/// Note: the length of the powers on the inside should be
	///       size of the inputs array
	/// </summary>
	/// <returns>The regression prediction.</returns>
	/// <param name="coefficients">Coefficients.</param>
	/// <param name="powers">Powers.</param>
	/// <param name="inputs">Inputs.</param>
	/// <param name="intercept">Intercept.</param>
	public static float Prediction(float[] coefficients, int[,] powers, float[] inputs, float intercept)
	{
		float prediction = intercept;

		for(int i = 0; i < coefficients.Length; ++i)
		{
			if(coefficients[i] != 0)
			{
				float total = 1;

				for(int j = 0; j < inputs.Length; ++j)
				{
					total *= UnityEngine.Mathf.Pow(inputs[j], powers[i,j]);
				}

				prediction += total * coefficients[i];
			}
		}

		return prediction;
	}
}

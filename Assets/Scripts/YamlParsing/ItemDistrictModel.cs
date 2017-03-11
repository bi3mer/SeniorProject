using System.Collections.Generic;

public class ItemDistrictModel
{
	/// <summary>
	/// Gets or sets the name of the district.
	/// </summary>
	/// <value>The name of the district.</value>
	public string DistrictName
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the items that can appear on land in the district.
	/// </summary>
	/// <value>The items.</value>
	public List<string> LandItems
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets items that can appear on water in the district.
	/// </summary>
	/// <value>The water items.</value>
	public List<string> WaterItems
	{
		get;
		set;
	}
}

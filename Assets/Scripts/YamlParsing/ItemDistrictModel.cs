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
	/// Gets or sets the items in the district.
	/// </summary>
	/// <value>The items.</value>
	public List<string> Items
	{
		get;
		set;
	}
}

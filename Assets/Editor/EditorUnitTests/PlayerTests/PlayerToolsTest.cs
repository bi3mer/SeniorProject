using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PlayerToolsTest 
{
	[Test]
	public void ConstructsPlayerTools ()
	{
		PlayerTools tools = new PlayerTools(new PlayerTool[0]);

		Assert.IsNull(tools.EquippedTool);
		Assert.IsFalse(tools.HasEquipped);
	}
}

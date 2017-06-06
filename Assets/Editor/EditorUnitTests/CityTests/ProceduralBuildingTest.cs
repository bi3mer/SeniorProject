using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class ProceduralTest 
{
	[Test]
	public void ConstructsProceduralBuilding ()
	{
		GameObject gameObject = new UnityEngine.GameObject();
		Transform transform = gameObject.transform;
		int seed = 1234;
		int floors = 3;
		DistrictConfiguration config = new DistrictConfiguration();

		ProceduralBuilding building = new ProceduralBuilding(transform, Vector3.left, seed, floors, config);

		Assert.AreEqual(building.Parent, transform);
		Assert.AreEqual(building.Position, Vector3.left);
		Assert.AreEqual(building.NumberOfFloors, floors);
		Assert.AreEqual(building.Configuration, config);
		Assert.AreEqual(building.Seed, seed);
		Assert.AreEqual(building.IsLoaded, false);
		Assert.NotNull(building.Attachments);
		Assert.IsNull(building.Instance);
	}

	[Test]
	public void LoadsProceduralBuilding ()
	{
		GameObject gameObject = new UnityEngine.GameObject();
		Transform transform = gameObject.transform;
		int seed = 1234;
		int floors = 3;
		DistrictConfiguration config = new DistrictConfiguration();

		ProceduralBuilding building = new ProceduralBuilding(transform, Vector3.left, seed, floors, config);
		ProceduralBuilding.Generator = new ProceduralBuildingCreator();
	}
}

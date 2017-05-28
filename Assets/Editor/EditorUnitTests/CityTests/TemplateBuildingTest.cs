using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TemplateBuildingTest 
{
	[Test]
	public void ConstructsTemplateBuilding ()
	{
		GameObject gameObject = new UnityEngine.GameObject();
		Transform transform = gameObject.transform;

		TemplateBuilding building = new TemplateBuilding(transform, Vector3.left, gameObject);

		Assert.AreEqual(building.Parent, transform);
		Assert.AreEqual(building.Position, Vector3.left);
		Assert.AreEqual(building.Template, gameObject);
		Assert.AreEqual(building.IsLoaded, false);
		Assert.NotNull(building.Attachments);
		Assert.IsNull(building.Instance);
	}

	[Test]
	public void LoadsTemplateBuilding ()
	{
		GameObject gameObject = new GameObject();
		Transform transform = gameObject.transform;

		TemplateBuilding building = new TemplateBuilding(transform, Vector3.left, gameObject);

		building.Load();

		Assert.AreEqual(building.IsLoaded, true);
		Assert.IsNotNull(building.Instance);
		Assert.AreEqual(building.Instance.transform.position, Vector3.left);
		Assert.AreEqual(building.Instance.transform.parent, transform);
	}

	[Test]
	public void CalculatesBounds ()
	{
		GameObject gameObject = new GameObject();
		Transform transform = gameObject.transform;

		TemplateBuilding building = new TemplateBuilding(transform, Vector3.left, gameObject);

		building.Load();

		Assert.AreNotEqual(building.BoundingBox, new Bounds());
	}
}
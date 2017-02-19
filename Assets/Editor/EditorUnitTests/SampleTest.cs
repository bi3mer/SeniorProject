using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class SampleTest 
{
	[Test]
	public void SimpleTest()
	{
		//Arrange
		string s1 = "string s1";

		//Act
		// change to s2
		string s2 = "string s2";
		s1 = s2;

		//Assert
		Assert.AreEqual(s2, s1);
	}
}

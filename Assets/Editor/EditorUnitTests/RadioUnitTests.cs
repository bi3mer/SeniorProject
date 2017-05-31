using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RadioUnitTests 
{
	[Test]
	public void ChangeChannelTest()
	{
		GameObject o = new GameObject();
		o.AddComponent<Radio> ();

		Radio r = o.GetComponent<Radio> ();

		r.lowMusic = 0.0f;
		r.highMusic = 150.0f;
		r.ChangeChannel (100.0f);

		Assert.AreEqual (r.CurrentChannel, RadioChannel.Music);
	}

	[Test]
	public void PowerTest()
	{
		GameObject o = new GameObject();
		o.AddComponent<Radio> ();

		Radio r = o.GetComponent<Radio> ();

		Assert.IsFalse (r.IsOn);

		r.Power ();

		Assert.IsTrue (r.IsOn);
	}

	[Test]
	public void StaticTest()
	{
		GameObject o = new GameObject();
		o.AddComponent<Radio> ();

		Radio r = o.GetComponent<Radio> ();

		Assert.IsFalse (r.StaticOverlayOn);
	}

	[Test]
	public void ChannelClickTest()
	{
		GameObject o = new GameObject();
		o.AddComponent<Radio> ();

		Radio r = o.GetComponent<Radio> ();

		r.SetChannel (RadioChannel.Mystery);
		r.OnChannelClick ();

		Assert.AreEqual (RadioChannel.Music, r.CurrentChannel);
	}

	[Test]
	public void AnnouncementTest()
	{
		GameObject o = new GameObject();
		o.AddComponent<Radio> ();

		Radio r = o.GetComponent<Radio> ();
		string announcement = r.GetWeatherAnnouncement (10f, 75f);

		bool containsSpeed = announcement.Contains ("10");
		Assert.IsTrue(containsSpeed);

		bool containsTemp = announcement.Contains ("75");
		Assert.IsTrue(containsTemp);
	}
}
using UnityEngine;
using System.Collections;

public class WhaleAgent : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The speed that the whale moves")]
	private float whaleSpeed = 1f;

	/// <summary>
	/// One of two targets that the whale will go back and forth between
	/// </summary>
	private Vector2 targetOne;

	/// <summary>
	/// Second of two targets that the whale will go back and forth between
	/// </summary>
	private Vector2 targetTwo;

	/// <summary>
	/// The current target, either targetOne or targetTwo, that the whale
	/// is currently move to
	/// </summary>
	private Vector2 target;

	/// <summary>
	/// Updates the targets.
	/// </summary>
	private void updateTargets(Vector2 position)
	{
		// check if the distance is low between the two vectors
		if(Vector2.SqrMagnitude(position - this.target) < Mathf.Epsilon)
		{
			if(this.target.Equals(this.targetOne))
			{
				this.target = this.targetTwo;
			}
			else
			{
				this.target = this.targetOne;
			}
		}
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		// initialize 3d targets
		Vector3 firstTarget;
		Vector3 secondTarget;

		// find targets
		if(this.transform.position == Game.Instance.CityBounds.CityBounds.min)
		{
			firstTarget  = Game.Instance.CityBounds.CityBounds.max;
			secondTarget = Game.Instance.CityBounds.CityBounds.min;
		}
		else
		{
			firstTarget  = Game.Instance.CityBounds.CityBounds.min;
			secondTarget = Game.Instance.CityBounds.CityBounds.max;
		}

		// convert targets to xz coordinates so we can ignore the height of them
		this.targetOne = VectorUtility.XZ(firstTarget);
		this.targetTwo = VectorUtility.XZ(secondTarget);

		// set the target for the whale
		this.target    = this.targetTwo;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		// calculate where the whale should move towards
		float step = this.whaleSpeed * Time.deltaTime;
		Vector2 newPosition = Vector2.MoveTowards(VectorUtility.XZ(this.transform.position), this.target, step);

		// update the position with the current y value for the coordinate
		this.transform.position = VectorUtility.twoDimensional3d(newPosition, this.transform.position.y);

		// update where the whale is looking at
		this.transform.LookAt(VectorUtility.twoDimensional3d(-this.target, this.transform.position.y));

		// we may have to switch targets since we reached the current one
		this.updateTargets(newPosition);
	}
}

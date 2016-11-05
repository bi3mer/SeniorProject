using UnityEngine;
using System.Collections;

public class doorAnim : MonoBehaviour
{
	// Don't make the mistake of taking this as an example of proper C#
	// Scripting. It's functional, but rough around the edges, like a
	// lot of artist-generated scripts.

	public string door0;
	public string door1;
	public float door_width;
	public Vector3 openDir;
	public float speed = 1;

	private Vector3 d0openPos;
	private Vector3 d0closedPos;
	private Vector3 d1openPos;
	private Vector3 d1closedPos;
	
	private GameObject myRoot;
	private GameObject door0Object;
	private GameObject door1Object;

	// This IEnumerator is a coroutine that supposedly opens and closes
	// friggin' doors.
	IEnumerator MoveTo( GameObject go, Vector3 destPos )
	{
		float t = 0;
		float rate = speed;
			
		while ( t < 1f && go.transform.position != destPos )
		{
			yield return null;
			t += Time.deltaTime * rate;
			go.transform.position = Vector3.Lerp( go.transform.position, destPos, t );
		}

		// This would also be a very good time to play any door-related
		// audio clips you might have.
	}

	void Start()
	{
		// Set up asset root and doors.
		// This is kind of a hack. Any time you start using slashes in
		// quotes, you either haven't found the proper method to access
		// what you need or there isn't one and you just have to deal
		// with it.
		myRoot = transform.parent.parent.gameObject;

		// GameObject.Find() can be expensive, so use it sparingly.  This is a set-once operation,
		// so doing it in the Start() block is OK.
		door0Object = GameObject.Find( myRoot.name + "/" + door0 );
		door1Object = GameObject.Find( myRoot.name + "/" + door1 );
		
		// Set the open and closed positions...
		d0closedPos = door0Object.transform.position;
		d0openPos =
			d0closedPos + ( transform.rotation * Vector3.right * door_width * transform.root.localScale.x );
		d1closedPos = door1Object.transform.position;
		d1openPos =
			d1closedPos + ( transform.rotation * Vector3.left * door_width * transform.root.localScale.x );
	}

	void Update()
	{

	}

	// Coroutines are still kind of a mystery to me, but the upshot is
	// that they let you execute functions independently of the
	// time- and frame-based update functions. It's weird. There's no
	// such thing as a StopCoroutine() either.
	public void Open()
	{
		StartCoroutine( MoveTo( door0Object, d0openPos ) );
		StartCoroutine( MoveTo( door1Object, d1openPos ) ); 
	}

	public void Close() {
		StartCoroutine( MoveTo( door0Object, d0closedPos ) );
		StartCoroutine( MoveTo( door1Object, d1closedPos ) ); 
	}
}

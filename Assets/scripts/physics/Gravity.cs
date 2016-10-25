using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	public static Vector3 up;
	public static Vector3 right;

	public static bool mirrored;
	public static int orientation;

	[HideInInspector]
	public Transform trans;
	public int start_orientation;
	public bool start_mirrored;

	static Vector3[] directions = { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
	Vector3 scale;

	int last_orientation;
	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform> ();
		scale = transform.localScale;
		Set_Gravity (start_orientation, start_mirrored);
		last_orientation = -1;
	}
	
	public static void Set_Gravity(int dir, bool mir){
		int up_num = (dir + 1) % 4;
		int right_num = dir % 4;

		orientation = right_num;
		mirrored = mir;
		up = directions [up_num];
		right = (mir ? -1 : 1) * directions [right_num];
	}

	void Set_Rotation(){
		trans.rotation = Quaternion.AngleAxis (90 * orientation, Vector3.forward);
		if (mirrored)
			transform.localScale = new Vector3 (-scale.x, scale.y, scale.z);
		else
			transform.localScale = scale;
	}

	void Update(){
		if(last_orientation != orientation){
			Set_Rotation ();
			last_orientation = orientation;
		}
		//transform.localScale += new Vector3 (.01f, .01f, .01f);
	}
}

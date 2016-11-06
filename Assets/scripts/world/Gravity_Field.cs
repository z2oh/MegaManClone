using UnityEngine;
using System.Collections;

public class Gravity_Field : MonoBehaviour {

    public int direction;
    public bool mirrored;
    public string target;

	void OnTriggerEnter2D(Collider2D other)
    {
        if(target == other.tag)
            Gravity.Set_Gravity(direction, mirrored);
    }
}

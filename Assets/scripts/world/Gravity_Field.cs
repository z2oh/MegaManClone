using UnityEngine;
using System.Collections;

public class Gravity_Field : MonoBehaviour {

    public int direction;
    public bool mirrored;

	void OnTriggerEnter2D(Collider2D other)
    {
        Gravity.Set_Gravity(direction, mirrored);
    }
}

using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bool shoot = Input.GetKeyDown(KeyCode.G);
        if (shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
        if (weapon != null)
            {
                weapon.Attack(false);
            }
        }
	}
}

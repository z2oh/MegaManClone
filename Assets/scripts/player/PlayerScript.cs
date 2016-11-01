using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("player");
    }
	
	// Update is called once per frame
	void Update () {
        bool shoot = Input.GetKeyDown(KeyCode.G);
        if (shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
        if (weapon != null)
            {
                weapon.Attack(player.GetComponent<Player_Movement>().Get_Forward());
            }
        }
	}
}

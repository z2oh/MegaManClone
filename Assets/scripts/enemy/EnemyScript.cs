using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    private WeaponScript weapon;
    private GameObject player;
	// Use this for initialization
	void Start () {
        weapon = GetComponent<WeaponScript>();
        player = GameObject.Find("player");
	}
	
	// Update is called once per frame
	void Update () {
	if (weapon != null && weapon.CanAttack)
        {
            //weapon.Attack(true, player.GetComponent<Player_Movement>().Get_Forward());
        }
	}
}

using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Transform bullet;

    void Shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
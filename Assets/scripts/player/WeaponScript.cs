using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    //public Transform bullet;
    public float shootingRate = 0.3f;
    private float shootCooldown;
    public GameObject bullet;
    void Awake()
    {

    }
    void Start()
    {
        shootCooldown = 0f;
    }
    void Update()
{

        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }
    public void Attack( Vector3 vel)// removed bool isEnemy
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;
           GameObject shotTransform = Instantiate(bullet);
            shotTransform.GetComponent<MoveScript>().Fire(vel);
            //shotTransform.position = transform.position;
            //ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
           /* if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = this.transform.right;
            }*/
        }
    }
    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }


































}
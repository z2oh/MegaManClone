using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    // The maximum health that the entity can have.
    public int max_health = 3;
    // The health that the entity starts with.
    public int start_health = 3;
    // If true, the object is destroyed when health drops below 0.
    public bool simple_death = true;
    // The amount of time that the entity is invincible upon being hit (in ms).
    public float invincibility_time_on_hit = 100;

    // The current health of the entity.
    int current_health;
    // The time that the entity last had health removed.
    float time_of_last_hit;

    public void Start()
    {
        current_health = start_health;
        // This ensures that the entity is invincible on spawn. This is likely to change.
        time_of_last_hit = -invincibility_time_on_hit;
    }

    // Removes 'amount' from 'current_health'. If 'simple_death' is true, the game object is destroyed when 'current_health' drops below 0.
    public void RemoveHealth(int amount)
    {
        print("removeHealth");
        // If we are passed the invincibility frames.
        if(Time.time > time_of_last_hit + invincibility_time_on_hit)
        {
            time_of_last_hit = Time.time;
            current_health -= amount;
        }
        if(current_health <= 0 && simple_death)
        {
            Destroy(gameObject);
        }
    }

    // Adds 'amount' to 'current_health' and then clamps it to 'max_health'.
    public void AddHealth(int amount)
    {
        current_health = Mathf.Max(current_health + amount, max_health);
    }
}

using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	public int damage = 1;
	public string tag = "";

	void OnTriggerEnter2D(Collider2D other)
	{
		if(tag != "" && other.tag != tag)
		{
			return;
		}
		var health_script = other.gameObject.GetComponent<Health>();
		if(health_script)
		{
			health_script.RemoveHealth(damage);
		}
	}
}

using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {

	public float speedRotate = 2;

	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (new Vector3(0, speedRotate, 0));
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.transform.tag == "Player")
		{
			if(other.GetComponent<PlayerControl>().enabled)
				PickUp (other.gameObject);
			Destroy (gameObject);
		}
	}

	void DestroyMe()
	{
		Destroy (gameObject);
	}

	public abstract void PickUp (GameObject character);
}

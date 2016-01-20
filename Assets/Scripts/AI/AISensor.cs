using UnityEngine;
using System.Collections;

public class AISensor : MonoBehaviour {

	private AI ai = null;
	public Direction sensor;
	public string objectName = "";

	// Use this for initialization
	void Start () {
		ai = transform.parent.gameObject.GetComponent<AI>();
	}

	void OnTriggerStay(Collider other){
		if(other.tag != "Player" && other.tag != "Invisible" && ai != null){
			if(sensor == Direction.forward && !ai.forward)
				ai.forward = true;
			else if(sensor == Direction.left && !ai.left)
				ai.left = true;
			else if(sensor == Direction.right && !ai.right)
				ai.right = true;
			objectName = other.gameObject.name;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.name == objectName){
			if(sensor == Direction.forward)
				ai.forward = false;
			else if(sensor == Direction.left)
				ai.left = false;
			else if(sensor == Direction.right)
				ai.right = false;
			objectName = "";
		}
	}

	void DestroyMe(){}
}
using UnityEngine;
using System.Collections;

public class LoadResources : MonoBehaviour {

	public ResourcesName type;
	private string path = "Objects/";
	private int myQueue;

	// Use this for initialization
	void Start () {
		myQueue = LoadStage.AddQueue ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (myQueue == LoadStage.queuing)
			Built();
	}

	void Built(){
		GameObject instance;
		if(type == ResourcesName.Stone)
			instance = (GameObject)Instantiate(Resources.Load(path+"Stone"),transform.position,transform.localRotation);
		else //if(type == ResourcesName.Wood)
			instance = (GameObject)Instantiate(Resources.Load(path+"Wood"),transform.position,transform.localRotation);
		instance.transform.parent = transform.parent;
		
		//fin Built add point to Load...
		LoadStage.NextQueue ();
		Destroy (gameObject);
	}
}
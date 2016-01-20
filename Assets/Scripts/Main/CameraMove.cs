using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	private Transform script;
	private Vector3 pos;// rote;
	private Quaternion rot;

	private Vector3 Began = Vector3.zero;

	// Use this for initialization
	void Start () {
		script = GameObject.Find("Scripts").transform;
		transform.parent = script.parent;
		pos = transform.position;// + new Vector3(0,8,0);
		pos.y = 8;
		rot = new Quaternion(0.7f,0f,0f,0.7f);
		//rote = new Vector3(90,0,0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position.y < 7.9f) {
			transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, rot,Time.deltaTime*1.3f);
			//transform.Rotate(rote*Time.deltaTime/2);
		}else {
			if(Input.touchCount > 0) {
				if(Input.GetTouch(0).phase == TouchPhase.Began) {
					Began = Input.GetTouch(0).position;
				}else if(Input.GetTouch(0).phase == TouchPhase.Moved) {
					Vector3 pos = Vector3.zero;
					pos.x =	Began.x - Input.GetTouch(0).position.x;
					pos.z = Began.y - Input.GetTouch(0).position.y;
					transform.position += pos*0.01f;
					Began = Input.GetTouch(0).position;
				}else if(Input.GetTouch(0).phase == TouchPhase.Ended) {

				}
			}else {
				if(Input.GetMouseButtonDown(0)) {
					Began = Input.mousePosition;
				}else if(Input.GetMouseButton(0)) {
					Vector3 pos = Vector3.zero;
					pos.x = Began.x - Input.mousePosition.x;
					pos.z = Began.y - Input.mousePosition.y;
					transform.position += pos*0.01f;
					Began = Input.mousePosition;
				}else if(Input.GetMouseButtonUp(0)) {

				}
			}
		}
	}
}

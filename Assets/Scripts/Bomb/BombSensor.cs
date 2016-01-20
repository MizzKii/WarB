using UnityEngine;
using System.Collections;

public class BombSensor : MonoBehaviour {

	public Compass dir;
	public float speed = 2;
	
	private int endPoint = 1,nextFx;
	private Vector3 pos = Vector3.zero;
	private Fire fire;

	public GameObject fx;

	// Use this for initialization
	void Start () {
		if(transform.parent.GetComponent<Fire>() != null){
			fire = transform.parent.GetComponent<Fire>();
			endPoint = fire.getArea();
		}
		if(dir == Compass.east){
			endPoint = (int)Mathf.Round(transform.position.x) + endPoint;
			nextFx = (int)Mathf.Round(transform.position.x) + 1;
			pos = new Vector3(speed,0,0);
		}else if(dir == Compass.north){
			endPoint = (int)Mathf.Round(transform.position.z) + endPoint;
			nextFx = (int)Mathf.Round(transform.position.z) + 1;
			pos = new Vector3(0,0,speed);
		}else if(dir == Compass.south){
			endPoint = (int)Mathf.Round(transform.position.z) - endPoint;
			nextFx = (int)Mathf.Round(transform.position.z) - 1;
			pos = new Vector3(0,0,-speed);
		}else if(dir == Compass.west){
			endPoint = (int)Mathf.Round(transform.position.x) - endPoint;
			nextFx = (int)Mathf.Round(transform.position.x) - 1;
			pos = new Vector3(-speed,0,0);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		transform.position += pos * Time.deltaTime;

		if(dir == Compass.east){
			if(transform.position.x > nextFx - 0.4f){
				Instantiate(fx,new Vector3(nextFx,0,transform.position.z),fx.transform.rotation);
				nextFx++;
			}
			if(transform.position.x > endPoint){
				Destroy(gameObject);
			}
		}else if(dir == Compass.north){
			if(transform.position.z > nextFx - 0.4f){
				Instantiate(fx,new Vector3(transform.position.x,0,nextFx),fx.transform.rotation);
				nextFx++;
			}
			if(transform.position.z > endPoint){
				Destroy(gameObject);
			}
		}else if(dir == Compass.south){
			if(transform.position.z < nextFx + 0.4f){
				Instantiate(fx,new Vector3(transform.position.x,0,nextFx),fx.transform.rotation);
				nextFx--;
			}
			if(transform.position.z < endPoint){
				Destroy(gameObject);
			}
		}else if(dir == Compass.west){
			if(transform.position.x < nextFx + 0.4f){
				Instantiate(fx,new Vector3(nextFx,0,transform.position.z),fx.transform.rotation);
				nextFx--;
			}
			if(transform.position.x < endPoint){
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player" || other.tag == "Enemy"){
				other.SendMessage("BombMe", fire.getPlayer());
		}else {
			//Debug.LogWarning(other.tag);
			if(other.tag == "Bomb" || other.tag == "Box" || other.tag == "Invisible")
				other.SendMessage("DestroyMe");
			if(other.tag != "Invisible")
				Destroy(gameObject);
		}
	}
}

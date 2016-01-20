using UnityEngine;
using System.Collections;

public class CheckBound : MonoBehaviour {
	
	Compass compass;
	//bool check = false;
	int num = -1;
	public GameObject p;
	[SerializeField]private MainBomb mainBomb;
	Vector3 pos;

	void Start () {
		transform.parent.GetComponent<MainBomb>().checkBound = gameObject;
		gameObject.SetActive(false);
	}

	public void Reset(Vector3 pos, Compass compass)
	{
		num = 0;
		this.compass = compass;
		this.pos = pos;
		if(compass == Compass.north)
		{
			pos.z += 2;
		}
		else if(compass == Compass.south)
		{
			pos.z -= 2;
		}
		else if(compass == Compass.east)
		{
			pos.x += 2;
		}
		else if(compass == Compass.west)
		{
			pos.x -= 2;
		}

		transform.position = pos;
	}
	

	// Update is called once per frame
	void FixedUpdate () {

		if(num < 5 && num > -1)
		{
			num++;
		}
		else
		{
			int _num = 0;
			//Debug.LogWarning(transform.position);
			if(compass == Compass.north)
			{
				_num = Mathf.RoundToInt(transform.position.z - pos.z);
			}
			else if(compass == Compass.south)
			{
				_num = Mathf.RoundToInt(pos.z - transform.position.z);
			}
			else if(compass == Compass.east)
			{
				_num = Mathf.RoundToInt(transform.position.x - pos.x);
			}
			else if(compass == Compass.west)
			{
				_num = Mathf.RoundToInt(pos.x - transform.position.x);
			}
			num = -1;
			gameObject.SetActive(false);
			mainBomb.CalBomb(_num,compass);
		}
	}

	void OnTriggerStay(Collider other)
	{

		//if(other.collider.bounds.Contains(transform.position))
		//{
		if(num > -1)
			if(other.tag != "Enemy" && other.tag != "Player" && other.tag != "Invisible" && other.tag != "Bomb")
			{
				Vector3 pos = transform.position;

				if(compass == Compass.north)
				{
					pos.z++;
				}
				else if(compass == Compass.south)
				{
					pos.z--;
				}
				else if(compass == Compass.east)
				{
					pos.x++;
				}
				else if(compass == Compass.west)
				{
					pos.x--;
				}
				num = 0;

				transform.position = pos;
				//Debug.Log ("Hit : " + other);
			}
		//}
	}

	public int Num()
	{
		return num;
	}
}

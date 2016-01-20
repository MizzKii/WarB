using UnityEngine;
using System.Collections;

public class PlayerCommand : MonoBehaviour {
	
	private Direction myDir = Direction.none;
	public Compass myCompass {get; private set;}

	private int nextHop = 0;
	private int thisHop = 0;
	[SerializeField]private float speed = 3;
	[SerializeField]private float speedRotate = 3;

	private Vector3 move = Vector3.zero;

	public bool idle{get; private set;}

	void Start()
	{
		idle = true;
		CheckCompass();
		GameObject.Find("Scripts").GetComponent<Setting>().AddAudioSource(audio);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(!idle) {
			if((int)myDir > 1)
				Turning();
			else if((int)myDir != 0)
				Moving();
		}
	}

	public void addSpeed() {
		if(GamePlay.IsSingle) {
			setSpeed(this.speed + 1);
		}else {
			networkView.RPC("setSpeed",RPCMode.All,this.speed+1);
		}
	}

	[RPC]private void setSpeed(float num) {
		if(num < 7 && num > 0) {
			this.speed = num;
			this.speedRotate = num;
		}
	}

	//Move to next hop
	private void Moving(){
		//move to next hop
		transform.position += (int)myDir * move * Time.deltaTime * speed;

		//If moved to next hop
		bool fin = false;
		if(myCompass == Compass.east)
			if(myDir == Direction.forward){
				if(transform.position.x >= nextHop)
					fin = true;
			}else{
				if(transform.position.x <= nextHop)
					fin = true;
			}
		else if(myCompass == Compass.north)
			if(myDir == Direction.forward){
				if(transform.position.z >= nextHop)
					fin = true;
			}else{
				if(transform.position.z <= nextHop)
					fin = true;
			}
		else if(myCompass == Compass.south)
			if(myDir == Direction.forward){
				if(transform.position.z <= nextHop)
					fin = true;
			}else{
				if(transform.position.z >= nextHop)
					fin = true;
			}
		else if(myCompass == Compass.west)
		if(myDir == Direction.forward){
				if(transform.position.x <= nextHop)
					fin = true;
			}else{
				if(transform.position.x >= nextHop)
					fin = true;
			}

		if(fin){
			myDir = Direction.none;
			idle = true;
		}

	}

	//Turn to directions
	private void Turning(){
		//Turning to next dir
		if(myDir == Direction.left)
		{
			transform.Rotate(Vector3.down*speedRotate);
		}
		else if(myDir == Direction.right)
		{
			transform.Rotate(Vector3.up*speedRotate);
		}

		//If Turned
		if(transform.rotation.eulerAngles.y > (int)myCompass-speedRotate &&
		   transform.rotation.eulerAngles.y < (int)myCompass+speedRotate)
		{
			myDir = Direction.none;
			idle = true;
			//CheckCompass();
			//transform.rotation = Quaternion.Euler(0,(int)myCompass,0);
		}
	}

	//If next hop can't walk maybe it hit some box
	private void OnCollisionEnter(Collision collision) {
		Return();
	}

	public void Return()
	{
		if(myDir == Direction.back || myDir == Direction.forward )
		{
			nextHop = thisHop;
			if(myDir == Direction.forward)
				myDir = Direction.back;
			else
				myDir = Direction.forward;
		}
	}

	private void CheckCompass()
	{
		//If Turned
		if(transform.rotation.eulerAngles.y < (int)Compass.west+speedRotate && 
		   transform.rotation.eulerAngles.y > (int)Compass.west-speedRotate)
		{
			myCompass = Compass.west;
		}
		else if(transform.rotation.eulerAngles.y < (int)Compass.south+speedRotate && 
		        transform.rotation.eulerAngles.y > (int)Compass.south-speedRotate)
		{
			myCompass = Compass.south;
		}
		else if(transform.rotation.eulerAngles.y < (int)Compass.east+speedRotate  &&
		        transform.rotation.eulerAngles.y > (int)Compass.east-speedRotate)
		{
			myCompass = Compass.east;
		}
		else if((transform.rotation.eulerAngles.y < (int)Compass.north+speedRotate  && transform.rotation.eulerAngles.y  > 360-speedRotate) ||
		        (transform.rotation.eulerAngles.y < (int)Compass.north+speedRotate  && transform.rotation.eulerAngles.y  > (int)Compass.north-speedRotate))
		{
			myCompass = Compass.north;
		}
	}

	// // // // // // // // // // // // //
//	public void Movement (Direction dir)
//	{
//		if(GamePlay.IsSingle)
//		{
//			setMovement(dir);
//		}
//		else
//		{
//			networkView.RPC("IntToDirection",RPCMode.All,(int)dir);
//		}
//	}
//
//	//RPC
//	[RPC]
//	void IntToDirection (int dir)
//	{
//		setMovement((Direction)dir);
//	}
//
//	void setMovement(Direction dir)
	public void Movement (Direction dir)
	{
		if(myDir == Direction.none)
		{
			if((int)dir < 2)
			{
//				//set this hop
//				if(myCompass == Compass.east || myCompass == Compass.west)
//					thisHop = (int)Mathf.Round (transform.position.x);
//				else
//					thisHop = (int)Mathf.Round (transform.position.z);
//			
//				//set next hop
//				if(myCompass == Compass.east){
//					nextHop = (int)Mathf.Round(transform.position.x) + (int)dir;
//					move = Vector3.right;
//				}else if(myCompass == Compass.north){
//					nextHop = (int)Mathf.Round(transform.position.z) + (int)dir;
//					move = Vector3.forward;
//				}else if(myCompass == Compass.south){
//					nextHop = (int)Mathf.Round(transform.position.z) - (int)dir;
//					move = Vector3.back;
//				}else if(myCompass == Compass.west){
//					nextHop = (int)Mathf.Round(transform.position.x) - (int)dir;
//					move = Vector3.left;
//				}
				if(GamePlay.IsSingle)
					setMove((int)dir);
				else
					networkView.RPC("setMove",RPCMode.All,(int)dir);
			}
			else
			{
				Compass compass;
				//set new compass
				if (dir == Direction.left)
				{
					if(myCompass <= Compass.north)
						compass = Compass.west;
					else
						compass = (Compass)((int)myCompass -90);
				}
				else //if(dir == Direction.right)
				{
					if(myCompass >= Compass.west)
						compass = Compass.north;
					else
						compass = (Compass)(int)myCompass +90;
				}
				if(GamePlay.IsSingle)
					setTurn((int)dir,(int)compass);
				else
					networkView.RPC("setTurn",RPCMode.All,(int)dir,(int)compass);
			}
//			//Start
//			myDir = dir;
//			idle = false;
		}
	}

	[RPC]private void setMove(int dir) {
		//set this hop
		if(myCompass == Compass.east || myCompass == Compass.west)
			thisHop = (int)Mathf.Round (transform.position.x);
		else
			thisHop = (int)Mathf.Round (transform.position.z);
		
		//set next hop
		if(myCompass == Compass.east){
			nextHop = (int)Mathf.Round(transform.position.x) + dir;
			move = Vector3.right;
		}else if(myCompass == Compass.north){
			nextHop = (int)Mathf.Round(transform.position.z) + dir;
			move = Vector3.forward;
		}else if(myCompass == Compass.south){
			nextHop = (int)Mathf.Round(transform.position.z) - dir;
			move = Vector3.back;
		}else if(myCompass == Compass.west){
			nextHop = (int)Mathf.Round(transform.position.x) - dir;
			move = Vector3.left;
		}
		//Start
		myDir = (Direction)dir;
		idle = false;
	}

	[RPC]private void setTurn(int dir, int compass){
		myCompass = (Compass)compass;
		myDir = (Direction)dir;
		idle = false;
	}
}
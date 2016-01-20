using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	private int area = 1;
	private Player player;

	void Start() {
		GameObject.Find("Scripts").GetComponent<Setting>().AddAudioSource(audio);
	}

	public void setFire(int damage, Player player)
	{
		area = damage;
		this.player = player;
		for(int i=0;i<transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

//	public void setArea(int x){
//		area = x;
//		for(int i=0;i<transform.childCount;i++){
//			transform.GetChild(i).gameObject.SetActive(true);
//		}
//	}

	public int getArea(){
		return area;
	}

	public Player getPlayer () {
		return player;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(transform.childCount == 0) {
			GameObject.Find("Scripts").GetComponent<Setting>().RemoveAudioSource(audio);
			Destroy(gameObject);
		}
	}
}

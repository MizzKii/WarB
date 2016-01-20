using UnityEngine;
using System.Collections;

public class MainBomb : MonoBehaviour {
	
	public GameObject bomb, checkBound;
	private Vector3 curPos;
	private GameObject hit;
	
	public GameObject  bombThrow;
	private NetworkControl nc;

	void Start()
	{
		nc = GameObject.Find("Scripts").GetComponent<NetworkControl>();
	}

	public void Bomb ()
	{
		Vector3 pos = transform.position;
		pos.y = -0.3f;
		//Debug.Log(GamePlay.myPlayer.getName+" Bomb!");
		//nc.CreateBomb(bomb, pos, bomb.transform.rotation,GamePlay.myPlayer);
		GameObject b = nc.CreateGameObject(bomb,pos,bomb.transform.rotation);
		b.GetComponent<Bomb>().setPlayer(GamePlay.myPlayer);
	}

	public void CalBomb(int num, Compass compass)
	{
		Vector3 pos = new Vector3(transform.position.x, -0.3f, transform.position.z);
		GameObject bomb = nc.CreateGameObject(bombThrow, pos, transform.rotation);
		//GameObject bomb = Instantiate(bombThrow, new Vector3(transform.position.x, -0.3f, transform.position.z), transform.rotation) as GameObject;
		bomb.GetComponent<BombAnimation>().Play(num, compass, GamePlay.myPlayer);
	}
}

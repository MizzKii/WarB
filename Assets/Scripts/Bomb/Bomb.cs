using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	//public int damage = 1;
	[SerializeField]
	private float sec = 3;
	[SerializeField]
	private GameObject fire;
	private Player player;
	private int damage = 1;
	private float time = 0;

	public void setPlayer(Player player)
	{
		if(GamePlay.IsSingle) {
			this.player = player;
			this.damage = player.damageNum;
		} else
			networkView.RPC("setBombRPC", RPCMode.All, player.getNet,player.damageNum);
	}

	public void setPlayer(Player player, int damage)
	{
//		if(GamePlay.IsSingle) {
//			this.player = player;
//			this.damage = damage;
//		} else {
//			//networkView.RPC("setPlayerByNet", RPCMode.All, player.getNet,damage);
//		}
		this.player = player;
		this.damage = damage;
		Debug.Log(player.getName+":Damage "+damage+":"+transform.position);
	}

	[RPC]private void setBombRPC(NetworkPlayer net, int damage)
	{
		foreach (Player p in MultiPlayer.playerList)
		{
			if(p.getNet == net)
			{
				this.player = p;
				this.damage = damage;
				Debug.Log(p.getName+":Damage "+damage+":"+transform.position);
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		Invoke("Boom",sec);
	}

	void FixedUpdate () {
		time += 0.01f;
	}

	private void Boom(){
		collider.enabled = false;
		Vector3 pos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
		fire = (GameObject)Instantiate(fire, pos, fire.transform.rotation);
		fire.GetComponent<Fire>().setFire(damage,player);
		//fire.SendMessage("setFire",damage,player);
		if(player != null)
			player.bombNum++;
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Bomb") {
			other.gameObject.SendMessage("RemoveBomb",time);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player")
			collider.isTrigger = false;
	}
	
	public void DestroyMe(){
		Boom();
	}

	public void RemoveBomb(float time) {
		if(this.time < time)
			Boom ();
		else {
			//if(player != null)
				player.bombNum++;
			Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class BombAnimation: MonoBehaviour {
	
	public GameObject bomb;
	Compass compass;
	private Player player;
	[SerializeField]
	float x = +1.2f;
	private bool checkPlay = false;

	[SerializeField]
	private int num = 0, max = 0;

	private Vector3 target;
	private bool oneTime = false;
	private int damage = 1;

	public void Play (int getNum, Compass getCompass, Player player) 
	{
		if(GamePlay.IsSingle)
		{
			_Play(getNum,(int)getCompass, player.getNet, player.damageNum);
		}else{
			networkView.RPC("_Play",RPCMode.All,getNum,(int)getCompass, player.getNet, player.damageNum);
		}
	}
	[RPC]private void _Play (int getNum, int getCompass, NetworkPlayer net, int damage) {
		num = getNum;
		compass = (Compass)getCompass;
		checkPlay = true;
		target = transform.position;
		if(compass == Compass.north) target.z += num;
		else if(compass == Compass.east) target.x += num;
		else if(compass == Compass.west) target.x -= num;
		else target.z -= num;
		
		if(num == 2) oneTime = true;
		else max = num;
		foreach(Player p in MultiPlayer.playerList) {
			if(p.getNet == net){
				player = p;
				break;
			}
		}
		this.damage = damage;
	}
	
	void FixedUpdate()
	{
		if(checkPlay)
		{
			Vector3 pos = transform.position;
			if(compass == Compass.north)
			{
				pos.z += Time.deltaTime;
			}
			else if(compass == Compass.east)
			{
				pos.x += Time.deltaTime;
			}
			else if(compass == Compass.west)
			{
				pos.x -= Time.deltaTime;
			}
			else
			{
				pos.z -= Time.deltaTime;
			}

			if(num <= 0)
			{
				checkPlay = false;
				//GameObject b = (GameObject)Instantiate(bomb, transform.position, transform.rotation);
				//b.GetComponent<Bomb>().setPlayer(GamePlay.myPlayer);
				//NetworkControl nc = GameObject.Find("Scripts").GetComponent<NetworkControl>();
				//GameObject b = nc.CreateGameObject(bomb,pos,bomb.transform.rotation);
				//b.GetComponent<Bomb>().setPlayer(GamePlay.myPlayer);
				//b.collider.isTrigger = false;
				GameObject b = Instantiate(bomb,pos,bomb.transform.rotation) as GameObject;
				b.GetComponent<Bomb>().setPlayer(player,damage);
				b.collider.isTrigger = false;
				Destroy(gameObject);
			}
			else
			{
				if(oneTime)
				{
					num -= paraX2pZ(pos);
				}
				else if(num == max)
				{
					num -= paraX2p(pos);
				}
				else if(num == 1)
				{
					num -= paraX2p_3(pos);
				}
				else
				{
					num -= paraX2p_2(pos);
				}
			}
		}
	}

	int paraX2pZ(Vector3 pos)
	{
		if(x > -1.29f)
		{
			x -= Time.deltaTime*1.32f;
			pos.y = (-x*x) + 1.4f;
			transform.position = pos;
		}
		else
		{
			return 2;
		}
		return 0;
	}

	int paraX2p(Vector3 pos)
	{
		if(x > -1.185f)
		{
			x -= Time.deltaTime*1.24f;
			pos.y = (-x*x)/2 + 1.4f;
			transform.position = pos;
		}
		else
		{
			if(num-2 == 1) x = 0.3f;
			else x = 0.5f;
			return 2;
		}
		return 0;
	}

	int paraX2p_2(Vector3 pos)
	{
		if(x > -0.5f)
		{
			x -= Time.deltaTime*1.01f;
			pos.y = (-x*x) +0.95f;
			transform.position = pos;
		}
		else
		{
			if(num-1 == 1) x = 0.3f;
			else x = 0.5f;
			return 1;
		}
		return 0;
	}

	int paraX2p_3(Vector3 pos)
	{
		if(x > -0.605f)
		{
			x -= Time.deltaTime*0.92f;
			pos.y = (-x*x)/0.3f +0.96f;
			transform.position = pos;
		}
		else
		{
			return 1;
		}
		return 0;
	}
}

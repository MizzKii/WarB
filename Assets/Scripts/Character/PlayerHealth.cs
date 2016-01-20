using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private int playerNumber;
	[SerializeField]
	private int health = 1;
	private bool damage = false;
	private float time = 0;
	private float LimitTime = 3;
	private float swap = 0.5f;
	private bool checkPoint = false;
	private float a = 0;
	public Texture2D red;
	private GamePlay gamePlay = null;

	//private PlayerAnimation pa;
	private PlayerControl control;

	private void Start () {
		//pa = GetComponent<PlayerAnimation>();
		control = GetComponent<PlayerControl>();
	}

	public void setHealth(int health)
	{
		this.health = health;
	}

	public void BombMe(Player player){
		if(!damage && control.enabled)
		{
			damage = true;
			if(gamePlay == null)
				gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();
			gamePlay.AddLog(player.getName + " Bomb " + GamePlay.myPlayer.getName);
			if(--health < 1)
			{
				control.enabled = false;
				GetComponent<PlayerAnimation>().PlayAnime("IsDeath");
				//Debug.LogWarning(player.getNumber);
				if(player.getNumber != playerNumber){
					gamePlay.AddPoint(player, 200);
					gamePlay.AddLog(player.getName + " Get 200P");
				}
				Invoke("Death", 2);
			}
		}
	}

	public void DestroyMe(){
		if(!damage && control.enabled)
		{
			damage = true;
			if(gamePlay == null)
				gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();
			gamePlay.AddLog("Enemy Hit " + GamePlay.myPlayer.getName);
			if(--health < 1)
			{
				control.enabled = false;
				GetComponent<PlayerAnimation>().PlayAnime("IsDeath");
				Invoke("Death", 2f);
			}
		}
	}

	private void Death()
	{
		gamePlay.StartEndCamera(transform.position, transform.rotation);
		//gamePlay.RemovePlayer(MultiPlayer.playerList[playerNumber]);
		gamePlay.RemovePlayer(GamePlay.myPlayer);
		GameObject.Find("Controller").SetActive(false);
		GameObject.Find("Scripts").GetComponent<Setting>().RemoveAudioSource(audio);
		if(GamePlay.IsSingle)
			Destroy(gameObject);
		else
			Network.Destroy(gameObject);
			//networkView.RPC("Remove",RPCMode.All);
	}

//	[RPC]private void Remove () {
//		Destroy(gameObject);
//	}

	private void OnGUI()
	{
		if(damage)
		{
			Color _default = GUI.color;
			
			if(checkPoint)
				a -= Time.fixedDeltaTime;
			else
				a += Time.fixedDeltaTime;
			GUI.color = new Color(1,1,1,a);
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), red);
			if(!checkPoint && a > swap)
				checkPoint = true;
			else if(checkPoint && a < 0)
				checkPoint = false;

			time += Time.fixedDeltaTime;
			if(time > LimitTime)
			{
				damage = false;
				time = 0;
				a = 0;
			}

			GUI.color = _default;
		}
	}
}

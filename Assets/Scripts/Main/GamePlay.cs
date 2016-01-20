using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlay : MonoBehaviour {

	public Map map;
	public GameObject[] character;
	public Transform[] spawn;
	public AudioClip win;
	public GameObject EndCamera;
	public Log log;

	public static Player myPlayer {get; private set;}
	public static bool IsSingle {get; private set;}
	private int botCount;// playerCount = 0;
	private Result result;
	private int difficulty, bonus, hightScore;
	
	private int minute,second,limitTime;
	private int endPattern;
	private int[] bestTime = new int[2];

	private int maxPlayer = 0, playerCount = 0;

	void Awake()
	{
		Application.targetFrameRate = 30;
		difficulty = PlayerPrefs.GetInt("Difficulty",1);
		if(MultiPlayer.playerList.Count > 1) {
			IsSingle = false;
			maxPlayer = MultiPlayer.playerList.Count;
			playerCount = maxPlayer;
		}else
			IsSingle = true;
		botCount = 0;

		if (Network.isServer) {
			List<int> spawn = new List<int>();
			for(int i=0;i<4;i++)
				spawn.Add(i);
			foreach (Player p in MultiPlayer.playerList){
				int r = Random.Range(0,spawn.Count-1);
				networkView.RPC("setSpawn", RPCMode.All, p.getNet, spawn[r]);
				spawn.RemoveAt(r);
			}
		}
		//playerCount = 0;
	}

	[RPC] private void setSpawn(NetworkPlayer n, int s){ 
		foreach(Player p in MultiPlayer.playerList) {
			if(p.getNet==n){ 
				p.setSpawn(s); 
				if (p.getNet == Network.player){
					GameObject me = (GameObject)Network.Instantiate (character[p.getNumber], spawn[p.getSpawn].position, spawn[p.getSpawn].rotation,0);
					myPlayer = p;
					me.GetComponent <PlayerControl> ().enabled = true;
				}
				break; 
			} 
		}
	}

	// Use this for initialization
	void Start () {
		//backRL.SetActive (false);
		result = GetComponent<Result>();
		GameObject me;
		if(MultiPlayer.playerList.Count > 1)
			IsSingle = false;

		if(IsSingle)
		{
			if(MultiPlayer.playerList.Count < 1)
			{
				Application.LoadLevel(0);
				//MultiPlayer.playerList.Add(new Player("test"));
			}
			else
			{
				int index = Random.Range(0,spawn.Length-1);
				me = (GameObject)Instantiate(character[MultiPlayer.playerList[0].getNumber],
			                            	 spawn[index].position,
			                            	 spawn[index].rotation);
				//setCamera(me.transform);
				myPlayer = MultiPlayer.playerList[0];
				me.GetComponent <PlayerControl> ().enabled = true;
			}
		}
		bonus = 0;
		second = 0;
		minute = 0;
		endPattern = 0;
		if (difficulty == 3)
			limitTime = 10;
		else if(difficulty == 2)
			limitTime = 6;
		else
			limitTime = 4;
		//limitTime
		/*else
		{
			foreach (Player p in MultiPlayer.playerList)
			{
				if (p.getNet == Network.player)
				{
					Debug.LogWarning("char:"+p.getNumber+", Spawn:"+p.getSpawn);
					me = (GameObject)Network.Instantiate (character[p.getNumber],
				                     spawn[p.getSpawn].position,
				                     spawn[p.getSpawn].rotation,0);
					//setCamera(me.transform);
					myPlayer = p;
					me.GetComponent <PlayerControl> ().enabled = true;
					break;
				}
			}
		}*/
		Invoke("OneSecond",1f);
	}

	/*void setCamera(Transform camera)
	{
		myCamera.position = camera.position;
		myCamera.rotation = camera.rotation;
		myCamera.parent = camera;
		myCamera.gameObject.SetActive(true);
		camera.gameObject.GetComponent <PlayerControl> ().enabled = true;
	}*/

	public void StartEndCamera (Vector3 position, Quaternion rotation) 
	{
		EndCamera.SetActive(true);
		EndCamera.transform.position = position;
		EndCamera.transform.rotation = rotation;
	}

	public void AddPoint(Player player, int point)
	{
		if(IsSingle)
			player.point += point;
		else
			networkView.RPC("RPCAddPoint", RPCMode.All, player.getNet, point);
	}

	[RPC]private void RPCAddPoint(NetworkPlayer player, int point) 
	{
		foreach (Player p in MultiPlayer.playerList) {
			if(p.getNet == player) {
				p.point += point;
				break;
			}
		}
	}

	public void AddLog(string text)
	{
		if(IsSingle)
			log.AddLog(text);
		else
			networkView.RPC("RPCAddLog", RPCMode.All, text);
	}

	[RPC]private void RPCAddLog(string text)
	{
		log.AddLog(text);
	}

	public void AddBot()
	{
		botCount++;
	}

	public void RemoveBot()
	{
		if(--botCount == 0 && IsSingle)
		{
			endPattern = 1;
			audio.PlayOneShot(win);
			SaveScore();
		}
	}
	
	//////////////////////////////////////////////////////

	public void PlayerDisconnected(NetworkPlayer player) {
		RPCRemovePlayer(player);
		Network.DestroyPlayerObjects(player);
		foreach(Player p in MultiPlayer.playerList){
			if(p.getNet == player){
				AddLog(p.getName+"'s Disconnected.");
				break;
			}
		}
	}

	public void RemovePlayer(Player player)
	{
		if(Network.isServer)
			RPCRemovePlayer(myPlayer.getNet);
		else if(!IsSingle){
			networkView.RPC("RPCRemovePlayer",RPCMode.Server,myPlayer.getNet);
		}else {
			Debug.Log("Lose!!");
			endPattern = 2;
			string key = map+"_"+difficulty.ToString();
			print(key);
			hightScore = PlayerPrefs.GetInt(key,0);
			bestTime[0] = PlayerPrefs.GetInt(key+"_MT",0);
			bestTime[1] = PlayerPrefs.GetInt(key+"_ST",0);
		}
	}

	[RPC]
	private void RPCRemovePlayer(NetworkPlayer net)
	{
		Player winPlayer = null;
		int life = 0;
		foreach(Player p in MultiPlayer.playerList)
		{
			if(p.getNet == net)
			{
				p.IsDear = true;
				Debug.Log(p.getName+"'s Dead");
			}
			else if(!p.IsDear)
			{
				life++;
				winPlayer = p;
			}
		}
		networkView.RPC("setPlayerCount", RPCMode.All, life);
		if(life < 2)
		{
			networkView.RPC("setWinner",RPCMode.All,winPlayer.getNet);
			//setWinner(winPlayer.getNet);
		}
	}

	[RPC]private void setPlayerCount(int count) {
		this.playerCount = count;
	}
	
	[RPC]
	private void setWinner(NetworkPlayer winPlayer)
	{
		foreach(Player p in MultiPlayer.playerList)
		{
			if(p.getNet == winPlayer)
				SetRank(p);
		}
		endPattern = 1;
		MultiPlayer.state = GameState.EndGame;
		//Network.Disconnect();
		//MultiPlayer.playerList.Clear();
		if(myPlayer.getNet == winPlayer)
			audio.PlayOneShot(win);
	}

	public void Reset () {
		MultiPlayer.state = GameState.Index;
		MultiPlayer.playerList.Clear();
		Application.LoadLevel(0);
		if(!IsSingle) {
			Network.Disconnect();
		}
	}

	//////////////////////////////////////////////////////
	
	//private Player winner;
	public GUISkin skin;
	//private float time;
	void OnGUI ()
	{
		Resolution.Android();
		GUI.skin = skin;
		if(Time.timeScale == 0)
			GUI.color = new Color(.5f,.5f,.5f);
		if(endPattern > 0)
		{
			//CancelInvoke("OneSecond");
			if(IsSingle)
			{
				if(endPattern == 1)
					result.Win(bestTime[0]+":"+bestTime[1], minute+":"+second, hightScore,myPlayer.point, bonus, (int)map);
				else
					result.Lose(bestTime[0]+":"+bestTime[1], hightScore,(int)map);
			}
			else
			{
				result.Ranking();
				//backRL.SetActive (true);
				/*if(GUI.Button(new Rect(Resolution.width/2 - 100, Resolution.height/2 +70, 200, 50), "Back to Menu"))
				{
					Reset ();
				}*/
			}
		}else if(IsSingle){
			Color color = GUI.color;
			if(minute >= limitTime)
				GUI.color = Color.red;

			GUI.Label(new Rect(Resolution.width/2-100,10,200,50), "Time: "+minute+":"+second);
			GUI.Label(new Rect(Resolution.width/2-100,50,200,50), "Enemy: "+botCount+"/4");
//			time += Time.deltaTime/2;
//			if(time >= 1){
//				second++;
//				time = 0;
//				if(second >= 60)
//				{
//					minute++;
//					second = 0;
//				}
//			}
			GUI.color = color;
		}else {
			GUI.Label(new Rect(Resolution.width/2-100,10,200,50), "Player: "+playerCount+"/"+maxPlayer);
		}
	}

	private void OneSecond() {
		if(++second >= 60)
		{
			minute++;
			second = 0;
		}
		if(endPattern < 1) {
			Invoke("OneSecond",1f);
		}
	}

	private void SaveScore() {
		bool add = false;
		if(difficulty == 3 && minute < 10) 
			add = true;
		else if (difficulty == 2 && minute < 6)
			add = true;
		else if (minute < 4) 
			add = true;

		if(add) {
			limitTime--;
			bonus = 60 - second;
			for (int i=0; i < limitTime - minute; i++) {
				bonus +=60;
			}
		}else {
			bonus -= second;
			for (int i=0; i < minute - limitTime; i++) {
				bonus -=60;
			}
		}
		string key = map+"_"+difficulty.ToString();
		print(key);
		hightScore = PlayerPrefs.GetInt(key,0);
		if(myPlayer.point+bonus > hightScore) {
			hightScore = myPlayer.point+bonus;
			PlayerPrefs.SetInt(key,hightScore);
			PlayerPrefs.SetInt(key+"_MT",minute);
			PlayerPrefs.SetInt(key+"_ST",second);
		}
		bestTime[0] = PlayerPrefs.GetInt(key+"_MT",0);
		bestTime[1] = PlayerPrefs.GetInt(key+"_ST",0);
	}

	private void SetRank (Player winner) {
		result.rank = MultiPlayer.playerList;
		Player tmp = result.rank[0];
		int i = 0;
		foreach (Player p in result.rank) {
			if(p == winner) {
				result.rank[0] = p;
				result.rank[i] = tmp;
				break;
			}
			i++;
		} 
		if(result.rank.Count > 2) {
			for (i = 1; i < result.rank.Count; i++) {
				for(int j = i; j < result.rank.Count; j++) {
					if(result.rank[j].point > result.rank[i].point) {
						tmp = result.rank[i];
						result.rank[i] = result.rank[j];
						result.rank[j] = tmp;
					}
				}
			}
		}
	}

	/*public void BackRL()
	{
		//backRL.SetActive (false);
		Reset ();
	}*/
}
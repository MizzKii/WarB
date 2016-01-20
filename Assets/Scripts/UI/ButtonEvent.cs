using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
	//[SerializeField]private AudioSource soundSin;
	[SerializeField]private AudioClip click;
	public GameObject blackBG, sinMenu, mulMenu, mulCreate, lobby, lPlay, logLobby;
	// Index // + // How To //
	public Animator btSin, btMul, btHow, imageHow;
	// Single //
	[SerializeField]private Animator sinTask;
	// Multi //
	public Animator mulTask, createTask, lobbyTask;
	public Transform joinArea;
	private List<GameObject> joinTasks = new List<GameObject>();
	public GameObject joinTask;

	public Text difTxt, cPlayer;
	public Image sMap, cMap, lMap;
	public GameObject[] players;
	[SerializeField]private Text[] nPlayer;
	public InputField pName, hName, password;

	private int difficulty, map, count, playerCount = -1;
	[SerializeField]private Sprite[] maps;
	private MultiPlayer multiPlayer;
	private Chat chat;
	public GUISkin skin;
	private bool create = false;
	private int checkRe = 0;

	private void Awake () {
		sinMenu.SetActive(false);
		mulMenu.SetActive(false);
		mulCreate.SetActive(false);
		logLobby.SetActive (false);
		lobby.SetActive(false);
		blackBG.SetActive(false);
		difficulty = PlayerPrefs.GetInt("Difficulty",1);
		map = PlayerPrefs.GetInt("Map",1);
		difTxt.text = CDif();
		sMap.sprite = maps[map - 1];
		cMap.sprite = maps[map - 1];
		count = 2;
		cPlayer.text = count.ToString();
		pName.text = "Player"+Random.Range(1,255);

		multiPlayer = GetComponent<MultiPlayer>();
		chat = GetComponent<Chat>();
		audio.Stop ();
	}

	private void OnGUI ()
	{
		Resolution.Android();
		if(MultiPlayer.state == GameState.Multi && !create) {
			GUI.skin = skin;
			//ShowList ();
			if(multiPlayer.GetHosts().Length != checkRe) {
				checkRe = multiPlayer.GetHosts().Length;
				_Refresh();
			}
		}
		else if(MultiPlayer.state == GameState.Lobby) 
		{
			if(playerCount != MultiPlayer.playerList.Count){
				GoToLobby();
				AtLobby();
			}
			chat.Run(new Rect(Resolution.width/2 - 113, Resolution.height/2 -75, 170, 92), multiPlayer.playerName);
		}
		else if(MultiPlayer.state == GameState.How )
		{
			HowTo ();
		}
		else if (MultiPlayer.state == GameState.Error)
		{
			if(playerCount > -1) {
				if(lobby.activeSelf) {
					audio.Play();
					MultiPlayer.state = GameState.Multi;
					Invoke("UnLobby", 0.1f);
					lobbyTask.SetBool("IsOut", true);
					mulTask.SetBool("IsOut", false);
					Invoke("ReturnMenu",0.2f);
				}
				else {
					Invoke("UnActive", 0.4f);
					mulTask.SetBool("IsOut", true);
					ReturnMenu();
				}
				/*audio.Play();
				btSin.SetBool("IsOut", false);
				btMul.SetBool("IsOut", false);
				btHow.SetBool("IsOut", false);
				if(lobby.activeSelf) {
					Invoke("UnLobby", 0.1f);
					multiPlayer.EndGame();
					lobbyTask.SetBool("IsOut", true);
				}else {
					Invoke("UnActive", 0.4f);
					mulTask.SetBool("IsOut", true);
				}*/

				playerCount = -1;
			}
		}

		/*if (logLobby.activeSelf == true) {
			if(Input.GetMouseButtonDown(0))
			{
				logLobby.SetActive(false);
			}
		}*/
	}

	/*public Texture2D bgJoin, bgKey, key, bgJoin2, btJoin;
	private void ShowList ()
	{
		if(multiPlayer.GetHosts().Length > 0)
		{
			//GUI.DrawTexture(new Rect(Resolution.width/2, 0, Resolution.width/2,Resolution.height), bgJoin);
			foreach (HostData data in multiPlayer.GetHosts())
			{
				int y = 0;
				string[] tmp = data.gameName.Split("-;".ToCharArray());
				string name = tmp[0];
				if(tmp.Length > 1) {
					name = tmp[2];
					GUI.DrawTexture(new Rect(Resolution.width/2+54, 95, 84, 82), bgKey);
					GUI.DrawTexture(new Rect(Resolution.width/2+72, 105, 50, 62), key);
				}
				GUI.DrawTexture(new Rect(Resolution.width/2+100, 40, 370, 206), bgJoin2);
				GUI.Label(new Rect(Resolution.width/2+135, 70, 290, 50), name);
				if(data.comment == "Ready"){
					if(GUI.Button(new Rect(Resolution.width/2+184, 120, 202, 90), btJoin))
					{
						if(pName.text.Trim() != "") {
							multiPlayer.playerName = pName.text;
						}else {
							multiPlayer.playerName =  "Player"+Random.Range(1,255);;
						}
						multiPlayer.Join(data);
						playerCount = 0;


					}
				}else {
					GUI.Label(new Rect(Resolution.width/2+184, 120, 202, 90),"Playing");
				}
				y+=180;
			}
		}
	}*/

	private void HowTo () {
		if(/*Input.touchCount > 0 || */Input.GetMouseButtonDown(0))
		{
			ReturnMenu();
		}
	}

	private string CDif() 
	{
		if(difficulty == 1) {
			return "Easy";
		}
		else if(difficulty == 2){
			return "Normal";
		}
		else if(difficulty == 3){
			return "Hard";
		}
		return "???";
	}

	public void BtSingle()
	{
		audio.Play();
		MultiPlayer.state = GameState.Single;

		btMul.SetBool("IsOut", true);
		btSin.SetBool("IsOut", true);
		btHow.SetBool("IsOut", true);
	
		blackBG.SetActive(true);
		sinMenu.SetActive(true);
		sinTask.SetBool("IsOut", false);

	}



	public void BtMulti()
	{
		audio.Play();
		MultiPlayer.state = GameState.Multi;
		//multiPlayer.Refresh();

		btMul.SetBool("IsOut", true);
		btSin.SetBool("IsOut", true);
		btHow.SetBool("IsOut", true);
		//UI Multi Play
		blackBG.SetActive(true);
		mulMenu.SetActive(true);
		mulTask.SetBool("IsOut", false);
		Refresh();
	}
	
	public void BtHow()
	{
		audio.Play();
		MultiPlayer.state = GameState.How;

		btHow.SetBool("IsOut", true);
		btSin.SetBool("IsOut", true);
		btMul.SetBool("IsOut", true);

		imageHow.SetBool("IsIn", true);
	}

	public void ReturnMenu() 
	{
		audio.Play();
		btSin.SetBool("IsOut", false);
		btMul.SetBool("IsOut", false);
		btHow.SetBool("IsOut", false);

		//Single Play //
		if (MultiPlayer.state == GameState.Single) {
			Invoke("UnActive", 0.4f);
			sinTask.SetBool("IsOut", true);
		}
		// Multi Play //
		else if (MultiPlayer.state == GameState.Multi) {
			Invoke("UnActive", 0.4f);
			mulTask.SetBool("IsOut", true);
		}
		// How to //
		else if (MultiPlayer.state == GameState.How) {
			imageHow.SetBool("IsIn", false);
		}
		MultiPlayer.state = GameState.Index;
	}

	private void UnActive ()
	{
		blackBG.SetActive(false);
		sinMenu.SetActive(false);
		mulMenu.SetActive(false);
	}

	public void PMap () 
	{
		audio.Play();
		if(map > 1) {
			sMap.sprite = maps[--map - 1];
			cMap.sprite = maps[map - 1];
		}
	}

	public void NMap () 
	{
		audio.Play();
		if(map < 3) {
			sMap.sprite = maps[++map - 1];
			cMap.sprite = maps[map - 1];
		}
	}

	// Single //
	public void SPlay ()
	{
		audio.Play();
		MultiPlayer.playerList.Add(new Player("Player"));
		SceneList _scene = GetComponent<SceneList>();
		Application.LoadLevel(_scene.getStage(map));
		PlayerPrefs.SetInt("Difficulty",difficulty);
		PlayerPrefs.SetInt("Map", map); 
		MultiPlayer.state = GameState.GamePlay;
	}

	public void PDif () 
	{
		audio.Play();
		if(difficulty > 1) {
			--difficulty;
			difTxt.text = CDif();
		}
	}
	
	public void NDif () 
	{
		audio.Play();
		if(difficulty < 3) {
			++difficulty;
			difTxt.text = CDif();
		}
	}

	// Multi //
	public void MCreate ()
	{
		audio.Play();
		mulCreate.SetActive(true);
		createTask.SetBool("IsOut", false);

		if(pName.text.Trim() != "") {
			multiPlayer.playerName = pName.text;
		}else {
			multiPlayer.playerName =  "Player"+Random.Range(1,255);
		}
		
		if(hName.text.Trim() == "") {
			hName.text = multiPlayer.playerName;
		}
		create = true;
	}
	
	public void MRefresh ()
	{
		audio.Play();
		Refresh();
	}

	private void Refresh() {
		multiPlayer.Refresh();
		_Refresh();
		//checkRe = 0;
		//_Refresh();
		CancelInvoke("Refresh");
		Invoke("Refresh",5f);
	}

	private void _Refresh() {
		foreach(GameObject task in joinTasks)
			Destroy(task);
		joinTasks.Clear();
		if(multiPlayer.GetHosts().Length > 0)
		{
			int i = 0;
			foreach (HostData data in multiPlayer.GetHosts())
			{
				GameObject task = Instantiate(joinTask) as GameObject;
				joinTasks.Add(task);
				joinTasks[i].transform.SetParent(joinArea);
				string[] tmp = data.gameName.Split(';');
				Debug.Log(tmp.Length);
				joinTasks[i].transform.GetChild(1).GetComponent<Text>().text = tmp[0];
				if(tmp.Length < 2)
					joinTasks[i].transform.GetChild(0).gameObject.SetActive(false);
				if(data.comment == "Ready"){
					joinTasks[i].transform.GetChild(3).gameObject.SetActive(false);
					joinTasks[i].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {
						if(pName.text.Trim() != "") {
							multiPlayer.playerName = pName.text;
						}else {
							multiPlayer.playerName =  "Player"+Random.Range(1,255);;
						}
						multiPlayer.Join(data);
						playerCount = 0;
					});
				}else {
					joinTasks[i].transform.GetChild(2).gameObject.SetActive(false);
				}
				i++;
			}
		}
	}

	public void CCreate () 
	{
		audio.Play();
		playerCount = 0;
		if(hName.text.Trim() == "")
			hName.text = multiPlayer.playerName;
		
		if(password.text.Trim() == "")
		{
			multiPlayer.Create(count, hName.text);
		}
		else
		{
			multiPlayer.Create(count, hName.text, password.text);
		}
	}

	public void BCreate ()
	{
		audio.Play();
		Invoke("UnActiveCreate", 0.4f);
		createTask.SetBool("IsOut", true);
		create = false;
	}

	public void NPlayer ()
	{
		audio.Play();
		if (count > 2) {
			cPlayer.text = (--count).ToString();
		}
	}

	public void PPlayer ()
	{
		audio.Play();
		if (count < 4) {
			cPlayer.text = (++count).ToString();
		}
	}

	private void UnActiveCreate()
	{
		mulCreate.SetActive(false);
	}

	public void Log01()
	{
		logLobby.SetActive (false);
	}

	// Lobby //
	private void GoToLobby ()
	{
		if (!lobby.activeSelf) {
			lobby.SetActive(true);
			lobbyTask.SetBool("IsOut", false);
			if(multiPlayer.isHost())
				networkView.RPC("RPCChangMap", RPCMode.AllBuffered, map);
			//lMap.sprite = maps[map - 1];
			//playerCount = MultiPlayer.playerList.Count;
			AtLobby ();
			lPlay.SetActive(multiPlayer.isHost());
			//createTask.SetBool("IsOut", true);
			mulTask.SetBool("IsOut", true);
			Invoke("UnActiveCreate", 0.4f);
		}
	}

	public void ChangeMap ()
	{
		if(multiPlayer.isHost()) {
			audio.Play();
			if (++map > 3) {
				map = 1;
			}
			networkView.RPC("RPCChangMap", RPCMode.AllBuffered, map);
		}
	}

	[RPC]private void RPCChangMap(int map) {
		//audio.Play();
		lMap.sprite = maps[map - 1];
	}

	private void AtLobby ()
	{
		playerCount = MultiPlayer.playerList.Count;
		int now = 0,end = MultiPlayer.playerList.Count;
		foreach (GameObject p in players)
		{
			if (now < end) {
				//p.color = new Color(255,255,255,255);
				p.SetActive(true);
				nPlayer[now].text = MultiPlayer.playerList[now].getName;
			}else if(p.activeSelf){
				//p.color = new Color(255,255,255,0);
				nPlayer[now].text = "";
				p.SetActive(false);
			}
			now++;
		}
	}

	public void BLobby ()
	{
		audio.Play();
		MultiPlayer.state = GameState.Multi;
		Invoke("UnLobby", 0.1f);
		multiPlayer.EndGame();
		lobbyTask.SetBool("IsOut", true);
		mulTask.SetBool("IsOut", false);
		playerCount = -1;
	}

	private void UnLobby ()
	{
		lobby.SetActive(false);
	}

	public void LPlay ()
	{
		audio.Play();
		if (MultiPlayer.playerList.Count > 1) {
			SceneList _scene = GetComponent<SceneList> ();
			multiPlayer.LoadLevel (_scene.getStage (map));
			PlayerPrefs.SetInt("Map", map); 
		} else {
			MultiPlayer.errorLog = "Players less than two.";
			logLobby.SetActive(true);
			logLobby.transform.GetChild(2).GetComponent<Text>().text = "Players less than two.";
		}
	}
}

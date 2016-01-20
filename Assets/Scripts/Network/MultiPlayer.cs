using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiPlayer : MonoBehaviour {
	public InputField pw;
	[SerializeField]private GameObject logEnterPass;
	public string gameType = "WarB";
	public int port = 55555;

	public static GameState state = GameState.Index;
	public static List<Player> playerList = new List<Player> ();
	public string playerName = "";
	private int playerNumber = 0;

	public static string errorLog = "";

	private bool Protected = false;
	private string gameName;

	void Start(){
		logEnterPass.SetActive (false);
	}

	public void Create(int connections,string gameName) {
		this.gameName = gameName;
		Network.InitializeServer(connections-1,port,false);
		MasterServer.RegisterHost(gameType, this.gameName, "Ready");
	}

	public void Create(int connections,string gameName, string password) {
		this.gameName = gameName;
		Network.incomingPassword = password;
		this.gameName = gameName+";";
		Network.InitializeServer(connections-1,port,false);
		MasterServer.RegisterHost(gameType, this.gameName, "Ready");
	}

	public void Refresh() {
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(gameType);
	}

	public HostData[] GetHosts()
	{
		return MasterServer.PollHostList ();
	}

	public void Join(HostData host) {
		if(!host.passwordProtected)
			Network.Connect(host);
		else{
			Protected = true;
			select = host;
		}
	}

	public void EndGame ()
	{
		Network.Disconnect();
		playerList.Clear();
	}

	public bool isHost ()
	{
		return Network.isServer;
	}

	public void LoadLevel (string name)
	{
		networkView.RPC("RPCLoadLevel", RPCMode.AllBuffered, name);
		if(Network.isServer) {
			Network.maxConnections = -1;
			MasterServer.RegisterHost(gameType,gameName,"Playing");
		}
	}

	private void setPlayerNumber() {
		int n = 0;
		foreach (Player p in playerList) {
			p.setNumber(n++);
		}
	}

	// password  password  password  password
	//public string err = "";
	private HostData select;
	//private string pw = "";

	void OnGUI() {
		if(Protected) {
			logEnterPass.SetActive(true);
			/*GUILayout.BeginArea(new Rect(Screen.width/2-100,Screen.height/2-50,200,100));
			GUILayout.BeginVertical("Box");

			pw = GUILayout.PasswordField(pw,'.');
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Cancel"))
				Protected = false;
			if(GUILayout.Button("Connect")) {
				Protected = false;
				Network.Connect(select,pw);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();*/
		}
	}

	// Massage   Massage   Massage   Massage   Massage   Massage 
	private void OnServerInitialized() {
		Debug.Log("Add Player & go to Lobby\nIP:"+Network.player.ipAddress);
		state = GameState.Lobby;
		networkView.RPC("NewPlayer",RPCMode.AllBuffered,playerName,Network.player);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.RegistrationSucceeded){
			//Debug.Log("Server registered");
			Refresh();
		}
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		state = GameState.Lobby;
		networkView.RPC("AddPlayer",RPCMode.Server,playerName,Network.player);
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
		state = GameState.Error;
		errorLog = error.ToString();
		Network.Disconnect();
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		/*Debug.Log("DisconnectedFromServer.");
		playerList.Clear();
		state = GameState.Multi;
		//back = true;
		Refresh();*/
		if(state == GameState.Lobby) {
			Debug.Log("DisconnectedFromServer.");
			playerList.Clear();
			state = GameState.Error;
			errorLog = info.ToString();
			//back = true;
			Refresh();
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		//Network.RemoveRPCs(player);
		//Network.DestroyPlayerObjects(player);
		networkView.RPC("RemovePlayer",RPCMode.AllBuffered,player);
		//state = GameState.Lobby;
		Refresh();
	}

	// Remote Procedure Calls  Remote Procedure Calls  Remote Procedure Calls
	[RPC]
	void NewPlayer(string name, NetworkPlayer net)
	{
		//state = GameState.Lobby;
		if(name.Trim() == "")
			name = PlayerPrefs.GetString("PlayerName","Player"+Random.Range(1,256));
		playerList.Add(new Player(name, net, playerNumber++));
	}

	[RPC]
	public void AddPlayer (string namePlayer,NetworkPlayer netPlayer)
	{
		networkView.RPC("NewPlayer",RPCMode.AllBuffered,namePlayer,netPlayer);
	}

	[RPC]
	void RemovePlayer(NetworkPlayer player)
	{
		foreach(Player p in playerList)
		{
			if(p.getNet == player)
			{
				playerList.Remove(p);
				break;
			}
		}
	}

	[RPC]
	void RPCLoadLevel (string scene)
	{
		setPlayerNumber();
		MultiPlayer.state = GameState.GamePlay;
		Application.LoadLevel(scene);
	}

	[RPC]
	void RandomPlayerSpawn()
	{
		int spawn = 0;
		foreach (Player p in playerList)
		{
			p.setSpawn(spawn++);
		}
	}

	//
	public void LoadSpawn()
	{
		networkView.RPC("RandomPlayerSpawn",RPCMode.AllBuffered);
	}

	public void JButton(){
		logEnterPass.SetActive (false);
		Protected = false;
		Network.Connect(select,pw.text);
	}

	public void BButon(){
		logEnterPass.SetActive (false);
		Protected = false;
	}
}

public class Player{
	public string getName{get; private set;}
	public NetworkPlayer getNet{get; private set;}
	public int getNumber{get; private set;}
	public int getSpawn{get; private set;}
	public bool IsDear = false;
	public int point = 0;

	//Item
	public int bombNum = 1;
	public int damageNum = 1;

	public Player(string playerName, NetworkPlayer network, int number)
	{
		this.getName = playerName;
		this.getNet = network;
		this.getNumber = number;
	}

	public Player(string playerName)
	{
		this.getName = playerName;
		this.getNumber = 0;
	}

	public void setSpawn (int spawn)
	{
		this.getSpawn = spawn;
	}

	public void setNumber(int number) {
		this.getNumber = number;
	}
}
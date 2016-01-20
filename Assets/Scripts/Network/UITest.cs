using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour {

	private MultiPlayer multiPlayer;
	private Chat chat;

	// Use this for initialization
	void Start () {
		multiPlayer = GetComponent <MultiPlayer> ();
		chat = GetComponent <Chat> ();
	}

	void OnGUI()
	{
		if(MultiPlayer.state == GameState.Index)
			Index();
		else if(MultiPlayer.state == GameState.Lobby)
			Lobby();
	}
	
	bool hostOn = false;
	public Texture2D lock2d;
	void Index()
	{
		if(hostOn)
			Host();

		GUILayout.BeginArea(new Rect(0,0,200,500));

		if(GUILayout.Button("Host")) hostOn = true;
		
		if(GUILayout.Button("Re"))
			multiPlayer.Refresh();
		
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(200,0,200,500));
		if(multiPlayer.GetHosts().Length > 0)
			foreach(HostData host in multiPlayer.GetHosts())
		{
			string[] tmp = host.gameName.Split(';');
			GUILayout.BeginHorizontal("Box");
			if(tmp.Length > 1)
				GUILayout.Box(lock2d);
			GUILayout.Label(tmp[1]);
			if(GUILayout.Button("Join"))
				multiPlayer.Join(host);
			GUILayout.EndHorizontal ();
		}
		GUILayout.EndArea();
	}

	string pws = "";

	void Host()
	{
		if(multiPlayer.playerName.Trim() == "")
			multiPlayer.playerName = PlayerPrefs.GetString("PlayerName","Player1");
		GUILayout.BeginArea(new Rect(Screen.width/2-100,Screen.height/2-250,200,500));
		GUILayout.BeginVertical("Box");
		multiPlayer.playerName = GUILayout.TextField(multiPlayer.playerName);
		pws = GUILayout.PasswordField(pws,'.');
		if(GUILayout.Button("OK"))
		if(pws != "")
			multiPlayer.Create (2,"Test",pws);
		else
			multiPlayer.Create (2,"Test");
		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}

	public Rect chatR;

	void Lobby()
	{
		/*GUILayout.BeginArea(new Rect(0,0,120,300));
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUILayout.Label("Lobby");
		foreach(Player p in MultiPlayer.playerList)
		{
			GUILayout.BeginHorizontal("Box");
			GUILayout.Label(p.getName);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();*/

		chat.Run(chatR, multiPlayer.playerName);
	}
}

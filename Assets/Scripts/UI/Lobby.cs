using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {

	public Texture2D bgHost, bgJoin, bgName, bgCreate, btCreate, btBack, btRefresh, bgKey, bgJoin2, key, btJoin;
	public Texture2D bgCreateHost, bgLobby, btStart, btBack2;
	public Texture previous, next;
	public Texture2D[] map, character;
	private string hostName = "", passwd = "";
	private bool host = false;
	private int mapNum = 0, playerCount = 2;
	private Vector2 moveL = Vector2.zero, moveR = Vector2.zero, scale = Vector2.zero;
	private bool back = false;
	private float alpha = 0;

	private MultiPlayer multiPlayer;
	private Chat chat;

	void Start()
	{
		moveL.x = Resolution.width;
		moveR.y = Resolution.height;
		multiPlayer = GetComponent<MultiPlayer>();
		multiPlayer.Refresh();
		chat = GetComponent<Chat>();
		multiPlayer.playerName = "Player"+Random.Range(1,255);
	}

	public bool MultiPlay (bool _return)
	{
		if(MultiPlayer.state == GameState.Lobby)
		{
			_Lobby ();
		}

		GUI.DrawTexture(new Rect(0-moveL.x,0,Resolution.width/2,Resolution.height), bgHost);
		GUI.DrawTexture(new Rect(50-moveL.x,50, 457.6f, 148.2f), bgName);
		GUI.DrawTexture(new Rect(45-moveL.x,190, 465.4f, 205.4f), bgCreate);
		GUI.DrawTexture(new Rect(45-moveL.x,390, 465.4f, 205.4f), bgCreate);

		multiPlayer.playerName = GUI.TextField(new Rect(230-moveL.x,93, 240, 60),multiPlayer.playerName,12);
		if(GUI.Button(new Rect(130-moveL.x,232, 297.6f, 117.6f), btCreate))
		{
			host = true;
			if(hostName.Trim() == "")
				hostName = multiPlayer.playerName;
		}
		else if(GUI.Button(new Rect(250-moveL.x,440, 178*1.3f, 84*1.3f), btRefresh))
		{
			multiPlayer.Refresh();
			Debug.Log(multiPlayer.GetHosts().Length);
		}
		else if(GUI.Button(new Rect(90-moveL.x,440, 124*1.3f, 84*1.3f), btBack))
		{
			_return = true;
		}

		if(multiPlayer.GetHosts().Length > 0)
		{
			GUI.DrawTexture(new Rect(Resolution.width/2+moveR.x, 0-moveR.y, Resolution.width/2,Resolution.height), bgJoin);
			foreach (HostData data in multiPlayer.GetHosts())
			{
				int y = 0;
				GUI.DrawTexture(new Rect(Resolution.width/2+54+moveR.x, y+95-moveR.y, 84, 82), bgKey);
				GUI.DrawTexture(new Rect(Resolution.width/2+72+moveR.x, y+105-moveR.y, 50, 62), key);
				GUI.DrawTexture(new Rect(Resolution.width/2+100+moveR.x, y+40-moveR.y, 370, 206), bgJoin2);
				GUI.Label(new Rect(Resolution.width/2+135+moveR.x, y+70-moveR.y, 290, 50), data.gameName);
				if(GUI.Button(new Rect(Resolution.width/2+184+moveR.x, y+120-moveR.y, 202, 90), btJoin))
				{
					multiPlayer.Join(data);
				}
				y+=180;
			}
		}

		if(host)
		{
			Host();

		}
		else if(MultiPlayer.state == GameState.Lobby)
		{
			if(moveL.x < Resolution.width)
			{
				moveL = Vector2.Lerp(moveL, new Vector2(Resolution.width/2+100, 0), 0.03f);
				moveR = Vector2.Lerp(moveR, new Vector2(Resolution.width/2+100, 0), 0.03f);
			}
		}
		else
		{
			if(_return)
			{
				moveL = Vector2.Lerp(moveL, new Vector2(Resolution.width/2 +100, 0), 0.03f);
				moveR = Vector2.Lerp(moveR, new Vector2(0, Resolution.height+100), 0.03f);
				return true;
			}
			else if(moveL.x > 0.1f)
			{
				moveL = Vector2.Lerp(moveL, Vector2.zero, 0.05f);
				moveR = Vector2.Lerp(moveR, Vector2.zero, 0.03f);
			}
		}
		return false;
	}

	private void Host()
	{
		if(MultiPlayer.state == GameState.Lobby)
		{
			if(scale.x > 0.1f)
			{
				scale = Vector2.Lerp(scale, Vector2.zero, 0.1f);
			}
			else
			{
				host = false;
			}
		}
		else if(back)
		{
			if(moveL.x > 0.1f)
			{
				moveL = Vector2.Lerp(moveL, Vector2.zero, 0.1f);
				moveR = Vector2.Lerp(moveR, Vector2.zero, 0.1f);
				scale = Vector2.Lerp(scale, Vector2.zero, 0.1f);
			}
			else
			{
				host = false;
				back = false;
			}
		}
		else if(moveL.x < Resolution.width && !back)
		{
			moveL = Vector2.Lerp(moveL, new Vector2(Resolution.width/2, 0), 0.05f);
			moveR = Vector2.Lerp(moveR, new Vector2(Resolution.width/2, 0), 0.05f);
			scale = Vector2.Lerp(scale, Vector2.one, 0.05f);
		}
		
		GUI.DrawTexture(new Rect(Resolution.width/2-282.8f* scale.x,Resolution.height/2 - 279.3f* scale.y, 565.6f* scale.x, 558.6f* scale.y), bgCreateHost);

		hostName = GUI.TextField(new Rect(Resolution.width/2+10* scale.x, Resolution.height/2 - 235*scale.y, 240*scale.x,50*scale.y), hostName,12);
		passwd = GUI.PasswordField(new Rect(Resolution.width/2+10* scale.x, Resolution.height/2 - 170*scale.y, 240*scale.x,50*scale.y),passwd,'-',12);

		GUI.Label(new Rect(Resolution.width/2 + 105*scale.x, Resolution.height/2 - 58*scale.y, 50*scale.x, 50*scale.y), playerCount.ToString());
		if(GUI.Button(new Rect(Resolution.width/2 + 30*scale.x, Resolution.height/2 - 75*scale.y, 70*scale.x, 70*scale.y), previous))
		{
			playerCount--;
			if(playerCount < 2)
				playerCount = 2;
		}
		else if(GUI.Button(new Rect(Resolution.width/2 + 160*scale.x, Resolution.height/2 -75*scale.y, 70*scale.x, 70*scale.y),next))
		{
			playerCount++;
			if(playerCount > 4)
				playerCount = 4;
		}

		GUI.DrawTexture(new Rect(Resolution.width/2 - 50*scale.x, Resolution.height/2 +5*scale.y, 200*scale.x, 120*scale.y), map[mapNum]);
		if(GUI.Button(new Rect(Resolution.width/2 - 130*scale.x, Resolution.height/2 + 30*scale.y, 70*scale.x, 70*scale.y), previous))
		{
			mapNum--;
			if(mapNum < 0)
				mapNum = 0;
		}
		else if(GUI.Button(new Rect(Resolution.width/2 + 160*scale.x, Resolution.height/2 +30*scale.y, 70*scale.x, 70*scale.y),next))
		{
			mapNum++;
			if(mapNum > map.Length-1)
				mapNum = map.Length-1;
		}
		
		if(GUI.Button(new Rect(Resolution.width/2+50*scale.x,Resolution.height/2+160*scale.y, 112.84f*scale.x, 76.44f*scale.y), btCreate))
		{
			if(hostName.Trim() == "")
				hostName = multiPlayer.playerName;

			if(passwd.Trim() == "")
				multiPlayer.Create(playerCount, hostName);
			else
				multiPlayer.Create(playerCount, hostName, passwd);
		}
		else if(GUI.Button(new Rect(Resolution.width/2 -200*scale.x,Resolution.height/2 +160*scale.y, 112.84f*scale.x, 76.44f*scale.y), btBack))
		{
			back = true;
		}
	}

	void _Lobby ()
	{
		Color _default = GUI.color;
		GUI.color = new Color(1,1,1, alpha);
		if(back)
		{
			alpha -= 0.01f;
			if(alpha <= 0) {
				multiPlayer.EndGame();
				MultiPlayer.state = GameState.Multi;
				back = false;
			}
		}
		else if(alpha < 1f) 
			alpha += 0.01f;
		GUI.DrawTexture(new Rect(Resolution.width/2-540,Resolution.height/2-324,1080,648), bgLobby);
		GUI.DrawTexture(new Rect(Resolution.width/2 - 495, Resolution.height/2 -285, 340, 204), map[mapNum]);
		if(GUI.Button(new Rect(Resolution.width/2 - 495, Resolution.height/2 -285, 340, 204), ""))
		{
			mapNum++;
			if(mapNum >= map.Length)
				mapNum = 0;
		}
		int x = 0, i = 0;
		foreach (Player player in MultiPlayer.playerList)
		{
			GUI.DrawTexture(new Rect(Resolution.width/2 - 105+x, Resolution.height/2 -280, 132, 160), character[i++]);
			GUI.Box(new Rect(Resolution.width/2 - 105+x, Resolution.height/2 -280, 132, 160), player.getName);
			x += 156;
		}

		chat.Run(new Rect(Resolution.width/2 - 113, Resolution.height/2 -75, 170, 92), multiPlayer.playerName);

		if(multiPlayer.isHost())
		{
			if(GUI.Button(new Rect(Resolution.width/2 - 410, Resolution.height/2 +80, 184, 70), btStart))
			{
				SceneList _scene = GetComponent<SceneList>();
				multiPlayer.LoadLevel(_scene.getStage(mapNum+1));
			}

		}

		if(GUI.Button(new Rect(Resolution.width/2 - 410, Resolution.height/2 +160, 184, 70), btBack2))
		{
			back = true;
		}
		GUI.color = _default;
	}

	public bool Error(bool re) {
		GUILayout.BeginArea(new Rect(Resolution.width/2-100,Resolution.height/2-100,200,200));
		GUILayout.BeginVertical("Box");
		GUILayout.Label(MultiPlayer.errorLog);
		if(GUILayout.Button("OK")){
			//MultiPlayer.state = GameState.Index;
			MultiPlayer.errorLog = "";
			return true;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		return re;
	}
}

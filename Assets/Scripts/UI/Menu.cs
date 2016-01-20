using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUISkin skin;
	public Texture2D bg, logo, howTo, bgSP, btMenu, bt, previous, next;
	public Texture2D[] map;
	//private int show = 0;
	private int mapNum = 0;
	private int difficulty = 1;
	private Vector2 scale = Vector2.zero, moveMenu = Vector2.zero;
	private bool _return = false;
	private Lobby lobby;

	void Start ()
	{
		lobby = GetComponent<Lobby>(); 
		difficulty = PlayerPrefs.GetInt("Difficulty",1);
	}

	void OnGUI ()
	{
		if(Input.GetKeyDown(KeyCode.A))
			Debug.Log(MultiPlayer.state);
		else if (Input.GetKeyDown(KeyCode.B))
			_return = true;
		GUI.skin = skin;
		Resolution.Android();
		GUI.DrawTexture(new Rect(0, 0, Resolution.width, Resolution.height), bg);
		GUI.DrawTexture(new Rect(50,50,400,242), logo);

		if(GUI.Button(new Rect(Resolution.width - 450 + moveMenu.x, Resolution.height - 390, 428.8f, 122.4f), btMenu))
		{
			MultiPlayer.state = GameState.Single; //show = 1;
		}
		else if(GUI.Button(new Rect(Resolution.width - 450 + moveMenu.x, Resolution.height - 260, 428.8f, 122.4f), btMenu))
		{
			MultiPlayer.state = GameState.Multi; //show = 2;
		}
		else if(GUI.Button(new Rect(Resolution.width - 450 + moveMenu.x, Resolution.height - 130, 428.8f, 122.4f), btMenu))
		{
			MultiPlayer.state = GameState.How; //show = 3;
		}

		GUI.skin.label.fontSize = 40;
		GUI.Label(new Rect(Resolution.width - 450 + moveMenu.x, Resolution.height - 390, 428.8f, 122.4f), "Sigle Play");
		GUI.Label(new Rect(Resolution.width - 440 + moveMenu.x, Resolution.height - 260, 428.8f, 122.4f), "Multi Play");
		GUI.Label(new Rect(Resolution.width - 415 + moveMenu.x, Resolution.height - 130, 428.8f, 122.4f), "How to play");

		if( MultiPlayer.state == GameState.Single )//show == 1)
		{
			GUI.skin.label.fontSize = (int)(40*scale.x);
			SinglePlay ();
		}
		else if(MultiPlayer.state == GameState.Multi || MultiPlayer.state == GameState.Lobby)//show == 2)
		{
			_return = lobby.MultiPlay(_return);
		}
		else if(MultiPlayer.state == GameState.How )//show == 3)
		{
			HowTo ();
		}
		else if (MultiPlayer.state == GameState.Error)
		{
			_return = lobby.Error(_return);
		}
		else if(!_return && MultiPlayer.state == GameState.Index)
		{
			//MultiPlayer.state = GameState.Index;
			_return = true;
		}

		if (_return)
		{
			moveMenu = Vector2.Lerp(moveMenu, Vector2.zero, 0.1f);
			scale = Vector2.Lerp(scale, Vector2.zero, 0.1f);
			if (moveMenu.x < 0.1f)
			{
				MultiPlayer.state = GameState.Index; //show = 0;
				_return = false;
			}
		}
		else if(MultiPlayer.state != GameState.Index )//show != 0)
		{
			if(moveMenu.x < 450)
			{
				moveMenu = Vector2.Lerp(moveMenu, new Vector2(450, 0), 0.1f);
			}
			if(scale.x < 1)
			{
				scale = Vector2.Lerp(scale, Vector2.one, 0.05f);
			}
		}
	}

	void SinglePlay ()
	{
		GUI.DrawTexture(new Rect(Resolution.width/2 - 560*scale.x, Resolution.height - 640*scale.y, 1120*scale.x, 672*scale.y), bgSP);
			

		string diff = "";
		if(difficulty == 2)
			diff = "NORMAL";
		else if(difficulty == 3)
			diff = "HARD";
		else
			diff = "EASY";
		GUI.Label(new Rect(Resolution.width/2 - 150*scale.x, Resolution.height-470*scale.y, 300*scale.x, 40*scale.y), diff);
		
		if(GUI.Button(new Rect(Resolution.width/2 - 190*scale.x,Resolution.height-480*scale.y, 60*scale.x, 60*scale.y), previous))
		{
			difficulty--;
			if(difficulty < 1)
				difficulty = 1;
		}
		else if(GUI.Button(new Rect(Resolution.width/2 + 130*scale.x,Resolution.height-480*scale.y, 60*scale.x, 60*scale.y),next))
		{
			difficulty++;
			if(difficulty > 3)
				difficulty = 3;
		}

		GUI.DrawTexture(new Rect(Resolution.width/2 - 150*scale.x, Resolution.height - 370*scale.y, 300*scale.x, 180*scale.y), map[mapNum]);
		if(GUI.Button(new Rect(Resolution.width/2 - 270*scale.x, Resolution.height - 315*scale.y, 90*scale.x, 90*scale.y), previous))
		{
			mapNum--;
			if(mapNum < 0)
				mapNum = 0;
		}
		else if(GUI.Button(new Rect(Resolution.width/2 + 180*scale.x, Resolution.height - 315*scale.y, 90*scale.x, 90*scale.y),next))
		{
			mapNum++;
			if(mapNum > map.Length-1)
				mapNum = map.Length-1;
		}
			
		if(GUI.Button(new Rect(Resolution.width/2 - 290*scale.x, Resolution.height -130*scale.y, 250*scale.x, 100*scale.y), bt))
		{
			_return = true;
		}
		GUI.Label(new Rect(Resolution.width/2 - 290*scale.x, Resolution.height -130*scale.y, 250*scale.x, 100*scale.y), "BACK");
			
		if(GUI.Button(new Rect(Resolution.width/2 + 40*scale.x, Resolution.height -130*scale.y, 250*scale.x, 100*scale.y), bt))
		{
			MultiPlayer.playerList.Add(new Player("Player"));
			SceneList _scene = GetComponent<SceneList>();
			Application.LoadLevel(_scene.getStage(mapNum+1));
			PlayerPrefs.SetInt("Difficulty",difficulty);
		}
		GUI.Label(new Rect(Resolution.width/2 + 40*scale.x, Resolution.height -130*scale.y,250*scale.x,100*scale.y),"PLAY");
	}

	void HowTo ()
	{
		GUI.DrawTexture(new Rect(Resolution.width/2-600*scale.x, Resolution.height/2-360*scale.y, 1200* scale.x, 720 * scale.y), howTo);
		if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
		{
			_return = true;
		}
	}
}

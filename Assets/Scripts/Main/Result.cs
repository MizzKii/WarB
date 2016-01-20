using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Result : MonoBehaviour {

	public GUISkin skin;
	public Texture2D head, back, again, win, lose, bgRank;
	public Texture2D[] player;
	public List<Player> rank = new List<Player>();
	public Rect[] rectRank;

	public void Win (string bestTime,string time, int hightScore, int score, int bonus, int map)
	{
		if(skin != null)
			GUI.skin = skin;
		GUI.DrawTexture(new Rect(0,0,Resolution.width, Resolution.height), head);
		GUI.DrawTexture(new Rect(Resolution.width/2-420, 40, 800, 480), win);
		GUI.Label(new Rect(Resolution.width/2-400,240,800,50),"Best Time : "+ bestTime +"    Best Score : " + hightScore.ToString());
		GUI.Label(new Rect(Resolution.width/2-200,320,400,50), "All Score : " + (score+bonus).ToString());

		GUI.Label(new Rect(Resolution.width/2-200,360,400,50),"Time : "+time);
		GUI.Label(new Rect(Resolution.width/2-200,400,400,50),"Score : "+score);
		GUI.Label(new Rect(Resolution.width/2-200,440,400,50),"Bonus : "+bonus);
		if(GUI.Button(new Rect(150, 500, 259, 78), back))
		{
			MultiPlayer.playerList.Clear();
			MultiPlayer.state = GameState.Index;
			Application.LoadLevel(0);
		}
		if(GUI.Button(new Rect(615, 500, 259, 78), again)) {
			MultiPlayer.playerList.Clear();
			MultiPlayer.playerList.Add(new Player("Player"));
			Application.LoadLevel(map);
		}
	}

	public void Lose (string bestTime, int hightScore, int map)
	{
		if(skin != null)
			GUI.skin = skin;
		GUI.DrawTexture(new Rect(0,0,Resolution.width, Resolution.height), head);
		GUI.DrawTexture(new Rect(Resolution.width/2-420, 40, 800, 480), lose);
		GUI.Label(new Rect(Resolution.width/2-400,240,800,50),"Best Time : "+ bestTime +"    Best Score : " + hightScore.ToString());
		if(GUI.Button(new Rect(150, 500, 259, 78), back))
		{
			MultiPlayer.playerList.Clear();
			MultiPlayer.state = GameState.Index;
			Application.LoadLevel(0);
		}
		if(GUI.Button(new Rect(615, 500, 259, 78), again)) {
			MultiPlayer.playerList.Clear();
			MultiPlayer.playerList.Add(new Player("Player"));
			Application.LoadLevel(map);
		}
	}

	public void Ranking ()
	{
		if(skin != null)
			GUI.skin = skin;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.DrawTexture(new Rect(0,0,Resolution.width, Resolution.height), bgRank);
		int i = 0;
		foreach (Player p in rank) {
		//for(i=0;i<4;i++){
			GUILayout.BeginArea(rectRank[i]);
			GUILayout.BeginHorizontal();
			//GUILayout.Box(player[p.getNumber]);
			GUILayout.Label((i+1)+"."+p.getName+" Point:"+p.point);
			//GUILayout.Label((i+1)+"."+"test"+" Point:"+"test");
			if(i == 0)
				GUILayout.Label(" Survivor!");
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			//GUI.DrawTexture(new Rect(),player[p.getNumber]);
			i++;
		}
		if(GUI.Button(new Rect(50, Resolution.height - 100, 250, 50), "Back to Menu")) {
			GetComponent<GamePlay>().Reset();
		}
	}
}

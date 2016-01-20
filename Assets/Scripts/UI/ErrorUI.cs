using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorUI : MonoBehaviour {

	[SerializeField]private GameObject logPass;
	[SerializeField]private Text text;

	void Start()
	{
		logPass.SetActive (false);
	}

	void OnGUI ()
	{
		if(MultiPlayer.state == GameState.Error)
		{
			logPass.SetActive(true);
			text.text = MultiPlayer.errorLog;
			/*GUILayout.BeginArea(new Rect(Screen.width/2-100,Screen.height/2-100,200,200));
			GUILayout.BeginVertical("Box");
			GUILayout.Label(MultiPlayer.errorLog);
			if(GUILayout.Button("OK"))
				MultiPlayer.state = GameState.Index;
			GUILayout.EndVertical();
			GUILayout.EndArea();*/
		}
	}

	public void WrongPass()
	{
		MultiPlayer.state = GameState.Index;
		logPass.SetActive (false);
	}
}

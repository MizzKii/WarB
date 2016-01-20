using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Setting : MonoBehaviour {

	private string sound = "Mute", pause = "Pause";
	public GUISkin guiSkin;
	public Texture2D black;
	public List<AudioSource> audioList = new List<AudioSource>();

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt("volume", 1) == 1) {
			sound = "Mute";
			foreach(AudioSource aus in audioList) {
				aus.mute = false;
			}
		}else {
			sound = "Unmute";
			foreach(AudioSource aus in audioList) {
				aus.mute = true;
			}
		}
	}
	
	void OnGUI() {
		GUI.skin = guiSkin;
		Resolution.Android();
		if(GUI.Button(new Rect(Resolution.width-180,0,200,60), sound)) {
			if(sound == "Mute") {
				sound = "Unmute";
				foreach(AudioSource aus in audioList) {
					if(aus)
						aus.mute = true;
				}
				PlayerPrefs.SetInt("volume", 0);
			}else {
				sound = "Mute";
				foreach(AudioSource aus in audioList) {
					if(aus)
						aus.mute = false;
				}
				PlayerPrefs.SetInt("volume", 1);
			}
		}
		if(MultiPlayer.state == GameState.GamePlay) {	
			if(pause == "Play") {
				GUI.DrawTexture(new Rect(0,0,Resolution.width,Resolution.height), black);
				GUI.color = new Color(0.7f,0.7f,0.7f);
				GUI.Label(new Rect(Resolution.width/2-50,Resolution.height/2-25,100,50), "Pause");
				GUI.color = new Color(1f,1f,1f);
			}
			if(GUI.Button(new Rect(Resolution.width-170,50,170,60), pause)) {
				if(pause == "Pause") {
					pause = "Play";
					Time.timeScale = 0f;
				}else {
					pause = "Pause";
					Time.timeScale = 1f;
				}
				if(!GamePlay.IsSingle)
					networkView.RPC("Pause", RPCMode.All,Time.timeScale);
			}
		}
	}

	public void AddAudioSource(AudioSource aus) {
		if(PlayerPrefs.GetInt("volume", 1)==1)
			aus.mute = false;
		else
			aus.mute = true;
		audioList.Add(aus);
	}

	public void RemoveAudioSource(AudioSource aus) {
		audioList.Remove(aus);
	}

	public void Pause(float time) {
		Time.timeScale = time;
	}
}

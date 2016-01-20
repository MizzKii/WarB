using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {

	public int maxLog = 20;
	public int maxChar = 50;
	public float fontSize = 35;
	public int btSentWidth = 100;
	public float textFieldHeight = 50;
	public string sent = "Sent";

	public Texture2D bgChatLog, bgText, bgTextField, btSent;
	public GUISkin chatSkin;

	private string chat = "";
	private float enter = 0;
	private float between = 10;
	private Vector2 scroollChat = Vector2.zero;
	private List<string> log = new List<string>();

	public void Run(Rect rect, string playerName)
	{
		if(chatSkin != null)
			GUI.skin = chatSkin;

		Rect newRect = new Rect(rect.min.x,rect.min.y,rect.max.x,rect.max.y-textFieldHeight);
		if(bgChatLog != null)
			GUI.DrawTexture(newRect,bgChatLog);


		scroollChat = GUI.BeginScrollView(new Rect(newRect.x, newRect.y+2,newRect.width,newRect.height-4),scroollChat,
		                                  new Rect(newRect.x,newRect.y,newRect.width-16,fontSize*log.Count));
		enter = rect.min.y;
		TextAnchor defaultTA = GUI.skin.label.alignment;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		foreach(string tmp in log)
		{
			Rect text = new Rect(rect.min.x+10,enter,rect.max.x,fontSize);
			if(bgText != null)
				GUI.DrawTexture(text,bgText);
			GUI.Label(text,tmp);
			enter += fontSize;
		}
		GUI.EndScrollView();

		newRect.y += newRect.height + between;
		newRect.width -= btSentWidth + between;
		newRect.height = textFieldHeight;
		if(bgTextField != null)
		GUI.DrawTexture(newRect,bgTextField);
		chat = GUI.TextField(newRect,chat,maxChar);

		newRect.x += rect.max.x - btSentWidth;
		newRect.width = btSentWidth;
		if(GUI.Button(newRect,btSent) 
		   && chat.Trim () != ""){
			networkView.RPC("Send", RPCMode.All, playerName+":"+chat);
			chat = "";
		}
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(newRect,sent);

		GUI.skin.label.alignment = defaultTA;
	}
	
	[RPC]
	void Send(string message){
		log.Add(message);
		scroollChat.y += fontSize;
		if(log.Count > maxLog)
			log.Remove(log[0]);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		log.Clear();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		log.Clear();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Log : MonoBehaviour {

	//public static Log instance{get; private set;}
	
	private List<string> logList = new List<string>();
	private List<float> logY = new List<float>();
	
	public LogPosition position;
	public float x = 0, y = 0;
	public float width = 100, height = 30, speed = 1;

	public float startY = 100;

	public float space = 30;

	//private Log(){}

//	void Start()
//	{
//		instance = this;
//	}

	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		Color defaultColor = GUI.color ;
		for(int i = 0; i < logList.Count; i++)
		{
			logY[i] -= 0.3f*speed;
			if(logY[i] < startY) {
				Color newColor = defaultColor;
				newColor.a = logY[i]*2/100+0.4f;
				//logY[i] -= 0.3f*speed;
				GUI.color = newColor;
				GUI.Label(setRect(logY[i]),logList[i]);

				if(newColor.a <= 0)
				{
					logList.Remove(logList[i]);
					logY.Remove(logY[i]);
				}
			}
		}
		GUI.color = defaultColor;

		if(Input.GetKey(KeyCode.Z))
			AddLog("1 2 3 4 5 6 7 8 9 10 11 12");
	}

	public void AddLog(string text)
	{
		logList.Add(text);
		if(logY.Count > 0 && logY[logY.Count-1] + space > startY) {
			logY.Add(logY[logY.Count-1]+space);
		}else {
			logY.Add(startY);
		}
	}

	Rect setRect(float newY)
	{
		if(position == LogPosition.LeftTop) {
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			return new Rect(x, y+newY, width, height);
		}else if(position == LogPosition.LeftMiddle) {
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			return new Rect(x, Screen.height/2+y+newY, width, height);
		}else if(position == LogPosition.LeftUnder){
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			return new Rect(x, Screen.height+y+newY, width, height);
		}else if(position == LogPosition.CenterTop){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			return new Rect(Screen.width/2+x, y+newY, width, height);
		}else if(position == LogPosition.Center){
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			return new Rect(Screen.width/2+x, Screen.height/2+y+newY, width, height);
		}else if(position == LogPosition.CenterUnder){
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			return new Rect(Screen.width/2+x, Screen.height+y+newY, width, height);
		}else if(position == LogPosition.RightTop){
			GUI.skin.label.alignment = TextAnchor.MiddleRight;
			return new Rect(Screen.width+x, y+newY, width, height);
		}else if(position == LogPosition.RightMiddle){
			GUI.skin.label.alignment = TextAnchor.MiddleRight;
			return new Rect(Screen.width+x, Screen.height/2+y+newY, width, height);
		}else if(position == LogPosition.RightUnder){
			GUI.skin.label.alignment = TextAnchor.MiddleRight;
			return new Rect(Screen.width+x, Screen.height+y+newY, width, height);
		}else
			return new Rect();
	}
}

public enum LogPosition
{
	LeftTop,
	LeftMiddle,
	LeftUnder,
	CenterTop,
	Center,
	CenterUnder,
	RightTop,
	RightMiddle,
	RightUnder
}

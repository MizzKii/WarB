using UnityEngine;
using System.Collections;

public class Resolution : MonoBehaviour {
	
	public static float width = 0, height = 0;

	public void iPhone5 ()
	{
		width = 1136;
		height = 640;
		Vector3 newScale = Vector3.one;
		newScale.x = Screen.width/width;
		newScale.y = Screen.height/height;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, newScale);
	}

	public static void Android () 
	{
		width = 1024;
		height = 614;
		Vector3 newScale = Vector3.one;
		newScale.x = Screen.width/width;
		newScale.y = Screen.height/height;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, newScale);
	}
}

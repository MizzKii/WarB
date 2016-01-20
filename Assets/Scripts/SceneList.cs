using UnityEngine;
using System.Collections;

public class SceneList : MonoBehaviour {

	public string[] stage = {"Menu", "stage_classic", "stage_rectangle", "stage_plus"};
	
	public string getStage(int index)
	{
		if(index > stage.Length)
			return "Menu";
		else
			return stage[index];
	}
}
using UnityEngine;
using System.Collections;

public class LoadStage : MonoBehaviour {
	
	public static bool loading {get; private set;}
	public static int queuing {get; private set;}
	private static int lastQueue;
	private Texture imageLoad = null;
	private bool bombObject = false;

	[SerializeField]
	private GameObject[] BombObject;

	void Awake ()
	{
		lastQueue = 0;
		queuing = 0;
		loading = false;
	}

	// Use this for initialization
	void Start () {
		if(BombObject.Length > 0)
			RandomBombObject();
		if (lastQueue > 0) {
			loading = true;
			if(imageLoad == null)
				imageLoad = (Texture)Resources.Load("Images/Black",typeof(Texture));
		}
	}

	void OnGUI(){
		if(loading){
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),imageLoad);
			GUI.Label(new Rect(Screen.width/2-20,Screen.height/2-25,100,50),"Load... ("+queuing.ToString()+"/"+lastQueue.ToString()+")");
			if(queuing >= lastQueue && bombObject){
				loading = false;
				Destroy(this);
			}
		}
	}

	void RandomBombObject()
	{
		if(GamePlay.IsSingle)
		{
			int bombNum = (int)Random.Range(0,BombObject.Length);
			setBombObject(bombNum);
		}
		else
		{
			if(Network.isServer)
			{
				int bombNum = (int)Random.Range(0,BombObject.Length-1);
				networkView.RPC("setBombObject",RPCMode.AllBuffered,bombNum);
			}
		}
	}

	[RPC]void setBombObject (int number)
	{
		bombObject = true;
		BombObject[number].SetActive(true);
		foreach(GameObject ob in BombObject)
			if(!ob.activeSelf)
				Destroy(ob);
	}

	public static int AddQueue(){
		return lastQueue++;
	}

	public static void NextQueue(){
		queuing++;
	}
}

using UnityEngine;
using System.Collections;

public class AddBot : MonoBehaviour {

	public GameObject bot;
	public Transform[] spawn;

	private int myQueue = -1;

	void FixedUpdate ()
	{
		if(myQueue < 0 && !LoadStage.loading){
			myQueue = LoadStage.AddQueue();
		}
		else if(myQueue == LoadStage.queuing)
			Built();
	}

	void Built () {
		if(GamePlay.IsSingle)
		{
			GamePlay gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();
			foreach(Transform transform in spawn)
			{
				Instantiate(bot, transform.position, bot.transform.rotation);
				gamePlay.AddBot();
			}
		}
		else if(Network.isServer)
		{
			GamePlay gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();
			foreach(Transform transform in spawn)
			{
				Network.Instantiate(bot, transform.position, bot.transform.rotation,0);
				gamePlay.AddBot();
			}
		}
		LoadStage.NextQueue();
		Destroy(gameObject);
	}
}

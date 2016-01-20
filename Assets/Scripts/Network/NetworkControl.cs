using UnityEngine;
using System.Collections;

public class NetworkControl : MonoBehaviour {
	
	public static bool isServer {get; private set;}
	public GameObject Bomb;
	
	void Awake () {
		if(Network.isServer)
			isServer = true;
		else
			isServer = false;
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		if(MultiPlayer.state != GameState.EndGame) {
			Debug.Log("Server Disconnected.");
			Application.LoadLevel("Menu");
			MultiPlayer.state = GameState.Error;
			MultiPlayer.errorLog = info.ToString();
			MultiPlayer.playerList.Clear();
		}
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		if(MultiPlayer.state != GameState.EndGame) {
			Debug.Log("Clean up after player " + player);
			//Network.RemoveRPCs(player);
			//Network.DestroyPlayerObjects(player);

			GameObject.Find("Scripts").GetComponent<GamePlay>().PlayerDisconnected(player);
			//MultiPlayer.playerList.Clear();
			//MultiPlayer.state = GameState.Index;
			//Application.LoadLevel("Menu");
		}
	}

	public GameObject CreateGameObject (GameObject ob, Vector3 position, Quaternion rotation)
	{
		if(GamePlay.IsSingle)
		{
			return (GameObject)Instantiate(ob, position, rotation);
		}
		else
		{
			return (GameObject)Network.Instantiate(ob, position, rotation,0);
		}
	}
	
//	public void CreateBomb (GameObject ob, Vector3 position, Quaternion rotation, Player player)
//	{
//		GameObject go;
//		if(GamePlay.IsSingle)
//		{
//			go = (GameObject)Instantiate(ob, position, rotation);
//			go.GetComponent<Bomb>().setPlayer(player);
//		}
//		else
//		{
//			networkView.RPC("setBomb",RPCMode.All,position.x,position.z,player.getNet);
//		}
//	}
//
//	[RPC]
//	private void setBomb(float x, float z,NetworkPlayer net)
//	{
//		Vector3 pos = new Vector3(x,0,z);
//		GameObject b = (GameObject)Instantiate(Bomb, pos, Bomb.transform.rotation);
//
//		foreach(Player p in MultiPlayer.playerList)
//		{
//			if(p.getNet == net)
//			{
//				b.GetComponent<Bomb>().setPlayer(p);
//				break;
//			}
//		}
//	}
}

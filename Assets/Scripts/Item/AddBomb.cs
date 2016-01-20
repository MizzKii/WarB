using UnityEngine;
using System.Collections;

public class AddBomb : Item {

	public override void PickUp (GameObject character)
	{
		//if(GamePlay.IsSingle)
			GamePlay.myPlayer.bombNum++;
//		else
//			foreach (Player p in MultiPlayer.playerList)
//			{
//				if(p.getNet == Network.player)
//				{
//					p.bombNum++;
//					break;
//				}
//			}

		GameObject.Find("Scripts").GetComponent<GamePlay>().AddLog(GamePlay.myPlayer.getName + " Add Bomb");
	}
}

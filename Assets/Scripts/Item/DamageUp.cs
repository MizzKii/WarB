using UnityEngine;
using System.Collections;

public class DamageUp : Item {

	public override void PickUp (GameObject character)
	{
//		foreach (Player p in MultiPlayer.playerList)
//		{
//			if(p.getNet == Network.player)
//			{
//				p.damageNum++;
//				break;
//			}
//		}
		GamePlay.myPlayer.damageNum++;
		GameObject.Find("Scripts").GetComponent<GamePlay>().AddLog(GamePlay.myPlayer.getName + " Add Damage");
	}
}

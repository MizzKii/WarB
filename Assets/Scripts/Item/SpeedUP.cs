using UnityEngine;
using System.Collections;

public class SpeedUP : Item {

	public override void PickUp (GameObject character)
	{
		PlayerCommand command = character.GetComponent<PlayerCommand>();
//		if(command.speed < 6) {
//			command.speed++;
//			command.speedRotate++;
//		}
		command.addSpeed();
		GameObject.Find("Scripts").GetComponent<GamePlay>().AddLog(GamePlay.myPlayer.getName + " Add Speed");
	}
}

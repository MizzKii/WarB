using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public Animator anime;
	public AudioClip death;
	public bool walk = true;

	public void PlayAnime (string name, bool value)
	{
		if(GamePlay.IsSingle) {
			anime.SetBool(name, value);
			if((name == "IsWalk" || name == "IsBack") && value && walk) {
				audio.Play();
				walk = false;
				Invoke("nextWalk",0.55f);
			}
		}
		else
			networkView.RPC ("PlayRPCB",RPCMode.All,name,value);
	}
	
	public void PlayAnime (string name)
	{
		if(GamePlay.IsSingle) {
			anime.SetTrigger(name);
			if(name == "IsDeath") {
				audio.PlayOneShot(death);
			}
		}
		else
			networkView.RPC ("PlayRPCT",RPCMode.All,name);
	}

	private void nextWalk() {
		walk = true;
	}
	
	[RPC]private void PlayRPCB(string name, bool value)
	{
		anime.SetBool(name, value);
		if((name == "IsWalk" || name == "IsBack") && value && walk) {
			audio.Play();
			walk = false;
			Invoke("nextWalk",0.55f);
		}
	}
	
	[RPC]private void PlayRPCT(string name)
	{
		anime.SetTrigger(name);
		if(name == "IsDeath") {
			audio.PlayOneShot(death);
		}
	}
}

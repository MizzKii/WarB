using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	public float percent;
	public GameObject[] items;

	public void DestroyMe(){
		if(GamePlay.IsSingle) {
			if(Random.Range(0f, 100f) < percent)
			{
				int index = (int)Random.Range(0,items.Length);
				Instantiate(items[index], transform.position, items[index].transform.rotation);
			}
		}else if(NetworkControl.isServer){
			if(Random.Range(0f, 100f) < percent)
			{
				int index = (int)Random.Range(0,items.Length);
				Network.Instantiate(items[index], transform.position, items[index].transform.rotation,0);
			}
		}
		Destroy(gameObject);
	}
}

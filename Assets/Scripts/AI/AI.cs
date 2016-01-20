using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	[SerializeField]
	private int heart = 1;
	private bool IsDie = false;
	private SphereCollider thisCollider;

	public bool forward = false;
	public bool left = false;
	public bool right = false;

	private Direction stage = Direction.none;
	[SerializeField]
	private Animator _animator;
	[SerializeField]
	private AudioClip audioIdle;
	[SerializeField]
	private AudioClip audioWalk;
	[SerializeField]
	private AudioClip audioDeath;
	private AudioSource _audio;

	private PlayerCommand command;

	private GamePlay gamePlay = null;

	// Use this for initialization
	void Start () {
		command = GetComponent <PlayerCommand> ();
		thisCollider = GetComponent <SphereCollider> ();
		_audio = GetComponent<AudioSource> ();
		if(GamePlay.IsSingle)
		{
			heart = PlayerPrefs.GetInt("Difficulty",1);
		}
		//_audio.loop = true;
		//_audio.volume = 1f;
		//_audio.clip = audioIdle;
		_audio.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GamePlay.IsSingle || Network.isServer)
		{
			if(!IsDie)
			{
				if(transform.position.y < -1F)
					Death();
				else if(command.idle)
					RandomHop();
			}
		}
	}

	void Death(){
		IsDie = true;
		_animator.SetTrigger("IsDeath");
		_audio.volume = 0.5f;
		_audio.loop = false;
		_audio.clip = audioDeath;
		_audio.Play();
		GameObject.Find("Scripts").GetComponent<Setting>().RemoveAudioSource(audio);
		Destroy(gameObject,2);
		gamePlay.RemoveBot();
	}

	void RandomHop(){
		if(left && right && !forward)
		{
			stage = Direction.forward;
			command.Movement(stage);
		}
		else if(stage == Direction.left || stage == Direction.right)
		{
			if(!forward)
			{
				stage = Direction.forward;
				command.Movement(stage);
			}
			else if(stage == Direction.left)
				command.Movement(Direction.left);
			else if(stage == Direction.right)
				command.Movement(Direction.right);
		}
		else if(forward && !left && right)
		{
			stage = Direction.left;
			command.Movement(stage);
		}
		else if(forward && left && !right)
		{
			stage = Direction.right;
			command.Movement(stage);
		}else{// 3 yack
			if(forward)
			{
				int x = Random.Range(0,1);
				if(x > 0)
				{
					stage = Direction.right;
					command.Movement(stage);
				}
				else
				{
					stage = Direction.left;
					command.Movement(stage);
				}
			}else { // 4 yack
				int x = Random.Range(1,3);
				if(x == 1)
				{
					stage = Direction.left;
					command.Movement(stage);
				}
				else if(x == 3)
				{
					stage = Direction.right;
					command.Movement(stage);
				}
				else{
					stage = Direction.forward;
					command.Movement(stage);
				}
			}
		}
		if(stage == Direction.forward)
		{
			_audio.clip = audioWalk;
			_audio.Play();
		}
		else{
			_audio.clip = audioIdle;
			_audio.Play();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			if(thisCollider.bounds.Contains(other.gameObject.transform.position))
				command.Return();
		}
		else if(other.gameObject.tag == "Player")
		{
			if(thisCollider.bounds.Contains(other.gameObject.transform.position))
				other.SendMessage("DestroyMe");
		}
	}
	
	public void BombMe(Player player)
	{
		if(gamePlay == null)
			gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();
		player.point += 100;
		if (--heart < 1 && !IsDie) {
			Death ();
		} else {
			IsDie = true;
			_animator.SetTrigger("IsShock");
			Invoke("ReShock",1f);
		}
		if(GamePlay.IsSingle || Network.isServer){
			gamePlay.AddLog(player.getName + " Bomb Enemy");
			if(heart == 0)
				gamePlay.AddLog("Enemy's Death");
			else
				gamePlay.AddLog("Enemy Heart x"+heart);
			gamePlay.AddLog(player.getName + " Get 100P");
		}
	}

	private void ReShock() {
		IsDie = false;
	}
}

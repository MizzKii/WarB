using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private PlayerCommand command;
	private PlayerAnimation pa;
	public Animator forwardAnim, backwardAnim, leftAnim, rightAnim, BombAnim;
	//private GamePlay gamePlay;
	private float time;// alpha = 0;
	//private int dam = 0;
	private bool bombBt = false, forwardBt = false, backwardBt = false, leftBt = false, rightBt = false;// dramage = false;
	private MainBomb mainBomb;
	//private GameObject hit;
	public GameObject _camera;//, forwardGO, backwardGO, leftGO, rightGO, bombGO;
	public Texture2D blood;
	public GUISkin skin;

	public GameObject bomb;

	private bool moved = true;

	// Use this for initialization
	void Start () {
		_camera.SetActive(true);
		command = GetComponent<PlayerCommand>();
		mainBomb = GetComponent<MainBomb>();
		pa = GetComponent<PlayerAnimation>();
		//gamePlay = GameObject.Find("Scripts").GetComponent<GamePlay>();

		//hit = GameObject.Find ("CheckHit");
		//hit.SetActive(false);
		Transform controller = GameObject.Find("Controller").transform;
//		forwardGO = controller.GetChild(0).gameObject;
//		backwardGO = controller.GetChild(1).gameObject;
//		leftGO = controller.GetChild(2).gameObject;
//		rightGO = controller.GetChild(3).gameObject;
//		bombGO = controller.GetChild(4).gameObject;

		forwardAnim = controller.GetChild(0).GetComponent<Animator> ();
		backwardAnim = controller.GetChild(1).GetComponent<Animator> ();
		leftAnim = controller.GetChild(2).GetComponent<Animator> ();
		rightAnim = controller.GetChild(3).GetComponent<Animator> ();
		BombAnim = controller.GetChild(4).GetComponent<Animator> ();
	}

	void OnGUI()
	{ 
		Resolution.Android();
		GUI.skin = skin;

		Controller ();
		Bombard ();
		if(!bomb.activeSelf && GamePlay.myPlayer.bombNum > 0)
			bomb.SetActive(true);
	}

	public void Controller()
	{
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Controller  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		//forward
		if(GUI.RepeatButton(new Rect(145, Resolution.height-305, 100, 100), ""))
		{
			forwardAnim.SetBool("Pressed", true);
			pa.PlayAnime("IsWalk", true);
			command.Movement(Direction.forward);
			forwardBt = true;
			moved = true;
		}
		else if (Event.current.type == EventType.Repaint && forwardBt)
		{
			forwardAnim.SetBool("Pressed", false);
			pa.PlayAnime("IsWalk", false);
			forwardBt = false;
		}
		//backward
		if(GUI.RepeatButton(new Rect(145, Resolution.height-125, 100, 100), ""))
		{
			backwardAnim.SetBool("Pressed", true);
			pa.PlayAnime("IsBack", true);
			command.Movement(Direction.back);
			backwardBt = true;
			moved = true;
		}
		else if (Event.current.type == EventType.Repaint && backwardBt)
		{
			backwardAnim.SetBool("Pressed", false);
			pa.PlayAnime("IsBack", false);
			backwardBt = false;
		}
		//left
		if(GUI.RepeatButton(new Rect(20, Resolution.height-215, 100, 100), ""))
		{
			leftAnim.SetBool("Pressed", true);
			pa.PlayAnime("IsLeft", true);
			command.Movement(Direction.left);
			leftBt = true;
		}
		else if (Event.current.type == EventType.Repaint && leftBt)
		{
			leftAnim.SetBool("Pressed", false);
			pa.PlayAnime("IsLeft", false);
			leftBt = false;
		}
		//right
		if(GUI.RepeatButton(new Rect(265, Resolution.height-215, 100, 100), ""))
		{
			rightAnim.SetBool("Pressed", true);
			pa.PlayAnime("IsRight", true);
			command.Movement(Direction.right);
			rightBt = true;
		}
		else if (Event.current.type == EventType.Repaint && rightBt)
		{
			rightAnim.SetBool("Pressed", false);
			pa.PlayAnime("IsRight", false);
			rightBt = false;
		}
	}

	public void Bombard()
	{
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Bomb  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		//bomb
		if(GUI.RepeatButton(new Rect(Resolution.width-220, Resolution.height-250, 200, 200), ""))
		{
			BombAnim.SetBool("Pressed", true);
			time += Time.fixedDeltaTime;
			bombBt = true;
		}
		else if (Event.current.type == EventType.Repaint && bombBt)
		{
			BombAnim.SetBool("Pressed", false);
			if(GamePlay.myPlayer.bombNum > 0 && bombBt)
			{
				//Debug.Log("T:"+time);
				if(time < 0.5f)
				{
					if(moved) {
						pa.PlayAnime("IsPlace");
						mainBomb.Bomb();
						moved = false;
					}
				}
				else
				{
					pa.PlayAnime("IsThrow");
					mainBomb.checkBound.SetActive(true);
					mainBomb.checkBound.GetComponent<CheckBound>().Reset(transform.position,command.myCompass);
				}
				time = 0;
				GamePlay.myPlayer.bombNum--;
				if(GamePlay.myPlayer.bombNum < 1)
					bomb.SetActive(false);
			}
			bombBt = false;
		}
	}

}

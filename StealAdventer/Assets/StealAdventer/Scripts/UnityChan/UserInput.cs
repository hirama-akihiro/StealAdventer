using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class UserInput : SingletonMonoBehavior<UserInput>
{
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(CrossPlatformInputManager.GetAxis("Horizontal"));
		Debug.Log(CrossPlatformInputManager.GetButtonDown("Jump"));
	}

	public bool PressAnyKey
	{
		get { return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetMouseButtonDown(0); }
	}
	public bool UnityChanLeftMove { get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0 || CrossPlatformInputManager.GetAxis("Horizontal") < 0 ; } }
	public bool UnityChanRightMove { get { return Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0 || CrossPlatformInputManager.GetAxis("Horizontal") > 0; } }
	public bool UnityChanJump { get { return Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button3) || CrossPlatformInputManager.GetButtonDown("Jump"); } }
	public bool UnityChanStealAttack { get { return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button0) || CrossPlatformInputManager.GetButtonDown("StealHand"); } }
	public bool UnityChanSkillAttack { get { return Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button2) || CrossPlatformInputManager.GetButtonDown("SkillAttack"); } }
	public bool UnityChanSkillAttackEnd { get { return Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Joystick1Button2) || CrossPlatformInputManager.GetButtonUp("SkillAttack"); } }
	public bool UnityChanUpperAttack { get { return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button1); } }
}

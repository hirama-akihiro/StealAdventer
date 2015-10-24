﻿using UnityEngine;
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
	}

	public bool PressAnyKey
	{
		get { return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetMouseButtonDown(0); }
	}
	public bool UnityChanLeftMove { get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0; } }
	public bool UnityChanRightMove { get { return Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0; } }
	public bool UnityChanJump { get { return Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button3); } }
	public bool UnityChanStealAttack { get { return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button0); } }
	public bool UnityChanSkillAttack { get { return Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button2); } }
	public bool UnityChanSkillAttackEnd { get { return Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Joystick1Button2); } }
	public bool UnityChanUpperAttack { get { return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button1); } }
}
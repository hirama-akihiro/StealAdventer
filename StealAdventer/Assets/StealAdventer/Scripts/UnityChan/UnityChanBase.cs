using UnityEngine;
using System.Collections;
using System;

public class UnityChanBase : MonoBehaviour {

	protected event Action OnNoMoveEvent;
	protected event Action OnMoveEvent;
	protected event Action OnJumpEvent;
	protected event Action OnSkillAttackEvent;
	protected event Action OnSkillAttackEndEvent;
	protected event Action OnStealHandEvent;
	protected event Action OnUpperHandEvent;

	private Transform cashedTransform;
	public Transform CashedTransform
	{
		get
		{
			if (cashedTransform == null) { cashedTransform = GetComponent<Transform>(); }
			return cashedTransform;
		}
	}

	// Use this for initialization
	virtual protected void Awake () {
		OnNoMoveEvent += OnNoMove;
		OnMoveEvent += OnMove;
		OnJumpEvent += OnJump;
		OnSkillAttackEvent += OnSkillAttack;
		OnSkillAttackEndEvent += OnSkillAttackEnd;
		OnStealHandEvent += OnStealHand;
		OnUpperHandEvent += OnUpperHand;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (!UserInput.Instance.UnityChanLeftMove && !UserInput.Instance.UnityChanRightMove && OnNoMoveEvent != null) { OnNoMoveEvent(); }
		if (UserInput.Instance.UnityChanLeftMove || UserInput.Instance.UnityChanRightMove && OnMoveEvent != null) { OnMoveEvent(); }
		if (UserInput.Instance.UnityChanJump && OnJumpEvent != null) { OnJumpEvent(); }
		if (UserInput.Instance.UnityChanSkillAttack && OnSkillAttackEvent != null) { OnSkillAttackEvent(); }
		if (UserInput.Instance.UnityChanSkillAttackEnd && OnSkillAttackEndEvent != null) { OnSkillAttackEndEvent(); }
		if (UserInput.Instance.UnityChanStealAttack && OnStealHandEvent != null) { OnStealHandEvent(); }
		if (UserInput.Instance.UnityChanUpperAttack && OnUpperHandEvent != null) { OnUpperHandEvent(); }
	}

	virtual protected void OnNoMove() { }
	virtual protected void OnMove() { }
	virtual protected void OnJump() { }
	virtual protected void OnSkillAttack() { }
	virtual protected void OnSkillAttackEnd() { }
	virtual protected void OnStealHand() { }
	virtual protected void OnUpperHand() { }
}

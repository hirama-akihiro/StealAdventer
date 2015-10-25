using UnityEngine;
using System.Collections;
using System;

public class Player : Character {

	protected event Action OnNoMoveEvent;
	protected event Action OnMoveEvent;
	protected event Action OnJumpEvent;
	protected event Action OnSkillAttackEvent;
	protected event Action OnSkillAttackEndEvent;
	protected event Action OnStealHandEvent;
	protected event Action OnUpperHandEvent;

	// Use this for initialization
	protected override void Awake () {
		OnNoMoveEvent += OnNoMove;
		OnMoveEvent += OnMove;
		OnJumpEvent += OnJump;
		OnSkillAttackEvent += OnSkillAttack;
		OnSkillAttackEndEvent += OnSkillAttackEnd;
		OnStealHandEvent += OnStealHand;
		OnUpperHandEvent += OnUpperHand;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (!UserInput.Instance.UnityChanLeftMove && !UserInput.Instance.UnityChanRightMove && OnNoMoveEvent != null) { OnNoMoveEvent(); }
		if (UserInput.Instance.UnityChanLeftMove || UserInput.Instance.UnityChanRightMove && OnMoveEvent != null) { OnMoveEvent(); }
		if (UserInput.Instance.UnityChanJump && OnJumpEvent != null) { OnJumpEvent(); }
		if (UserInput.Instance.UnityChanSkillAttack && OnSkillAttackEvent != null) { OnSkillAttackEvent(); }
		if (UserInput.Instance.UnityChanSkillAttackEnd && OnSkillAttackEndEvent != null) { OnSkillAttackEndEvent(); }
		if (UserInput.Instance.UnityChanStealAttack && OnStealHandEvent != null) { OnStealHandEvent(); }
		if (UserInput.Instance.UnityChanUpperAttack && OnUpperHandEvent != null) { OnUpperHandEvent(); }
	}

	/// <summary>
	/// 無敵時間中に点滅させるコルーチン
	/// </summary>
	/// <returns></returns>
	private IEnumerator MutekiFlashing()
	{
		gameObject.layer = LayerMask.NameToLayer(LayerNames.MutekiPlayer);
		while (mutekiTime > 0)
		{
			SetRendererEnable(true);
			yield return new WaitForSeconds(blinkerInterval);
			SetRendererEnable(false);
			yield return new WaitForSeconds(blinkerInterval);
			mutekiTime -= blinkerInterval * 2;
		}

		SetRendererEnable(true);
		gameObject.layer = LayerMask.NameToLayer(LayerNames.Player);
	}

	virtual protected void OnNoMove() { }
	virtual protected void OnMove() { }
	virtual protected void OnJump() { }
	virtual protected void OnSkillAttack() { }
	virtual protected void OnSkillAttackEnd() { }
	virtual protected void OnStealHand() { }
	virtual protected void OnUpperHand() { }
}

using UnityEngine;
using System.Collections;

public class ZipperMouse_hirama: MonoBehaviour
{
	#region Field
	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;

	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4, Explosion = 5 };
		
	/// <summary>
	/// RigidBody
	/// </summary>
	private Rigidbody myRigidbody;
	
	/// <summary>
	/// Animation
	/// </summary>
	private Animation myAnimation;
	#endregion

	IEnumerator Start()
	{
		status = GetComponent<CharacterStatus>();
		status.NowAngle = CharacterStatus.CharacterAngle.Left;
		status.NowState = (int)CharacterState.Moving;
		myRigidbody = GetComponent<Rigidbody>();
		myAnimation = GetComponent<Animation>();

		while (true)
		{
			// 3秒待機
			yield return new WaitForSeconds(3.0f);
			Instantiate(status.skillObject, new Vector3(transform.position.x + 2 * (int)status.NowAngle, transform.position.y + 2, transform.position.z), transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	}
	
	void OnTriggerEnter(Collider c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		if (layerName == "ReflectionArea") {
			transform.Rotate (0, 180, 0);
			if (status.NowAngle == CharacterStatus.CharacterAngle.Left)
				status.NowAngle = CharacterStatus.CharacterAngle.Right;
			else
				status.NowAngle = CharacterStatus.CharacterAngle.Left;
		}
	}
}

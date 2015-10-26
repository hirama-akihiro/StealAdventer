using UnityEngine;
using System.Collections;

public class LegCollider : MonoBehaviour {

	private UnityChanController unityController;
	private Minotaur minotaur;

	// Use this for initialization
	void Start () {
		unityController = GameObject.Find("SDUnityChan").GetComponent<UnityChanController>();
		minotaur = GameObject.Find("Minotaur").GetComponent<Minotaur>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter(Collider collider)
	{
		// 地面設置時ジャンプ可能にする
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if (layerName == LayerNames.Stage_FloorObject || layerName == LayerNames.Stage_BossFloorObject)
		{
			unityController.IsJump = false;
			unityController.SetBool("Jump", false);
		}
		if(layerName == LayerNames.Stage_BossFloorObject && !GameEnder.I.IsFinish)
		{
			 // ターゲットカメラの位置を固定
			TargetCamera.I.isCompliance = false;
			TargetCamera.I.transform.position = new Vector3(200, 2, -15);

			minotaur.enabled = true;
			GameObject.Find("DataCanvas").transform.FindChild("HP(Enemy)").GetComponent<HPGauge>().GaugeDisplay(GameObject.Find("Minotaur"));

			if (AudioManager.I.IsPlaying("Fight"))
			{
				AudioManager.I.StopAudio("Fight");
				AudioManager.I.PlayAudio("bossBattle", AudioManager.PlayMode.Repeat);
			}
		}
	}

	public void GameEnd() { enabled = false; }
}

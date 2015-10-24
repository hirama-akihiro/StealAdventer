using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameEnder : SingletonMonoBehavior<GameEnder> {

	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	public GameObject playerObject;

	/// <summary>
	/// ボスオブジェクト
	/// </summary>
	public GameObject bossObject;

	/// <summary>
	/// カメラ
	/// </summary>
	public GameObject targetCamera;

    /// <summary>
	/// ステータスなどのデータ表示キャンバス
	/// </summary>
    public GameObject dataCanvas; 

    /// <summary>
	/// リザルト画面表示用キャンバス
	/// </summary>
    public GameObject resultCanvas;

	private bool isFinish;
	public bool isGameOver;
	public bool isGameClear;

	public GameObject resultMessaeText;
	public GameObject pressMessageText;

	// Use this for initialization
	void Start () {
		//soundManager = new SoundManager ();
		isFinish = false;
		isGameOver = false;
		isGameClear = false;
	}
	
	// Update is called once per frame
	void Update () {
		// ゲームオーバ
		if(isGameOver&& !isFinish)
		{
			targetCamera.GetComponent<TargetCamera>().enabled = false;
			targetCamera.transform.position = new Vector3(200, 1, -3);
			playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			playerObject.transform.position = new Vector3(200, 0, 0);
			playerObject.transform.Rotate(0, (int)playerObject.GetComponent<Character>().NowAngle * 90, 0);
			playerObject.GetComponent<Animator>().speed = 1.0f;
			playerObject.GetComponent<UnityChanController>().SetBool("GameOver", true);
			playerObject.GetComponent<UnityChanController>().IsControllable = false;
			isFinish = true;
			playerObject.BroadcastMessage("GameEnd");
			bossObject.SendMessage("GameEnd");
            SEManager.Instance.PlayAudio("GameOverVoice");

			dataCanvas.SetActive(false);

			/* リザルト画面表示 */
			resultCanvas.GetComponent<ResultCanvasScript>().ShowResult();
		}

		// ゲームクリア
		if(bossObject.GetComponent<Character>().IsDestroy && !isFinish)
		{
            ScoreManager.Instance.GameClear();

			//resultMessaeText.GetComponent<Text>().text = "Game Clear !!";
			//pressMessageText.GetComponent<Text>().text = "Press Space Key";

			targetCamera.GetComponent<TargetCamera>().enabled = false;
			targetCamera.transform.position = new Vector3(200, 1, -3);
			playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			playerObject.transform.position = new Vector3(200, 0, 0);
			playerObject.transform.Rotate(0, (int)playerObject.GetComponent<Character>().NowAngle * 90, 0);
			playerObject.GetComponent<Animator>().speed = 1.0f;
			// 今のアニメーションを強制的に終わらせる
			playerObject.GetComponent<UnityChanController>().SetBool("GameClear", true);
			playerObject.GetComponent<UnityChanController>().IsControllable = false;
			isFinish = true;
			playerObject.BroadcastMessage("GameEnd");
			bossObject.SendMessage("GameEnd");
            SEManager.Instance.PlayAudio("GameClearVoice");

			dataCanvas.SetActive(false);

			/* リザルト画面表示 */
			resultCanvas.GetComponent<ResultCanvasScript>().ShowResult();
		}

		if(isFinish)
		{
			if (UserInput.Instance.PressAnyKey && !resultCanvas.activeSelf)
			{
                resultMessaeText.GetComponent<Text>().text = "";
                pressMessageText.GetComponent<Text>().text = "";
                //dataCanvas.SetActive(false);

                /* リザルト画面表示 */
                //resultCanvas.GetComponent<ResultCanvasScript>().ShowResult();

                //GameObject.Find("ElapsedTimeText").GetComponent<ElapsedTimeScript>().GameEnd();
            }
			else if (UserInput.Instance.PressAnyKey && resultCanvas.GetComponent<ResultCanvasScript>().IsDisplayed)
            {
                resultCanvas.GetComponent<ResultCanvasScript>().IsDisplayed = false;
                AudioManager.Instance.StopAudio();
				//FadeManager.Instance.LoadLevel("TitleScene", 0.5f);
                Application.LoadLevel("TitleScene");
            }

			Animator animator = playerObject.GetComponent<Animator>();
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GameClear"))
			{
				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f) { animator.speed = 0.0f; }
				return;
			}

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GameOver"))
			{
				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f) { animator.speed = 0.0f; }
				return;
			}
		}
	}

	public bool IsFinish { get { return isFinish; } }
}

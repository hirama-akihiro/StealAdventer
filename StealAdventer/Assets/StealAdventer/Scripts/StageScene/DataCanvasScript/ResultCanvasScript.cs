using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ResultCanvasScript : MonoBehaviour {

    /// <summary>
    /// テキスト表示間隔
    /// </summary>
    public float displayInterval;

    /// <summary>
    /// Sランクの必要得点
    /// </summary>
    public int rankSBorder;

    /// <summary>
    /// Aランクの必要得点
    /// </summary>
    public int rankABorder;

    /// <summary>
    /// Bランクの必要得点
    /// </summary>
    public int rankBBorder;

    /// <summary>
    /// Cランクの必要得点
    /// </summary>
    public int rankCBorder;

    /// <summary>
    /// クリアタイム表示テキスト
    /// </summary>
    public Text clearTimeText;

    /// <summary>
    /// クリアタイムポイント表示テキスト
    /// </summary>
    public Text clearTimePointText;

    /// <summary>
    /// 取得コイン数表示テキスト
    /// </summary>
    public Text coinsText;

    /// <summary>
    /// 取得コイン数ポイント表示テキスト
    /// </summary>
    public Text coinsPointText;

    /// <summary>
    /// 被ダメージ量表示テキスト
    /// </summary>
    public Text damagesText;

    /// <summary>
    /// 被ダメージ量ポイント表示テキスト
    /// </summary>
    public Text damagesPointText;

    /// <summary>
    /// 敵撃破数表示テキスト
    /// </summary>
    public Text defeatEnemiesText;

    /// <summary>
    /// 敵撃破数表示テキスト
    /// </summary>
    public Text defeatEnemiesPointText;

    /// <summary>
    /// 死亡回数表示テキスト
    /// </summary>
    public Text playerDeadTimesText;

    /// <summary>
    /// 死亡回数ポイント表示テキスト
    /// </summary>
    public Text playerDeadTimesPointText;

    /// <summary>
    /// 総得点表示テキスト
    /// </summary>
    public Text totalPointText;

    /// <summary>
    /// 最終評価表示テキスト
    /// </summary>
    public Text finalEvaluationText;

    /// <summary>
    /// 「PressSpaceKey」表示テキスト
    /// </summary>
    public Text PressSpaceKeyText;

    /// <summary>
    /// スコアが表示済みかどうか
    /// </summary>
    public bool IsDisplayed { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private IEnumerator ShowResultCoroutine()
    {
		Debug.Log("Result Canvas Is Show");
		//yield return null;
		yield return new WaitForSeconds(displayInterval);
        clearTimeText.transform.parent.gameObject.SetActive(true);
        clearTimeText.text = new DateTime(0).Add(ScoreManager.I.ClearTime).ToString("mm:ss");
        clearTimePointText.text = ScoreManager.I.GetClearTimePoint().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        yield return new WaitForSeconds(displayInterval);
        coinsText.transform.parent.gameObject.SetActive(true);
        coinsText.text = ScoreManager.I.Coins.ToString();
        coinsPointText.text = ScoreManager.I.GetCoinsPoint().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        yield return new WaitForSeconds(displayInterval);
        damagesText.transform.parent.gameObject.SetActive(true);
        damagesText.text = ScoreManager.I.DamagedScore.ToString();
        damagesPointText.text = ScoreManager.I.GetDamagesPoint().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        yield return new WaitForSeconds(displayInterval);
        defeatEnemiesText.transform.parent.gameObject.SetActive(true);
        defeatEnemiesText.text = ScoreManager.I.DefeatEnemyScore.ToString();
        defeatEnemiesPointText.text = ScoreManager.I.GetDefeatEnemiesPoint().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        yield return new WaitForSeconds(displayInterval);
        playerDeadTimesText.transform.parent.gameObject.SetActive(true);
        playerDeadTimesText.text = ScoreManager.I.PlayerDeadTimes.ToString();
        playerDeadTimesPointText.text = ScoreManager.I.GetPlayerDeadTimesPoint().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        yield return new WaitForSeconds(displayInterval);
        totalPointText.transform.parent.parent.gameObject.SetActive(true);
        transform.FindChild("Line").gameObject.SetActive(true);
        totalPointText.text = ScoreManager.I.GetFinalScore().ToString() + "P";
        AudioManager.I.PlayAudio("SETextDisplay");

        char rank;

        int finalScore = ScoreManager.I.GetFinalScore();
        if (finalScore >= rankSBorder)
            rank = 'S';
        else if (finalScore >= rankABorder)
            rank = 'A';
        else if (finalScore >= rankBBorder)
            rank = 'B';
        else if (finalScore >= rankCBorder)
            rank = 'C';
        else
            rank = 'D';

        yield return new WaitForSeconds(displayInterval * 3);
        finalEvaluationText.gameObject.SetActive(true);
        finalEvaluationText.text = "最終評価:" + rank;
        AudioManager.I.PlayAudio("BGMResult");

        while (AudioManager.I.IsPlaying("Result"))
            yield return null;

        PressSpaceKeyText.gameObject.SetActive(true);

        IsDisplayed = true;
    }

    public void ShowResult()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowResultCoroutine());
    }

    private void ShowResultNormal()
    {
        clearTimeText.transform.parent.gameObject.SetActive(true);
        clearTimeText.text = new DateTime(0).Add(ScoreManager.I.ClearTime).ToString("mm:ss");
        clearTimePointText.text = ScoreManager.I.GetClearTimePoint().ToString();

        coinsText.transform.parent.gameObject.SetActive(true);
        coinsText.text = ScoreManager.I.Coins.ToString();
        coinsPointText.text = ScoreManager.I.GetCoinsPoint().ToString();

        damagesText.transform.parent.gameObject.SetActive(true);
        damagesText.text = ScoreManager.I.DamagedScore.ToString();
        damagesPointText.text = ScoreManager.I.GetDamagesPoint().ToString();

        defeatEnemiesText.transform.parent.gameObject.SetActive(true);
        defeatEnemiesText.text = ScoreManager.I.DefeatEnemyScore.ToString();
        defeatEnemiesPointText.text = ScoreManager.I.GetDefeatEnemiesPoint().ToString();

        playerDeadTimesText.transform.parent.gameObject.SetActive(true);
        playerDeadTimesText.text = ScoreManager.I.PlayerDeadTimes.ToString();
        playerDeadTimesPointText.text = ScoreManager.I.GetPlayerDeadTimesPoint().ToString();

        totalPointText.transform.parent.parent.gameObject.SetActive(true);
        totalPointText.text = ScoreManager.I.GetFinalScore().ToString();

        char rank;

        int finalScore = ScoreManager.I.GetFinalScore();
        if (finalScore >= rankSBorder)
            rank = 'S';
        else if (finalScore >= rankABorder)
            rank = 'A';
        else if (finalScore >= rankBBorder)
            rank = 'B';
        else if (finalScore >= rankCBorder)
            rank = 'C';
        else
            rank = 'D';

        finalEvaluationText.gameObject.SetActive(true);
        finalEvaluationText.text = "最終評価:" + rank;
    }
}

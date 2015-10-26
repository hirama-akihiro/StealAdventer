using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScoreManager : SingletonMonoBehavior<ScoreManager> {

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 最高スコアを取るためのクリアタイム
    /// </summary>
    public float fastestClearTime;

    /// <summary>
    /// コインの取得枚数の得点への補正倍率
    /// </summary>
    public float coinMagnification;

    /// <summary>
    /// 敵撃破数の得点への補正倍率
    /// </summary>
    public float dEbemyMagnification;

    /// <summary>
    /// 総ダメージ量の得点への補正倍率
    /// </summary>
    public float damagesMagnification;

    /// <summary>
    /// 最高スコアを取るために受けてもいい最大ダメージ
    /// </summary>
    public float allowDamage;

    /// <summary>
    /// 死亡回数の得点への補正倍率
    /// </summary>
    public float playerDeadMagnification;

    /// <summary>
    /// 所持コイン数
    /// </summary>
    public int Coins { get; private set; }

    /// <summary>
    /// エネミー撃破数
    /// </summary>
    public int DefeatEnemyScore { get; private set; }

    /// <summary>
    /// 総被ダメージ量
    /// </summary>
    public int DamagedScore { get; private set; }

    /// <summary>
    /// クリアタイムパラメータ
    /// </summary>
    public TimeSpan ClearTime { get; private set; }

    /// <summary>
    /// プレイヤー死亡回数
    /// </summary>
    public int PlayerDeadTimes { get; private set; }

    /// <summary>
    /// ゲームスタート時の時刻
    /// </summary>
    private DateTime startTime;

    /// <summary>
    /// クリアタイムの最大得点
    /// </summary>
    public int clearTimePointMax;

    /// <summary>
    /// コインの最大得点
    /// </summary>
    public int coinsPointMax;

    /// <summary>
    /// 被ダメージ量の最大得点
    /// </summary>
    public int damagesPointMax;

    /// <summary>
    /// 敵撃破数の最大得点
    /// </summary>
    public int defeatEnemiesPointMax;

    /// <summary>
    /// プレイヤー死亡回数の最大得点
    /// </summary>
    public int playerDeadTimesPointMax;

    protected override void Awake()
    {
		base.Awake();
        if (this != I)
        {
            Destroy(this);
            return;
        }
        //DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start () {
        GameStart();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int aScore)
    {
        Score = Score + aScore;
    }
    */

    /// <summary>
    /// エネミー撃破数加算
    /// </summary>
    public void DefeatEnemy()
    {
        DefeatEnemyScore = DefeatEnemyScore + 1;
    }

    /// <summary>
    /// 被ダメージ量加算
    /// </summary>
    public void AddDamagedScore(int damage)
    {
        DamagedScore = DamagedScore + damage;
    }

    /// <summary>
    /// コイン取得数加算(1枚固定)
    /// </summary>
    public void AddCoin(int coinCt = 1)
    {
		Coins += coinCt;
    }

    /// <summary>
    /// プレイヤー死亡回数加算
    /// </summary>
    public void PlayerDead()
    {
        PlayerDeadTimes++;
    }

    /// <summary>
    /// クリアタイムによるポイントを取得
    /// </summary>
    public int GetClearTimePoint()
    {
        if (ClearTime.Seconds == 0)
            return 0;
        return (int)Mathf.Min(clearTimePointMax, 1.0f / (float)(ClearTime.TotalSeconds) * fastestClearTime * clearTimePointMax);
    }

    /// <summary>
    /// コインによるポイントを取得
    /// </summary>
    public int GetCoinsPoint()
	{
        return (int)Mathf.Min(coinsPointMax, Coins * coinMagnification);
    }

    /// <summary>
    /// 被ダメージ量によるポイントを取得
    /// </summary>
    public int GetDamagesPoint()
    {
        return (int)Mathf.Max(0.0f, damagesPointMax + Mathf.Min(allowDamage - (float)DamagedScore, 0.0f) * damagesMagnification * damagesPointMax);
    }

    /// <summary>
    /// 敵撃破数によるポイントを取得
    /// </summary>
    public int GetDefeatEnemiesPoint()
    {
        return (int)Mathf.Min(defeatEnemiesPointMax, DefeatEnemyScore * dEbemyMagnification);
    }

    /// <summary>
    /// 死亡回数によるポイントを取得
    /// </summary>
    public int GetPlayerDeadTimesPoint()
    {
        return (int)Mathf.Max(0.0f, playerDeadTimesPointMax - PlayerDeadTimes * playerDeadMagnification);
    }

    /// <summary>
    /// 最終スコア取得
    /// </summary>
    public int GetFinalScore()
    {
        return GetClearTimePoint() + GetCoinsPoint() + GetDamagesPoint() + GetDefeatEnemiesPoint() + GetPlayerDeadTimesPoint();
    }

    /// <summary>
    /// ゲーム開始からの経過時間を取得
    /// </summary>
    public TimeSpan GetElapsedTime()
    {
        return DateTime.Now - startTime;
    }

    /// <summary>
    /// ゲーム開始時のメソッド
    /// </summary>
    public void GameStart()
    { 
        enabled = true;
		//Score = 0;
        ClearTime = new TimeSpan(0);
        DamagedScore = 0;
        DefeatEnemyScore = 0;
        Coins = 0;
        PlayerDeadTimes = 0;
        startTime = DateTime.Now;
    }

    /// <summary>
    /// ゲームクリア時のメソッド
    /// </summary>
    public void GameClear()
    {
        ClearTime = DateTime.Now - startTime;
    }

    /// <summary>
    /// ゲーム終了時のメソッド
    /// </summary>
    public void GameEnd()
    {
        enabled = false;
        //ClearTime = DateTime.Now - startTime;
    }
}

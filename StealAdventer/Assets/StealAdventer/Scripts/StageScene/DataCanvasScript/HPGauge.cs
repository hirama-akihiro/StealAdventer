using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPGauge : MonoBehaviour {

    /// <summary>
    /// HPを表示する対象のゲームオブジェクト
    /// </summary>
    public GameObject character;

    /// <summary>
    /// HPゲージのスライダー
    /// </summary>
    public Slider gauge;

    /// <summary>
    /// HPゲージの変化にかける時間
    /// </summary>
    public float gaugeChangeTime;

    /// <summary>
    /// HPゲージ変化開始からの時間
    /// </summary>
    private float timeSinceBeginHPChanged;

    /// <summary>
    /// HPゲージが変化中かどうか
    /// </summary>
    private bool hpChanging;

    /// <summary>
    /// HP変化時の1フレームごと変化幅
    /// </summary>
    private float hpChangeValue;

    /// <summary>
    /// HP変化開始時のキャラクターのHP
    /// </summary>
    private int hpBeforeChange;

    // Use this for initialization
    void Start () {
        if (gameObject.activeInHierarchy && character != null)
        {
            gauge.maxValue = character.GetComponent<Character>().maxHP;
        }
        hpBeforeChange = 0;
        gauge.value = 0.0f;
        gauge.minValue = 0.0f;
        timeSinceBeginHPChanged = 0.0f;
        hpChangeValue = 0.0f;
        hpChanging = false;
        
        //HPChange();
    }
	
	// Update is called once per frame
	void Update () {
        HPChange();
    }

    private void HPChange()
    {
        if (gameObject.activeInHierarchy && character != null && gauge.value != character.GetComponent<Character>().nowHP && hpBeforeChange != character.GetComponent<Character>().nowHP && !hpChanging)
        {
            hpChanging = true;
            hpBeforeChange = character.GetComponent<Character>().nowHP;
            hpChangeValue = (gauge.value - hpBeforeChange) / gaugeChangeTime * Time.deltaTime;
        }

        if (hpChanging)
        {         
            gauge.value -= hpChangeValue;
            timeSinceBeginHPChanged += Time.deltaTime;
            //Debug.Log(gauge.value + " " + character.GetComponent<CharacterStatus>().nowHP);
            
            if (timeSinceBeginHPChanged >= gaugeChangeTime || Mathf.Abs(gauge.value - character.GetComponent<Character>().nowHP) < hpChangeValue)
            {
                hpChanging = false;
                timeSinceBeginHPChanged = 0.0f;
                hpBeforeChange = character.GetComponent<Character>().nowHP;
                gauge.value = character.GetComponent<Character>().nowHP;
            }
            
        }
    }

    /// <summary>
    /// HPを表示するキャラクターをセットしてHPゲージを表示
    /// </summary>
    public void GaugeDisplay(GameObject character)
    {
        gameObject.SetActive(true);

        this.character = character;
		gauge.maxValue = character.GetComponent<Character>().maxHP;
        gauge.minValue = 0.0f;
        hpBeforeChange = character.GetComponent<Character>().nowHP;
    }

    /// <summary>
    /// HPゲージを隠す
    /// </summary>
    public void GaugeHidden()
    {
        gameObject.SetActive(false);
    }
}

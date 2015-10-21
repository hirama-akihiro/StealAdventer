using UnityEngine;
using System.Collections;

public class leftRotateScript : MonoBehaviour {

    private Vector3 pos;
    private Rigidbody myRigidbody;

    private Vector3 rot;

	private int rotateCount = 0;
	public float ungle;

	public bool IsTrapOn{ get; set;}

    // Use this for initialization
    void Start()
    {
		IsTrapOn = false;
        pos = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
        rot = Vector3.zero;
    }

    // Update is called once per frame
    public void Update()
    {
		if (IsTrapOn) {
			Rotate ();
		}
    }

	/// <summary>
	/// 回転 
	/// </summary>
	public void Rotate()
	{
		//Debug.Log (rotateCount);
		if (rotateCount <= ungle) {
			transform.Rotate (new Vector3 (0, 0, -1), 1);
			rotateCount++;
		}
	}
}
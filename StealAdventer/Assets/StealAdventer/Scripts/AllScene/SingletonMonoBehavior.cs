using UnityEngine;
using System.Collections;

/// <summary>
/// Unity用Singleton基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>
	/// シングルトンStatic変数
	/// </summary>
	private static T instance;

	/// <summary>
	/// シングルトン変数
	/// </summary>
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				if (instance == null)
				{
					Debug.LogError(typeof(T) + " is Nothing");
				}
			}
			return instance;
		}
	}

	void OnDestroy()
	{
		if (instance == this) { instance = null; }
	}

	protected virtual void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if (this == Instance) { return this; }
		Destroy(this);
		return false;
	}

	static public bool IsValid() { return instance != null; }
}


using UnityEngine;
using System.Collections;

public class ParticleScaler : MonoBehaviour
{
	public float scale = 1;

	public void Awake()
	{
		var particles = GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].startSize *= scale;
			particles[i].startSpeed *= scale;
			particles[i].startRotation *= scale;
			particles[i].transform.localScale *= scale;

		}
	}
}
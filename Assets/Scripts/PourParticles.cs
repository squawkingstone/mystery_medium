using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourParticles : MonoBehaviour 
{

	[SerializeField] ParticleSystem particle;

	void Update()
	{
		float dot = Vector3.Dot(transform.up, Vector3.down);
		if (dot >= 0.0f && !particle.isPlaying)
		{
			particle.Play();
		} 
		if (dot < 0.0f && particle.isPlaying)
		{
			particle.Stop();
		}
	}

}

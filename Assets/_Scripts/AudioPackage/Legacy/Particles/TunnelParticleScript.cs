using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelParticleScript : MonoBehaviour {
	[Range(1,100)]
	public float BufferAmount;
	public Color Color1;
	public Color colorComplemntary;
	public GameObject cam;
	public bool BufferingMusicAvgAmp = true;
	ParticleSystem Ps;
	ParticleSystem.Particle[] Particles;//array that stores all particles.             
	Vector3[] OriginalPos;
	void Start () {
		Ps = GetComponent<ParticleSystem>();
		StartCoroutine(MusicBuffer());
	}

	private void Update()
	{
		float stepSpeed = (AudioPeer.AmplitudeAvgBuffer / 15) + 0.01f;
		var PSMain = Ps.main;
		PSMain.startColor = Color1; 
	}

	// Update is called once per frame
	private float MaxDistanceFromOrigin = 0;
	private float MaxPSDistance = 6;
	void LateUpdate () {
		if (Particles == null || Particles.Length < Ps.main.maxParticles)
		{
			Particles = new ParticleSystem.Particle[Ps.main.maxParticles];            
		}
		if(OriginalPos == null || OriginalPos.Length < Ps.main.maxParticles)
		{
			OriginalPos = new Vector3[Ps.main.maxParticles];
		}
		Ps.GetParticles(Particles);

		for (int i = 0; i < Particles.Length; i++)
		{
			Vector3 Ref = new Vector3(0, 0, Particles[i].position.z);
			float DistanceFromCent = Vector3.Distance(Particles[i].position, Ref);
			if (MaxDistanceFromOrigin < DistanceFromCent)
			{
				MaxDistanceFromOrigin = DistanceFromCent;
			}
			Color PC = Particles[i].color;
			//if (UnityEngine.Random.Range(0, 200) == 1)
			//{
			//    Debug.Log("DistanceFromCent= " + DistanceFromCent);
			//    Debug.Log("AlphaChannel = " + Mathf.Pow(((DistanceFromCent / MaxDistanceFromOrigin)+0.2f),4) * 255);
			//}
			Particles[i].color = new Color(PC.r, PC.g, PC.b, Mathf.Pow(((Mathf.Pow(DistanceFromCent,2) / MaxDistanceFromOrigin) + 0.15f), 4) * 255);
			if (Particles[i].position.z < cam.transform.position.z)
			{
				Particles[i].remainingLifetime = 0;
			}
			else
			{                
				if (Particles[i].startLifetime - Particles[i].remainingLifetime < 0.15f)
				{
					OriginalPos[i] = Particles[i].position;
					//Finds if the Max Distance of the particle system is greater than current max distance;
					if (Particles[i].position.z - cam.transform.position.z > MaxPSDistance)
					{
						MaxPSDistance = Particles[i].position.z - cam.transform.position.z;
					}
				}
				else
				{
					float Distance = Particles[i].position.z - cam.transform.position.z;
					float DistancePercent = Distance / MaxPSDistance;
					Vector3 tempRef = new Vector3(0,0,0);
					try
					{//                       current particle pos   target position based off original * musicBuff(at the distance percent calculates diff bettween current and target then adds a buffer
						tempRef = new Vector3(Particles[i].position.x + (OriginalPos[i].x * MusicAVGBuffer[(int)(BufferAmount - (DistancePercent * BufferAmount))] - Particles[i].position.x) / 4
							, Particles[i].position.y + (OriginalPos[i].y * MusicAVGBuffer[(int)(BufferAmount - (DistancePercent * BufferAmount))] - Particles[i].position.y) / 4
							, Particles[i].position.z);
					}
					catch (Exception)
					{
						Debug.LogWarning("tempRefError: input" + (int)(BufferAmount - (DistancePercent * BufferAmount)));
						string x = "";
						foreach(float log in MusicAVGBuffer)
						{
							x = x + ", " + log.ToString();
						}
						Debug.Log("Input to List: " + (int)(DistancePercent * BufferAmount) +"DistancePercent:" + DistancePercent + "\n" + x + "\n List Size: " + MusicAVGBuffer.Count);
					}
					//Debug.Log(MusicAVGBuffer[(int)(DistancePercent * 200)] + "Input: " + (int)(DistancePercent * 200));
					Particles[i].position = tempRef;
				}
			}
		}
		Ps.SetParticles(Particles, Particles.Length);

	}

	List<float> MusicAVGBuffer = new List<float>();
	IEnumerator MusicBuffer()
	{
		for(int i = 0; i < 100; i++)
		{
			MusicAVGBuffer.Add(0);
		}
		while (BufferingMusicAvgAmp)
		{
			//MusicAVGBuffer.Insert(0, AudioPeer.audioBandRangeBuffer[0]*2 + 0.5f);
			MusicAVGBuffer.Insert(0, (Mathf.Pow(AudioPeer.audioBandRangeBuffer[0],2)/2 + AudioPeer.AmplitudeAvgBuffer) * 2 + 0.5f);
			try
			{
				MusicAVGBuffer.RemoveAt(101);
			}
			catch (Exception)
			{
				Debug.Log("ListNotFull");
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailParticleScript : MonoBehaviour {

    public int AudioBand;
    public float Multiplier;
    public float MinimumEmission;
    public Color CurrentColor;
    public Color ParticleColor1;
    public Color ParticleColor2;

    ParticleSystem Ps;
    ParticleSystem.Particle[] Particles;//array that stores all particles.             
    Vector3[] OriginalPos;
    // Use this for initialization
    void Start () {
        Ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Particles == null || Particles.Length < Ps.main.maxParticles)
        {
            Particles = new ParticleSystem.Particle[Ps.main.maxParticles];
        }

        Ps.GetParticles(Particles);
        Color finalColor;
        var Psmain = Ps.main;
        for(int i = 0; i < Particles.Length; i++)
        {
            CurrentColor = Particles[i].color;
            finalColor = CurrentColor * Mathf.LinearToGammaSpace(((AudioPeer.AmplitudeAvgBuffer) * Multiplier) + MinimumEmission);
            Particles[i].color = finalColor;
        }


        Ps.SetParticles(Particles, Particles.Length);
    }
}

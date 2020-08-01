using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToParticleAttractor : MonoBehaviour {
    public bool MusicSync;
    public bool AddAverageAmplitude;
    public float AvgAmplitudeMultiplier;
    [Range(0,10)]
    public float force = 10.0f;

    ParticleSystem Ps;
    ParticleSystem.Particle[] Particles;//array that stores all particles.
    // Use this for initialization
    void Start()
    {
        Ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Particle References
        if (Particles == null || Particles.Length < Ps.main.maxParticles)
        {
            Particles = new ParticleSystem.Particle[Ps.particleCount];
        }
        Ps.GetParticles(Particles);

        //Audio Calculationns

        var main = Ps.main;
        var PSNoise = Ps.noise;
        var PSshape = Ps.shape;
        if (MusicSync)
        {
            if (AddAverageAmplitude)
            {
                main.simulationSpeed = (AudioPeer.audioBandRangeBuffer[0]) + (AudioPeer.AmplitudeAvg * AvgAmplitudeMultiplier);
                AvgAmplitudeMultiplier = AudioPeer.audioBandRange[0];
            }
            else
            {
                //main.simulationSpeed = 0.75f + (AudioPeer.audioBandRangeBuffer[0] / 4);
                //PSNoise.strengthMultiplier = AudioPeer.AmplitudeAvg;
                force = 0.5f + AudioPeer.audioBandRange[0];                
            }
        }

        //physics calculations

        Vector3 TargetPos = new Vector3(0,0,0);
        float ForceDeltaTime = force * Time.deltaTime;
        for (int i = 0; i < Particles.Length; i++)
        {
            for (int j = 0; j < Particles.Length; j++)
            {
                TargetPos = TargetPos + (Particles[j].position - Particles[i].position); //adds up all the differences between the particles
            }
            TargetPos.Normalize();
            TargetPos = new Vector3(TargetPos.x, TargetPos.y, 0);

            //Vector3 DirectonToTarget = Vector3.Normalize(TargetPos - Particles[i].position);
            Vector3 DirectonToTarget = TargetPos - Particles[i].position;
            Vector3 seekForce = (DirectonToTarget * ForceDeltaTime);
            Particles[i].velocity += seekForce;

        }
        Ps.SetParticles(Particles, Particles.Length);
    }

    float MinMax(float _input, float _min, float _max)
    {
        //Debug.Log("input recived: "+ _input);
        float output = _input;
        if (_input > _min && _input < _max)
        {
           // Debug.Log("Input within range");
            return _input;
        }
        else {
            if (_input < _min)
            {
                //Debug.Log("smaller than min setting new output");
                output = _min;
                //Debug.Log("output: " + output);
                return output;                
            }
            if (_input > _max)
            {
                //Debug.Log("larger than max setting new output");
                output = _max;
                //Debug.Log("output: " + output);
                return output;
            }
        }
        //Debug.Log("this code is still executed");
        return output;
    }
}

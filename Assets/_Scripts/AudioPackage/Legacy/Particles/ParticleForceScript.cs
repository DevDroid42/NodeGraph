
using System.Collections;
using UnityEngine;

public class ParticleForceScript : MonoBehaviour {

    public Transform Target;
    public bool SyncSimSpeed;
    public bool AddAverageAmplitude;
    public float AvgAmplitudeMultiplier;
    public float force = 10.0f;

    ParticleSystem Ps;
    ParticleSystem.Particle[] Particles;//array that stores all particles.
    // Use this for initialization
    void Start () {
        StartCoroutine(RenableParticles());
        Ps = GetComponent<ParticleSystem>();

	}
	
	// Update is called once per frame
	void LateUpdate () {
        //Particle References
        if (Particles == null || Particles.Length < Ps.main.maxParticles)
        {
            Particles = new ParticleSystem.Particle[Ps.particleCount];
        }
        Ps.GetParticles(Particles);

        //Audio Calculationns

        var main = Ps.main;
        if (SyncSimSpeed)
        {
            if (AddAverageAmplitude)
            {
                main.simulationSpeed = (AudioPeer.audioBandRangeBuffer[0]) + (AudioPeer.AmplitudeAvg * AvgAmplitudeMultiplier);
                AvgAmplitudeMultiplier = AudioPeer.audioBandRange[0];
            }
            else
            {
                main.simulationSpeed = AudioPeer.audioBandRangeBuffer[0];
            }
        }

        //physics calculations
        Vector3 TargetPos = Target.position;
        float ForceDeltaTime = force * Time.deltaTime;
        for (int i = 0; i < Particles.Length; i++)
        {
            
            Vector3 DirectonToTarget = Vector3.Normalize(TargetPos - Particles[i].position);

            Vector3 seekForce = (DirectonToTarget * ForceDeltaTime);
            Particles[i].velocity += seekForce;         
            
        }
        Ps.SetParticles(Particles, Particles.Length);
	}

    IEnumerator RenableParticles()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    
}

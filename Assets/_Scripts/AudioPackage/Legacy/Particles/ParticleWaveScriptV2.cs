using UnityEngine;

public class ParticleWaveScriptV2 : MonoBehaviour
{
    public ParticleSystem PS;

    [Range(1,50)]
    public float OffsetBufferDivider = 1;
    public Vector3 OriginalPos;
    public bool SmoothBasedOnApmplitude;
    public bool colorBasedOnAmp;
    public Color MinColor;
    public Color MaxColor;
    private float[] Counter = new float[8];
    private float TargetOffset;
    private float CurrentOffset;
    // Use this for initialization
    void Start()
    {
        OriginalPos = gameObject.transform.position;
        
        //Time.timeScale = (1);
    }

    
    void Update()
    {

        //    TargetOffset = (Mathf.Sin(Counter[0] * AudioPeer.audioBandRange[0] +
        //Mathf.Sin(Counter[1] * (AudioPeer.audioBandRange[1] * 2) +
        //Mathf.Sin(Counter[2] * (AudioPeer.audioBandRange[2] * 4) +
        //Mathf.Sin(Counter[3] * (AudioPeer.audioBandRange[3] * 8) +
        //Mathf.Sin(Counter[4] * (AudioPeer.audioBandRange[4] * 16) +
        //Mathf.Sin(Counter[5] * (AudioPeer.audioBandRange[5] * 32) +
        //Mathf.Sin(Counter[6] * (AudioPeer.audioBandRange[6] * 64) +
        //Mathf.Sin(Counter[7] * (AudioPeer.audioBandRange[7] * 128))))))))))+OriginalPos.y;

                TargetOffset = (Mathf.Sin(Counter[0] * (AudioPeer.audioBandRange[0] *128) +
        Mathf.Sin(Counter[1] * (AudioPeer.audioBandRange[1] * 64) +
        Mathf.Sin(Counter[2] * (AudioPeer.audioBandRange[2] * 32) +
        Mathf.Sin(Counter[3] * (AudioPeer.audioBandRange[3] * 16) +
        Mathf.Sin(Counter[4] * (AudioPeer.audioBandRange[4] * 8) +
        Mathf.Sin(Counter[5] * (AudioPeer.audioBandRange[5] * 4) +
        Mathf.Sin(Counter[6] * (AudioPeer.audioBandRange[6] * 2) +
        Mathf.Sin(Counter[7] * (AudioPeer.audioBandRange[7] * 1)))))))))) + OriginalPos.y;

        TargetOffset *= AudioPeer.AmplitudeAvg + 1f;
        //WaveformBufferCode
        if (SmoothBasedOnApmplitude)
        {
            CurrentOffset += ((TargetOffset - transform.position.y) / (OffsetBufferDivider / AudioPeer.AmplitudeAvg));
        }
        else
        {
            CurrentOffset += ((TargetOffset - transform.position.y) / OffsetBufferDivider);
        }

        if (colorBasedOnAmp)
        {
            var psMain = PS.main;
            psMain.startColor = Color.Lerp(MinColor, MaxColor, AudioPeer.AmplitudeAvg);
        }
        
        gameObject.transform.position = new Vector3(OriginalPos.x, OriginalPos.y + CurrentOffset, OriginalPos.z);

        for (int i = 0; i < Counter.Length; i++)
        {
            Counter[i] += Mathf.Pow(i, 2) * Time.deltaTime;
        }
    }
}


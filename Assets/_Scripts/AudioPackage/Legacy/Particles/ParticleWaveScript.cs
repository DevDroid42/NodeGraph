
using UnityEngine;

public class ParticleWaveScript : MonoBehaviour {
    public ParticleSystem PS;
    public bool colorBasedOnAmp;
    public Color MinColor;
    public Color MaxColor;
    public Vector3 OriginalPos;
    private float Counter = -1000000;
	// Use this for initialization
	void Start () {
        OriginalPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        //gameObject.transform.position = OriginalPos + new Vector3(OriginalPos.x, Mathf.Sin(AudioPeer.audioBandRangeBuffer[0]), OriginalPos.z);
        gameObject.transform.position = new Vector3(OriginalPos.x,OriginalPos.y + Mathf.Sin(Counter)*AudioPeer.AmplitudeAvg, OriginalPos.z);
        Counter += AudioPeer.AmplitudeAvg * 40 * Time.deltaTime;

        if (colorBasedOnAmp)
        {
            var psMain = PS.main;
            psMain.startColor = Color.Lerp(MinColor, MaxColor, AudioPeer.AmplitudeAvg);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScaler : MonoBehaviour {
	public int band;
	public float _startScale, ScaleMultplier;
	public float BrightnessModifier;
	public float MinimumEmission;
	public bool useBuffer;
	public Color CC; //coustom color
	Material CubeMat;
	Renderer ObjectRender;
	// Use this for initialization
	void Start () {
		ObjectRender = gameObject.GetComponent<Renderer>();
		CubeMat = ObjectRender.material;
	}
	
	// Update is called once per frame
	void Update () {
		if (useBuffer)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.bandBuffer[band] * ScaleMultplier) + _startScale, transform.localScale.z);
		}
		else
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.freqBand[band] * ScaleMultplier) + _startScale, transform.localScale.z);
		}
		CubeMat.color = CC;
		Color finalColor = CubeMat.color * Mathf.LinearToGammaSpace(((AudioPeer.audioBandRangeBuffer[band]) * BrightnessModifier) + MinimumEmission);
		CubeMat.SetColor("_EmissionColor", finalColor);
		DynamicGI.SetEmissive(ObjectRender, finalColor);
	}
}

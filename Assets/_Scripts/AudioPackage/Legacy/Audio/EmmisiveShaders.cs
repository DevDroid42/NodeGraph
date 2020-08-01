using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisiveShaders : MonoBehaviour {

	public Material material;
	public GameObject[] emmissiveGiUpdatable;
	[Range(0,7)]
	public int band;
	public float MinimumEmission;
	public float Multiplier;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update() {
		//Color finalColor = material.color * Mathf.LinearToGammaSpace(((AudioPeer.audioBandRangeBuffer[band]) * Multiplier) + MinimumEmission);
		//material.SetColor("_EmissionColor", finalColor);
		for (int i = 0; i < emmissiveGiUpdatable.Length; i++)
		{
			//DynamicGI.SetEmissive(emmissiveGiUpdatable[i].GetComponent<Renderer>(), finalColor);
		}
	}
}

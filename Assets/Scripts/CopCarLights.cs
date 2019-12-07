using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopCarLights : MonoBehaviour
{
    // Start is called before the first frame update
    public Light blueLight;
    public GameObject blueMaterial;
    public Light redLight;
    public GameObject redMaterial;

    private Vector4 origBlue, origRed;

    void Start()
    {
        blueLight.enabled = false;
        redLight.enabled = false;

        origBlue = blueMaterial.GetComponent<Renderer>().material.GetVector("_EmissionColor");
        origRed = redMaterial.GetComponent<Renderer>().material.GetVector("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        float mod = Time.time % 1.2f;
        blueLight.enabled = (mod < 0.1) || (mod >= 0.2 && mod < 0.3) || (mod >= 0.4 && mod < 0.5);
        redLight.enabled = (mod >= 0.6 && mod < 0.7) || (mod >= 0.8 && mod < 0.9) || (mod >= 1.0 && mod < 1.1);

        blueMaterial.GetComponent<Renderer>().material.SetVector("_EmissionColor", origBlue * (blueLight.enabled ? 5.0f : 0.25f));
        redMaterial.GetComponent<Renderer>().material.SetVector("_EmissionColor", origRed * (redLight.enabled ? 5.0f : 0.25f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class flicker : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D lightToFlicker;
    public float variation;
    private float initIntensity;
    // Start is called before the first frame update

    void Start()
    {
        lightToFlicker = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        initIntensity = lightToFlicker.intensity;   
    }

    // Update is called once per frame
    void Update()
    {
        lightToFlicker.intensity = Random.Range(initIntensity - variation, initIntensity + variation);
    }
}

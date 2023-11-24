using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    bool triggerDestruction;
    AudioSource ownAudio;
    public bool chain;
    public AudioSource nextAudio;

    private void Start()
    {
        ownAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerDestruction)
        {
            if (!ownAudio.isPlaying)
            {
                if (chain)
                {
                    if (nextAudio != null)
                    {
                        nextAudio.Play();
                    }
                }
                Destroy(gameObject);
            }
        }
        else if (ownAudio.isPlaying)
        {
            triggerDestruction = true;
        }
    }
}

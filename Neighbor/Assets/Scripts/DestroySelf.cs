using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    bool triggerDestruction;
    AudioSource ownAudio;
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
                Destroy(gameObject);
            }
        }
        else if (ownAudio.isPlaying)
        {
            triggerDestruction = true;
        }
    }
}

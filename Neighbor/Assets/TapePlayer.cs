using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapePlayer : MonoBehaviour
{
    public AudioClip loadTape;
    public AudioClip startTape;
    public AudioClip endTape;

    List<AudioClip> returnClips = new List<AudioClip>();
    public List<InventoryItem> tapes; 

    public void PlayTape(string tapeName, AudioController audio)
    {
        AudioClip tapeClip = FindTape(tapeName);

        returnClips.Clear();
        returnClips.Add(loadTape);
        returnClips.Add(startTape);
        returnClips.Add(tapeClip);
        returnClips.Add(endTape);

        audio.PlayChainClips(returnClips);
    }

    AudioClip FindTape(string tapeToFind)
    {
        foreach(InventoryItem tapeCheck in tapes)
        {
            if (tapeCheck.itemName == tapeToFind)
            {
                return tapeCheck.GetComponent<TapeClip>().tapeRecording;
            }
        }

        return null;
    }
}

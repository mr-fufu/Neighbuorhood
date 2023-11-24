using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public GameObject audioPlayers;
    public GameObject singleAudioPlayers;

    public GameObject audioElement;
    public GameObject singleAudioElement;
    public GameObject chainAudioElement;

    private List<AudioClip> currentPlaying = new List<AudioClip>();
    private List<AudioSource> currentPlayers = new List<AudioSource>();

    private List<AudioClip> holdPlaying = new List<AudioClip>();
    private List<AudioSource> holdPlayers = new List<AudioSource>();

    public void ChangeClip(List<AudioClip> newAudio)
    {
        foreach (AudioSource player in currentPlayers)
        {
            if (!newAudio.Contains(player.clip))
            {
                StartCoroutine(FadeOut(player, 1f));

                holdPlaying.Add(player.clip);
                holdPlayers.Add(player);
            }
        }

        foreach (AudioSource player in holdPlayers)
        {
            currentPlayers.Remove(player);
            currentPlaying.Remove(player.clip);
        }

        holdPlaying.Clear();
        holdPlayers.Clear();

        foreach (AudioClip clip in newAudio)
        {
            if (!currentPlaying.Contains(clip))
            {
                GameObject newAudioElement = Instantiate(audioElement, audioPlayers.transform);
                AudioSource audioPlayer = newAudioElement.GetComponent<AudioSource>();

                audioPlayer.clip = clip;
                audioPlayer.Play();
                StartCoroutine(FadeIn(audioPlayer, 0.5f));

                holdPlaying.Add(clip);
                holdPlayers.Add(audioPlayer);
            }
        }

        currentPlayers.AddRange(holdPlayers);
        currentPlaying.AddRange(holdPlaying);

        holdPlaying.Clear();
        holdPlayers.Clear();
    }

    public void PlayMultipleClips(List<AudioClip> clips)
    {
        foreach (AudioClip clip in clips)
        {
            GameObject newSingleAudioElement = Instantiate(singleAudioElement, singleAudioPlayers.transform);
            AudioSource singleAudioPlayer = newSingleAudioElement.GetComponent<AudioSource>();

            singleAudioPlayer.clip = clip;
            singleAudioPlayer.Play();
        }
    }

    public void PlayChainClips(List<AudioClip> clips)
    {
        AudioSource firstAudioPlayer = null;
        AudioSource holdAudioPlayer = null;

        for (int i = 0; i<clips.Count; i++)
        {
            GameObject chainAudio = Instantiate(chainAudioElement, singleAudioPlayers.transform);
            AudioSource chainAudioPlayer = chainAudio.GetComponent<AudioSource>();
            chainAudioPlayer.clip = clips[i];

            if (i == 0)
            {
                firstAudioPlayer = chainAudioPlayer;
            }
            else
            {
                holdAudioPlayer.GetComponent<DestroySelf>().nextAudio = chainAudioPlayer;
            }

            holdAudioPlayer = chainAudioPlayer;
        }

        firstAudioPlayer.Play();
    }

    public void PlaySingleClip(AudioClip clip)
    {
        GameObject newSingleAudioElement = Instantiate(singleAudioElement, singleAudioPlayers.transform);
        AudioSource singleAudioPlayer = newSingleAudioElement.GetComponent<AudioSource>();
        singleAudioPlayer.clip = clip;
        singleAudioPlayer.Play();
    }

    public IEnumerator FadeOut(AudioSource fadeAudio, float fadeSpeed)
    {
        float fadeOutValue = 0;

        while (fadeOutValue < 1)
        {
            fadeAudio.volume = Mathf.Lerp(1, 0, fadeOutValue);
            fadeOutValue += Time.deltaTime * fadeSpeed;

            yield return null;
        }

        Destroy(fadeAudio.gameObject);

        yield break;
    }

    public IEnumerator FadeIn(AudioSource fadeAudio, float fadeSpeed)
    {
        float fadeInValue = 0;

        while (fadeInValue < 1)
        {
            fadeAudio.volume = Mathf.Lerp(0, 1, fadeInValue);
            fadeInValue += Time.deltaTime * fadeSpeed;

            yield return null;
        }

        yield break;
    }
}

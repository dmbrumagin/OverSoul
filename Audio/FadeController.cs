using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private static AudioSource[] audioSources;

    void Awake()
    {
        // TODO make a proper audio manager w/ multiple song start/stop support
        // https://trello.com/c/HTrbqU5c/9-create-system-for-non-linear-soundtrack
        audioSources = GameObject.FindGameObjectWithTag("MusicController").GetComponents<AudioSource>();
    }

    public void StartFadeIn(int source, float speed = 0.01f, float endVolume = 1) {
        StartCoroutine(FadeIn(source, speed, endVolume));
    }
    public void StartFadeOut(int source, float speed = 0.01f)
    {
        StartCoroutine(FadeOut(source, speed));
    }

    // TODO use curved fade so this sounds more natural?
    static IEnumerator FadeIn(int source, float speed, float endVolume)
    {
        audioSources[source].volume = 0;
        float audioVolume = audioSources[source].volume;

        while (audioSources[source].volume < endVolume) {
            audioVolume += speed;
            if (audioVolume > 1) audioVolume = 1;
            audioSources[source].volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    static IEnumerator FadeOut(int source, float speed)
    {
        float audioVolume = audioSources[source].volume;

        while (audioSources[source].volume > 0)
        {
            audioVolume -= speed;
            if (audioVolume < 0) audioVolume = 0;
            audioSources[source].volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

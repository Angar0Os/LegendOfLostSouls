using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class LOLS_SoundManager : MonoBehaviour
{
    public AudioClip[] soundClips;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

    private AudioSource audioSource;

    private static LOLS_SoundManager instance;

    public static LOLS_SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LOLS_SoundManager>();

                if (instance == null)
                {
                    GameObject LOLS_SoundManagerGO = new GameObject("LOLS_SoundManager");
                    instance = LOLS_SoundManagerGO.AddComponent<LOLS_SoundManager>();
                }
            }

            return instance;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaySound("Click");
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
        foreach (AudioClip clip in soundClips)
        {
            soundDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            if(soundName == "Click")
            {
                audioSource.volume = 0.25f;
            }
            else
            {
                audioSource.volume = 1f;
            }
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
    }

    public void PlaySoundWithDelay(string soundName, float delay)
    {
        StartCoroutine(PlayDelayed(soundName, delay));
    }

    private IEnumerator PlayDelayed(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(soundName);
    }
}

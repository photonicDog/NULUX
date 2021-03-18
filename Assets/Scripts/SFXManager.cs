// SFXManager

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioAssetKey audioKey;

    private new AudioSource audio;

    public static SFXManager Instance;

    private Dictionary<int, AudioClip> hitSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple SFXManager in the scene!");
            Object.Destroy(this);
        }
        audio = GetComponent<AudioSource>();
        Object.DontDestroyOnLoad(base.gameObject);
    }

    public void PlayAudio(string key)
    {
        audio.clip = audioKey.audioDict[key].audio;
        audio.Play();
    }

    public void KillAudio()
    {
        audio.clip = null;
        audio.Stop();
    }

    public AudioSource GetAudio()
    {
        return audio;
    }

    public void ResetAudio() {
        audio.Stop();
        audio.time = 0;
    }

    public void LoadHitsounds(List<ScoringJudgement> judges) {
        hitSounds = new Dictionary<int, AudioClip>();
        foreach (var j in judges) {
            hitSounds.Add(j.leftMS, j.feedbackAudio);
        }
    }

    public void PlayHitsound(int index) {
        audio.clip = hitSounds[index];
        audio.Play();
    }
}

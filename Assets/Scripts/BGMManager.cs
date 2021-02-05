// BGMManager
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioAssetKey audioKey;

    public AudioSource audio;

    public AudioSource loopSource;

    public static BGMManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple BGMManager in the scene!");
            Object.Destroy(this);
        }
        Object.DontDestroyOnLoad(base.gameObject);
    }

    public void PlayAudio(string key)
    {
        AudioClip clip = audioKey.audioDict[key].audio;
        AudioClip start = audioKey.audioDict[key].start;
        if (start != null)
        {
            audio.clip = start;
            audio.loop = false;
            loopSource.clip = clip;
            loopSource.loop = true;
            audio.PlayScheduled(AudioSettings.dspTime + 0.20000000298023224);
            loopSource.PlayScheduled(AudioSettings.dspTime + (double)start.length + 0.20000000298023224);
        }
        else
        {
            loopSource.clip = clip;
            loopSource.Play();
            loopSource.loop = true;
        }
    }

    public void KillAudio()
    {
        audio.clip = null;
        loopSource.clip = null;
        audio.Stop();
        loopSource.Stop();
    }
}

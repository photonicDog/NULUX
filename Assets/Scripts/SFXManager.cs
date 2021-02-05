// SFXManager
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioAssetKey audioKey;

    private AudioSource audio;

    public static SFXManager Instance;

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
}

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioScrubber : MonoBehaviour {
   
    private float scrollPos = 0f;
    public float bpm;

    private float speedRatio = 1f;
    public AudioMixer am;
   
    private void Start() {
        GetComponent<AudioSource>().Play();    
    }
   
    private void OnGUI(){
        scrollPos = GUI.HorizontalSlider(new Rect(25f, 50f, Screen.width - 50, 50f), scrollPos, 0, GetComponent<AudioSource>().clip.length);
        speedRatio = GUI.HorizontalSlider(new Rect(25f, 100f, 200f, 50f), speedRatio, 0.5f, 1.5f);
        
        if(GUI.changed == true){
            GetComponent<AudioSource>().time = scrollPos;
            GetComponent<AudioSource>().pitch = speedRatio;
            am.outputAudioMixerGroup.audioMixer.SetFloat("pitchBend", 1f / speedRatio);
        }
       
        GUI.Label(new Rect(10f, 80f, 100f, 30f), (GetComponent<AudioSource>().time / (60 / bpm)).ToString());
    }
}
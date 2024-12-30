using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    bool isInit = false;

    private AudioMixer mixer;
    public AudioMixer Mixer 
    { 
        get {
            if (isInit == false)
            {
                Init();
            }
            return mixer; 
        } 
    }

    AudioSource _audioSources2D;

    public void Init()
    {
        isInit = true;
        // 오디오 믹서 로드
        mixer = Managers.Resource.Load<AudioMixer>("Mixer/Mixer");
        
        GameObject root = GameObject.Find("Sounds");
        if(root == null)
        {
            root = new GameObject { name = "Sounds" };
            Object.DontDestroyOnLoad(root);
            GameObject go = new GameObject();
            _audioSources2D = go.AddComponent<AudioSource>();
            _audioSources2D.volume = 0.5f;
            go.transform.parent = root.transform;

            _audioSources2D.outputAudioMixerGroup = Mixer.FindMatchingGroups("FX")[0];
        }
    }

    public void Play2D(string path)
    {
        AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
        if (isInit == false)
        {
            Init();
        }

        if (audioClip == null)
            return;

        AudioSource audioSource = _audioSources2D;
        audioSource.PlayOneShot(audioClip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {
    public enum Sound{
        BackgroundMusic,
        BirdJump,
        Score,
        Lose,
        ButtonClick,
    }
    private static GameObject _soundGO = new GameObject("Sound", typeof(AudioSource));

    public static void PlaySound(Sound sound){
        if(_soundGO == null) _soundGO = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = _soundGO.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }
    private static AudioClip GetAudioClip(Sound sound){
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClips){
            if(soundAudioClip.sound == sound){
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }
}

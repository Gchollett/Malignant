using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundLibrary : MonoBehaviour
{
    [System.Serializable]
    public struct SoundEffect
    {
        public string groupID;
        public AudioClip[] clips;
    }

    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name){
        foreach (var soundEffect in soundEffects){
        if (soundEffect.groupID == name){
            return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
        }
        }
        return null;
    }
}

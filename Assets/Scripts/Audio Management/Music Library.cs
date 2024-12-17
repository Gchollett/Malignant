using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{

    [System.Serializable]
    public struct MusicTrack{
        public string trackName;
        public AudioClip clip;
    }
    public MusicTrack[] tracks;

    public AudioClip GetClipFromName(string trackName){
        foreach (var track in tracks){
            if (track.trackName == trackName){
                Debug.Log(track.trackName);
                return track.clip;
            }
        }
        //Warning.Error("Track Was Not Found");
        return null;
    }
}

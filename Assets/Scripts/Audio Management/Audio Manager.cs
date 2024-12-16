using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        var sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Overworld"){
            PlaySound2d("overworld_music");
        } else if(sceneName == "StartScreen"){
            PlaySound2d("start_screen_music");
        } else if(sceneName == "CardGame"){
            PlaySound2d("battle_music");
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos){
        if(clip != null){
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound2d(string soundName){
        if(soundName != null){
            Debug.Log("Playin");
            sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
        }
    }

}

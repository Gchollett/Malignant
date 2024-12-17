using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake(){
        Debug.Log("Checking For New Music");
        if (Instance != null){
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        playSceneMusic();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        playSceneMusic();
    }
    private void playSceneMusic(){
        var sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Overworld"){
            PlayMusic("overworld_music");
        } else if(sceneName == "StartScreen"){
            PlayMusic("start_screen_music");
        } else if(sceneName == "CardGame"){
            PlayMusic("battle_music");
        } else if(sceneName == "Hunter"){
            PlayMusic("hunter_music");
        } else if(sceneName == "CardStore"){
            PlayMusic("store_track");
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

    public void PlayMusic(string trackName, float fadeDuration = 0.5f){
        Debug.Log("Playin");
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        //yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            Debug.Log(percent);
            yield return null;
        }
 
        musicSource.clip = nextTrack;
        musicSource.Play();
 
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
        Debug.Log("Finished Coroutine");
    }

}

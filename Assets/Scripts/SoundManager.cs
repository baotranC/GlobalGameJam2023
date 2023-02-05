using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public AudioMixer audioMixer;

    float musicVol;
    float SFXVol;


    void Awake() {
        musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVol = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    void Start() {
        //SetMusicVolume(musicVol);
        //SetSFXVolume(SFXVol);
    }

    public void PlaySound(AudioSource source, bool randomPitch=false, bool createTempSourceIfBusy=true, bool createTempSource = false, float delay = 0f) {
        if (source == null || !source.gameObject.activeInHierarchy) {
            return;
        }

        if (source.isPlaying) {
            if (createTempSourceIfBusy) {
                if (randomPitch) {
                    source.pitch = Random.Range(0.9f, 1.1f);
                }
                StartCoroutine(CreateTempSource(source, delay:delay));
            }
        } else {
            if (randomPitch) {
                source.pitch = Random.Range(0.9f, 1.1f);
            }

            if(createTempSource) {
                StartCoroutine(CreateTempSource(source, delay: delay));
            } else {
                if(delay > 0f) {
                    StartCoroutine(DelayedPlaySound(source, delay));
                } else {
                    source.Play();
                }
            }
        }
    }

    public void PlaySound(AudioSource source, AudioClip[] possibleClips, bool randomPitch=false, bool createTempSourceIfBusy=true, bool createTempSource=false, float delay=0f) {
        if(source == null || ! source.gameObject.activeInHierarchy || possibleClips.Length == 0) {
            return;
        }
        source.clip = possibleClips[Random.Range(0, possibleClips.Length)];
        PlaySound(source, randomPitch, createTempSourceIfBusy, createTempSource, delay);
    }

    IEnumerator CreateTempSource(AudioSource refSource, float delay=0f) {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.volume = refSource.volume;
        newSource.clip = refSource.clip;

        PlaySound(newSource, delay:delay);

        yield return new WaitForSeconds(delay);

        // yield return new WaitForSeconds(1f);
        // Destroy(newSource);

        for(int i=0; i<100; ++i) {
            yield return new WaitForSeconds(1f);
            if(! newSource.isPlaying) {
                Destroy(newSource);
                break;
            }
        }
    }


    IEnumerator DelayedPlaySound(AudioSource source, float delay) {
        yield return new WaitForSeconds(delay);

        source.Play();
    }


    public void SetMusicVolume(float value) {
        // Slider should go from 0.0001 to 1
        
        musicVol = value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVol) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVol);
    }

    public void SetSFXVolume(float value) {
        // Slider should go from 0.0001 to 1

        SFXVol = value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVol) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXVol);
    }

    public float GetMusicVolume() {
        return musicVol;
    }

    public float GetSFXVolume() {
        return SFXVol;
    }

}

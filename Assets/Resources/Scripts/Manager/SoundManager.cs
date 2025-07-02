using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : BaseManager<SoundManager>
{
    public List<AudioClip> backgroundMusic;
    public AudioClip backgroundGame;
    public AudioClip btnClick;
    public AudioClip btnAppear;
    public AudioClip wordchainGame;
    public AudioClip cutFruitsGame;
    public AudioClip winGame;
    public AudioClip loseGame;
    public AudioClip correctWord;
    public AudioClip inCorrectWord;
    public AudioClip chooseWord;
    public AudioClip wordApear;
    public AudioClip slice;
    public AudioClip bomb;
    public AudioClip fireWork;
    public Transform parentTransform;

    private Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();

    public float musicVolume = 1f;
    public void PlayOneShotSound(AudioClip clip, float musicVolumne = 1f)
    {
        if (clip == null) return;

        Transform reusable = null;
        foreach (Transform child in parentTransform)
        {
            if (!child.gameObject.activeSelf)
            {
                reusable = child;
                break;
            }
        }

        GameObject audioObj;

        if (reusable != null)
        {
            audioObj = reusable.gameObject;
            audioObj.SetActive(true);
        }
        else
        {
            audioObj = new GameObject("OneShotAudio");
            audioObj.transform.SetParent(parentTransform);
        }

        AudioSource audio = audioObj.GetComponent<AudioSource>();
        if (audio == null)
            audio = audioObj.AddComponent<AudioSource>();

        audio.clip = clip;
        audio.volume = musicVolumne;
        audio.loop = false;
        audio.Play();

        Instance.StartCoroutine(DisableAfterFinished(audio));
    }

    private IEnumerator DisableAfterFinished(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.gameObject.SetActive(false);
    }


    public void PlayLoopingMusic(AudioClip clip, string tag = "Music", bool isRandomMusic = false, float volumne = 0.5f)
    {
        if (clip == null) return;

        if (loopingSources.ContainsKey(tag))
        {
            AudioSource existing = loopingSources[tag];
            if (existing != null && existing.gameObject.activeSelf)
            {
                if (existing.clip != clip)
                {
                    existing.clip = clip;
                    existing.Play();
                }
                return;
            }
        }

        GameObject audioObj = new GameObject("LoopingMusic_" + tag);
        audioObj.transform.SetParent(parentTransform);

        AudioSource audio = audioObj.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volumne;
        audio.loop = !isRandomMusic; 
        audio.Play();

        loopingSources[tag] = audio;

        if (isRandomMusic)
        {
            Instance.StartCoroutine(LoopRandomMusicCoroutine(audio, tag));
        }
    }
    private IEnumerator LoopRandomMusicCoroutine(AudioSource audio, string tag)
    {
        while (true)
        {
            yield return new WaitForSeconds(audio.clip.length);

            AudioClip newClip = RandomMusic();
            if (newClip != null)
            {
                audio.clip = newClip;
                audio.Play();
            }
            else
            {
                yield break;
            }
        }
    }


    public void StopLoopingMusic(string tag = "Music")
    {
        if (loopingSources.ContainsKey(tag))
        {
            AudioSource src = loopingSources[tag];
            if (src != null)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
            loopingSources.Remove(tag);
        }
    }
    public AudioClip RandomMusic()
    {
        if (backgroundMusic == null || backgroundMusic.Count == 0)
            return null;

        int index = Random.Range(0, backgroundMusic.Count);
        return backgroundMusic[index];
    }
    public void PressButton()
    {
        PlayOneShotSound(btnClick);

    }
    public void PlayAppearSound()
    {
        PlayOneShotSound(btnAppear);
    }
}

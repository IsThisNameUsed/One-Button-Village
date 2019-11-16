using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public string AutoPlayMusic = "";
    public StringClipDictionary Songs;
    public StringClipDictionary Sounds;

    public float FadeSpeed = 1f;

    private AudioSource _source;
    private string lastScene;
    private List<string> currentSounds;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _source = GetComponent<AudioSource>();

        if (AutoPlayMusic != "")
            StartMusic(AutoPlayMusic, 0.6f);

        SceneManager.activeSceneChanged += OnSceneChanged;
        lastScene = SceneManager.GetActiveScene().name;

        currentSounds = new List<string>();
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        if (lastScene == arg1.name)
            return;

        switch(arg1.name)
        {
            case "level1":
                ChangeMusic("music1", 0.25f);
                break;
        }

        lastScene = arg1.name;
    }

    public void ChangeMusic(string music, float volume)
    {
        StartCoroutine(CorChangeMusic(music, volume));
    }

    private IEnumerator CorChangeMusic(string music, float volume)
    {
        while(_source.volume > 0.1f)
        {
            _source.volume -= FadeSpeed * Time.deltaTime;
            yield return null;
        }

        StartMusic(music, volume);
    }

    public void StartMusic(string songName, float volume)
    {
        AudioClip clip;
        Songs.TryGetValue(songName, out clip);

        if (clip == null)
        {
            Debug.LogWarning("Tried to play an inexistant song : " + songName);
            return;
        }

        StartCoroutine(CorStartMusic(clip, volume));
    }

    public IEnumerator CorStartMusic(AudioClip clip, float volume)
    {
        _source.clip = clip;
        _source.volume = 0;
        _source.Play();
        while (_source.volume < volume)
        {
            _source.volume += FadeSpeed * Time.deltaTime;
            yield return null;
        }
        _source.volume = volume;
    }

    public void Pause(bool pause = true)
    {
        if (pause)
            _source.Pause();
        else
            _source.UnPause();
    }

    public void PlaySound(string sound, float volume, bool unique = false, float fade = 0.5f)
    {
        if (unique && currentSounds.Exists(s => s == sound))
            return;

        AudioClip clip;
        Sounds.TryGetValue(sound, out clip);

        if (clip == null)
        {
            Debug.LogWarning("Tried to play an inexistant sound : " + sound);
            return;
        }

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.PlayOneShot(clip);
        
        if (fade != 0)
        {
            gameObject.Tween("Sound" + Time.time, 0, volume, fade, TweenScaleFunctions.Linear, t =>
            {
                source.volume = t.CurrentValue;
            });
            StartCoroutine(FinishVolume(source, clip.length - fade, fade));
        }
        else
        {
            source.volume = volume;
        }

        Destroy(source, clip.length + 0.2f);

        if (unique)
            StartCoroutine(RemoveUnique(sound, clip.length));
    }

    private IEnumerator RemoveUnique(string sound, float time)
    {
        currentSounds.Add(sound);
        yield return new WaitForSeconds(time);
        currentSounds.Remove(sound);
    }

    private IEnumerator FinishVolume(AudioSource source, float time, float fade)
    {
        yield return new WaitForSeconds(time);
        gameObject.Tween("Sound" + Time.time, source.volume, 0, fade, TweenScaleFunctions.Linear, t =>
        {
            source.volume = t.CurrentValue;
        });
    }
}

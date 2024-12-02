using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс для управления изменениями и настройкой звука
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private Dictionary<string,Sound> _musicDictionary, _sfxDictionary;

    private float _masterVolume = 1f; // Общий уровень громкости
    private float _fadeDuration = 5; // Длительность плавности воспроизведения

    private float _currentMusicVolume;
    private float _musicVolume;
    private float _sfxVolume;

    public const string MUSIC_VOLUME = "MusicVolume"; 
    public const string SFX_VOLUME = "SFXVolume"; 
    public const string MASTER_VOLUME = "MasterVolume";
    public const string MUTE = "Mute"; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        _musicDictionary = _musicSounds.ToDictionary(s => s.name);
        _sfxDictionary = _sfxSounds.ToDictionary(s => s.name);
        LoadAllVolumes(); 
        PlayMusic("BackGroundMusic");
    }

    private void OnApplicationQuit()
    {
        SaveAllVolumes();
    }

    /// <summary>
    /// Воспроизводит фоновую музыку
    /// </summary>
    /// <param name="name">Назавние трека</param>
    public void PlayMusic(string name)
    {
        if (_musicDictionary.TryGetValue(name,out Sound s))
        {
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
        else
        {
            Debug.Log("Sound not found");
        }
    }

    /// <summary>
    /// Воспроизводит звуковой эффект
    /// </summary>
    /// <param name="name">Название эффекта</param>
    public void PlaySFX(string name)
    {
        if (_sfxDictionary.TryGetValue(name,out Sound s))
        {
            _sfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("SFX not found");
        }
    }

    /// <summary>
    /// Включает/выключает звук
    /// </summary>
    /// <param name="mute">Состояние звука</param>
    public void AudioToggle(bool mute)
    {
        _musicSource.mute = mute;
        _sfxSource.mute = mute; 

        if (!mute)
        {
            StartCoroutine(FadeIn()); 
        }
        else
        {
            StopAllCoroutines();
        }

        PlayerPrefs.SetInt(MUTE, mute ? 1 : 0); 
    }

    /// <summary>
    /// Устанавливает громкость фонофой музыки
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void MusicVolume(float volume)
    {
        _currentMusicVolume = volume; 
        _musicSource.volume = volume * _masterVolume; 
        _musicVolume = volume; 
    }

    /// <summary>
    /// Устанавливает громкость звуковых эффектов
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume * _masterVolume; 
        _sfxVolume = volume; 
    }

    /// <summary>
    /// Устанавливает общую громкость
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void MasterVolume(float volume)
    {
        _masterVolume = volume; 
        ApplyMasterVolume(); 
    }

    /// <summary>
    /// Применяет общую громкость ко всем источникам звука
    /// </summary>
    private void ApplyMasterVolume()
    {
        _musicSource.volume = _musicVolume * _masterVolume; 
        _sfxSource.volume = _sfxVolume * _masterVolume; 
    }

    /// <summary>
    /// Корутина для плавного увеличения громкости
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        float currentTime = 0f; 
        _musicSource.volume = 0f; 

        while (currentTime < _fadeDuration) 
        {
            currentTime += Time.deltaTime; 
            _musicSource.volume = Mathf.Lerp(0f, _currentMusicVolume, currentTime / _fadeDuration) * _masterVolume; // Плавно увеличивае громкость
            yield return null; // Ждет до следующего кадра
        }

        _musicSource.volume = _currentMusicVolume * _masterVolume; 
    }

    /// <summary>
    /// Сохраняет громкость всех источников
    /// </summary>
    public void SaveAllVolumes()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, _musicVolume); 
        PlayerPrefs.SetFloat(SFX_VOLUME, _sfxVolume);
        PlayerPrefs.SetFloat(MASTER_VOLUME, _masterVolume); 
        PlayerPrefs.Save(); 
    }

    /// <summary>
    /// Загружает громкость всех источников
    /// </summary>
    private void LoadAllVolumes()
    {
        _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f); 
        _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME, 1f); 
        _masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME, 1f); 

        _currentMusicVolume = _musicVolume; 

        ApplyMasterVolume(); 
    }
}

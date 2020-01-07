using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class QAudioSingleton : MonoSingleton<QAudioSingleton>
{
    private Dictionary<string, AudioClip> _dicAudio = null;
    public AudioSource _musicSource = null;                  //背景音乐
    public AudioSource _effectSource = null;                 //音效
    private float _musicVolume = 0;                          //背景音乐音量    
    private float _effectVolume = 0;                         //音效音量  

    public void Init()
    {
        if (this._dicAudio == null)
        {
            this._musicSource = this.gameObject.AddComponent<AudioSource>();
            this._effectSource = this.gameObject.AddComponent<AudioSource>();
            this._dicAudio = new Dictionary<string, AudioClip>();
            this.InitAllAudioClip();
            this.SetMusicVolume(1);
            this.SetEffectVolume(1);
        }
    }
    /// <summary>
    /// 开启背景音乐
    /// </summary>
    /// <param name=""></param>
    public void OpenMusic()
    {
        this._musicSource.Play();
        SetMusicVolume(1);
    }
    public void CloseMusic()
    {
        this._musicSource.Stop();
        SetMusicVolume(0);
    }
    /// <summary>
    /// 开启音效
    /// </summary>
    /// <param name="_Value"></param>
    public void OpenEffect()
    {
        this._effectSource.Play();
        SetEffectVolume(1);

    }
    public void CloseEffect()
    {
        this._effectSource.Stop();
        SetEffectVolume(0);
    }

    public void SetMusicVolume(float _Value)
    {
        if (_Value < 0)
            return;
        else
            this._musicVolume = _Value;
    }

    public void SetEffectVolume(float _Value)
    {
        if (_Value < 0)
            return;
        else
            this._effectVolume = _Value;
    }


    public AudioSource PlayerEffectAudio(string _ClipName, float _Volume = 1)
    {
        if (this._dicAudio.ContainsKey(_ClipName))
        {
            _Volume *= this._effectVolume;
            if (0.01f < _Volume)
            {
                this._effectSource.PlayOneShot(this._dicAudio[_ClipName], _Volume);
            }
        }
        else
        {
            Debug.LogWarning("找不到对应得声音");
        }
        return this._effectSource;
    }
    public AudioSource PlayeMusicAudio(string _ClipName, float _Volume = 1)
    {
        if (this._dicAudio.ContainsKey(_ClipName))
        {
            _Volume *= this._musicVolume;
            if (0.01f < _Volume)
            {
                this._musicSource.loop = true;
                this._musicSource.clip = this._dicAudio[_ClipName];
                this._musicSource.volume = _Volume;
                this._musicSource.Play();
            }
            else
            {
                this._musicSource.Stop();
            }
        }
        else
        {
            Debug.LogWarning("找不到对应得声音");
        }
        return this._musicSource;
    }

    void InitAllAudioClip()
    {
        this._dicAudio.Add(AudioName.Click, Resources.Load<AudioClip>(AudioPath.Click));
        this._dicAudio.Add(AudioName.Win, Resources.Load<AudioClip>(AudioPath.Win));
        this._dicAudio.Add(AudioName.Fail, Resources.Load<AudioClip>(AudioPath.Fail));
        this._dicAudio.Add(AudioName.BackMusic, Resources.Load<AudioClip>(AudioPath.BackMusic));
        this._dicAudio.Add(AudioName.BackMusic2, Resources.Load<AudioClip>(AudioPath.BackMusic2));
    }
}
public class AudioPath
{
    public static string Click = "Source/Click";
    public static string Fail = "Source/Fail";
    public static string Win = "Source/Win";
    public static string BackMusic = "Source/BackMusic";
    public static string BackMusic2 = "Source/欢快渐进BG";
}
public static class AudioName
{
    public static string Click = "Click";
    public static string Fail = "Fail";
    public static string Win = "Win";
    public static string BackMusic = "BackMusic";
    public static string BackMusic2 = "BackMusic2";
}
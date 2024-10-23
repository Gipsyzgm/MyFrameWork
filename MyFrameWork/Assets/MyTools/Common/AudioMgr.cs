/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.12.28
 *  描述信息：简单声音和音效控制。
 *  注意事项：
 *  1：和ExportAudio自动生成声音路径文件结合使用，路径可以在ExportAudio变更。
 *  2：不支持同时播放多个背景音乐。
 *  3：声音文件放在Assets/Resources/MySource路径下。避免同名文件。
 *  4：使用：a:编辑器模式下我的工具——导入声音。b:工程代码中调用Init方法即可。c:通过MyAudioMgr.Instance.调用对应方法即可。
 */

using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public class AudioMgr : MonoSingleton<AudioMgr>
{
    private Dictionary<string, AudioClip> _dicAudio = null;
    public AudioSource musicSource = null; //背景音乐
    public AudioSource effectSource = null; //音效

    public bool musicOn = true;
    public bool soundOn = true;

    public void Init()
    {
        if (_dicAudio == null)
        {
            _dicAudio = new Dictionary<string, AudioClip>();
            InitAllAudioClip();
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
        musicOn = PlayerPrefs.GetInt(PlayerPrefKey.MusicOn, 1) == 1;
        soundOn = PlayerPrefs.GetInt(PlayerPrefKey.SoundOn, 1) == 1;
        SetMusicVolume(PlayerPrefs.GetFloat(PlayerPrefKey.MusicVoulume, 1.0f));
        SetEffectVolume(PlayerPrefs.GetFloat(PlayerPrefKey.SoundVoulume, 1.0f));
        if (musicOn)
        {
            PlayMusic(MyAudioName.bgm_1);
        }
    }

    /// <summary>
    /// 开启背景音乐
    /// </summary>
    public void OpenMusic(bool isOpen)
    {
        if (isOpen)
        {
            PlayerPrefs.SetInt(PlayerPrefKey.MusicOn, 1);
            if (musicSource.clip != null)
            {
                musicSource.Play();
            }
            else
            {
                PlayMusic(MyAudioName.BackMusic);
            }
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefKey.MusicOn, 0);
            musicSource.Stop();
        }
    }

    /// <summary>
    /// 开启音效
    /// </summary>
    public void OpenEffect(bool isOpen)
    {
        soundOn = isOpen;
        PlayerPrefs.SetInt(PlayerPrefKey.SoundOn, isOpen ? 1 : 0);
    }

    public void SetMusicVolume(float value)
    {
        if (value < 0)
            return;
        else
        {
            musicSource.volume = value;
            PlayerPrefs.SetFloat(PlayerPrefKey.MusicVoulume, value);
        }
    }

    public void SetEffectVolume(float value)
    {
        if (value < 0)
            return;
        else
        {
            effectSource.volume = value;
            PlayerPrefs.SetFloat(PlayerPrefKey.SoundVoulume, value);
        }
    }


    public void PlayEffect(MyAudioName clipName, float volume = 1f)
    {
        if (soundOn == false)
        {
            return;
        }

        string _ClipName = clipName.ToString();
        if (_dicAudio.TryGetValue(_ClipName, out var value))
        {
            effectSource.volume = effectSource.volume * volume;
            effectSource.PlayOneShot(value);
        }
        else
        {
            Debug.LogWarning("找不到对应音效" + _ClipName);
        }
    }

    public void PlayEffect(int audioId)
    {
        //PlayEffect(musicName, volume);
    }


    public void PlayMusic(MyAudioName clipName, float volume = 1f)
    {
        string _ClipName = clipName.ToString();
        if (_dicAudio.TryGetValue(_ClipName, out var value))
        {
            musicSource.loop = true;
            musicSource.volume = musicSource.volume * volume;
            musicSource.clip = value;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("找不到对应得音效" + _ClipName);
        }
    }

    public void PlayEffectOnTarget(MyAudioName clipName, GameObject target, float volume = 1f)
    {
        if (soundOn == false)
        {
            return;
        }

        AudioSource audioSource = target.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = target.AddComponent<AudioSource>();
        }

        string _ClipName = clipName.ToString();
        if (_dicAudio.TryGetValue(_ClipName, out var value))
        {
            audioSource.loop = true;
            audioSource.volume = effectSource.volume * volume;
            effectSource.PlayOneShot(value);
        }
        else
        {
            Debug.LogWarning("找不到对应得音效" + _ClipName);
        }
    }

    void InitAllAudioClip()
    {
        foreach (var item in Enum.GetValues(typeof(MyAudioName)))
        {
            AudioClip audioClip = LoaderMgr.Instance.LoadAssetSync<AudioClip>(MyAudioPath.Path + item.ToString());
            _dicAudio.Add(item.ToString(), audioClip);
        }
    }
}
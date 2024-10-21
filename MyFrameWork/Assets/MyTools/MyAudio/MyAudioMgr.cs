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

public class MyAudioMgr : MonoSingleton<MyAudioMgr>
{
    private Dictionary<string, AudioClip> _dicAudio = null;
    public AudioSource _musicSource = null; //背景音乐
    public AudioSource _effectSource = null; //音效

    public void Init()
    {
        if (this._dicAudio == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _effectSource = gameObject.AddComponent<AudioSource>();
            _dicAudio = new Dictionary<string, AudioClip>();
            InitAllAudioClip();
            SetMusicVolume(PlayerPrefs.GetFloat(PlayerPrefKey.MusicVoulume, 1.0f));
            SetEffectVolume(PlayerPrefs.GetFloat(PlayerPrefKey.SoundVoulume, 1.0f));
        }

        PlayeMusicAudio(MyAudioName.BackMusic);
    }

    /// <summary>
    /// 开启背景音乐
    /// </summary>
    /// <param name=""></param>
    public void OpenMusic(bool isOpen)
    {
        if (isOpen)
        {
            PlayerPrefs.SetInt(PlayerPrefKey.MusicOn, 1);
            _musicSource.Play();
            SetMusicVolume(1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefKey.MusicOn, 0);
            _musicSource.Stop();
            SetMusicVolume(0);
        }

    }

    /// <summary>
    /// 开启音效
    /// </summary>
    /// <param name="_Value"></param>
    public void OpenEffect(bool isOpen)
    {
        if (isOpen)
        {
            PlayerPrefs.SetInt(PlayerPrefKey.SoundOn, 1);
            _effectSource.Play();
            SetEffectVolume(1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefKey.SoundOn, 0);
            _effectSource.Stop();
            SetEffectVolume(0);
        }

    }

    public void CloseEffect()
    {
       
    }

    public void SetMusicVolume(float _Value)
    {
        Debug.LogError("音量" + _Value);
        if (_Value < 0)
            return;
        else
        {
            _musicSource.volume = _Value;
            PlayerPrefs.SetFloat(PlayerPrefKey.MusicVoulume, _Value);
        }
    }

    public void SetEffectVolume(float _Value)
    {
        if (_Value < 0)
            return;
        else
        {
            _effectSource.volume = _Value;
            PlayerPrefs.SetFloat(PlayerPrefKey.SoundVoulume, _Value);
        }
    }


    public AudioSource PlayerEffectAudio(MyAudioName ClipName)
    {
        string _ClipName = ClipName.ToString();
        if (_dicAudio.ContainsKey(_ClipName))
        {
            _effectSource.PlayOneShot(_dicAudio[_ClipName]);
        }
        else
        {
            Debug.LogWarning("找不到对应得声音");
        }

        return this._effectSource;
    }

    public AudioSource PlayeMusicAudio(MyAudioName ClipName)
    {
        string _ClipName = ClipName.ToString();
        if (_dicAudio.ContainsKey(_ClipName))
        {
            _musicSource.loop = true;
            _musicSource.clip = _dicAudio[_ClipName];
            _musicSource.Play();
        }
        else
        {
            Debug.LogWarning("找不到对应得声音");
        }

        return _musicSource;
    }

    void InitAllAudioClip()
    {
        foreach (var item in Enum.GetValues(typeof(MyAudioName)))
        {
            var obj = Addressables.LoadAssetAsync<AudioClip>(MyAudioPath.Path + item.ToString()).WaitForCompletion();
            this._dicAudio.Add(item.ToString(), obj);
        }
    }
}
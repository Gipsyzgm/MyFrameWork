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

public class MyAudioMgr : MonoSingleton<MyAudioMgr>
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


    public AudioSource PlayerEffectAudio(MyAudioName ClipName, float _Volume = 1)
    {
        string  _ClipName = ClipName.ToString();
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
    public AudioSource PlayeMusicAudio(MyAudioName ClipName, float _Volume = 1)
    {
        string _ClipName = ClipName.ToString();
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
        foreach (var item in Enum.GetValues(typeof(MyAudioName)))
        {
            item.ToString();
            //this._dicAudio.Add(item.ToString(), Resources.Load<AudioClip>(MyAudioPath.Path + item.ToString()));
            this._dicAudio.Add(item.ToString(), ABMgr.Instance.LoadAsset<AudioClip>(MyAudioPath.Path + item.ToString()));
        }
    }
}

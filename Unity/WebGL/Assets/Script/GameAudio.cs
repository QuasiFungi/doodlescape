using UnityEngine;
using System.Collections.Generic;
public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;
    private AudioSource _audioUI;
    private AudioSource _audioEntity;
    public AudioClip[] _clips;
    public enum AudioType
    {
        UI,
        ENTITY
    }
    void Awake()
    {
        // overwrite copy between loads
        if (Instance) Destroy(Instance.gameObject);
        Instance = this;
        // 
        _audioUI = transform.GetChild(0).GetComponent<AudioSource>();
        _audioEntity = transform.GetChild(1).GetComponent<AudioSource>();
        _queue = new List<SFX>();
        // double check
        _audioEntity.mute = true;
    }
    public void Register(int id, AudioType type, bool isTick = false)
    {
        // ignore invalid ids
        if (id < 0 || id > _clips.Length - 1) return;
        // 
        // _audio.clip = _clips[id];
        // _audio.Play();
        // if (isTick)
        // {
        //     if (!_queue.Contains(_clips[id])) _queue.Add(_clips[id]);
        // }
        // else _audio.PlayOneShot(_clips[id]);
        // 
        Register(_clips[id], type, isTick);
    }
    public bool _isDebug = false;
    public void Register(AudioClip clip, AudioType type, bool isTick = false)
    {
        if (_isDebug) print(type.ToString() + ":\t" + clip.name);
        // 
        if (isTick)
        {
            SFX sfx = new SFX(clip, type);
            if (!_queue.Contains(sfx)) _queue.Add(sfx);
        }
        else
        {
            switch (type)
            {
                case AudioType.UI:
                    _audioUI.PlayOneShot(clip);
                    break;
                case AudioType.ENTITY:
                    _audioEntity.PlayOneShot(clip);
                    break;
            }
        }
    }
    void OnEnable()
    {
        GameClock.onTickUI += onTick;
        ManagerUI.onReady += Initialize;
        GameMaster.onTransition += ToggleActive;
    }
    void OnDisable()
    {
        GameClock.onTickUI -= onTick;
        ManagerUI.onReady -= Initialize;
        GameMaster.onTransition -= ToggleActive;
    }
    private void Initialize()
    {
        // prevents sound glitch on game start
        _audioEntity.mute = false;
    }
    private void ToggleActive(bool state)
    {
        // mute during transition
        _audioEntity.mute = !state;
        // 
        if (_isDebug) print(state ? "Unmute" : "Mute");
        // if (state)
        // {
        //     // _audioUI.mute = true;
        //     _audioEntity.mute = true;
        // }
        // else
        // {
        //     // _audioUI.mute = false;
        //     _audioEntity.mute = false;
        // }
    }
    private List<SFX> _queue;
    private struct SFX
    {
        public AudioClip Clip;
        public AudioType Type;
        public SFX(AudioClip clip, AudioType type)
        {
            Clip = clip;
            Type = type;
        }
    }
    private void onTick()
    {
        for (int i = _queue.Count - 1; i > -1; i--)
        {
            switch (_queue[i].Type)
            {
                case AudioType.UI:
                    _audioUI.PlayOneShot(_queue[i].Clip);
                    break;
                case AudioType.ENTITY:
                    _audioEntity.PlayOneShot(_queue[i].Clip);
                    break;
            }
        }
        // 
        _queue.Clear();
    }
}
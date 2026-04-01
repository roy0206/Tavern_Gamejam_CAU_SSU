using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, ISceneEventListener
{
    Dictionary<string, AudioClip> soundClips;
    Dictionary<int, (AudioSource source, int count)> activeSources;
    Transform poolParent;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;
    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;
    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;

    [SerializeField] AudioSource channelPrefab;

    new void Awake()
    {
        base.Awake();
        activeSources = new();
        poolParent = transform;
        LoadAllSoundsFromResources();
    }

    // Update is called once per frame
    void Update() => ManageActiveSound();

    int count = 0;
    int CreateSourceId() => 10000000 + count++;


    private void LoadAllSoundsFromResources()
    {
        soundClips = new Dictionary<string, AudioClip>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        foreach (var clip in clips)
            soundClips.Add(clip.name, clip);


        Debug.Log(soundClips.Count + "Sounds have Loaded");
    }

    public int PlaySound(string soundName, Transform sourceParent, float volume, int repeatTime)
    {
        if (soundClips.TryGetValue(soundName, out AudioClip clip))
        {
            var channel = GetAvaliableChannel();
            channel.transform.parent = sourceParent;
            channel.transform.localPosition = Vector2.zero;
            channel.clip = clip;
            channel.volume = volume;
            channel.loop = false;
            channel.maxDistance = volume * 10;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0f, 1f);   // Min Distance → 최대 볼륨
            curve.AddKey(1f, 0f);   // Max Distance → 0

            channel.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
            channel.Play();
            var id = CreateSourceId();
            activeSources[id] = (channel, repeatTime);
            return id;
        }
        else return -1;
    }

    public void StopSound(int id)
    {
        if(activeSources.TryGetValue(id, out (AudioSource source, int count) channel))
        {
            channel.source.Stop();
            UnableChannel(channel.source);
            activeSources.Remove(id);
        }
    }

    public void UnableChannel(AudioSource source)
    {
        int key = 0;
        foreach (KeyValuePair<int, (AudioSource, int)> pair in activeSources)
        {
            if (pair.Value.Item1 == source)
            {
                key = pair.Key;
                break;
            }
        }
        if (key == 0) return;

        source.Stop();
        source.transform.parent = poolParent;
        source.gameObject.SetActive(false);

        activeSources.Remove(key);
    }

    private AudioSource GetAvaliableChannel()
    {
        AudioSource channel;
        for (int i = 0; i < poolParent.childCount; i++)
        {
            channel = poolParent.GetChild(i).GetComponent<AudioSource>();
            if (channel && !channel.gameObject.activeSelf)
            {
                channel.gameObject.SetActive(true);
                channel.transform.localPosition = Vector3.zero;
                return channel;
            }
        }

        channel = Instantiate(channelPrefab.gameObject).GetComponent<AudioSource>();
        channel.gameObject.SetActive(true);
        channel.playOnAwake = false;
        channel.transform.parent = poolParent.transform;
        channel.transform.localPosition = Vector3.zero;

        return channel;
    }

    private void ManageActiveSound()
    {
        List<int> removeList = new();
        List<int> replayList = new();
        foreach (int key in activeSources.Keys)
        {
            if (!activeSources.TryGetValue(key, out var value) || value.source == null || value.count <= 0)
            {
                removeList.Add(key);
                continue;
            }
            else if (!value.source.isPlaying)
            {
                if (value.count > 1) replayList.Add(key);
                else removeList.Add(key);
            }

        }

        while(removeList.Count > 0)
        {
            if (activeSources.TryGetValue(removeList[0], out var value))
            {
                if(value.source != null)
                    UnableChannel(value.source);
            }

            removeList.RemoveAt(0);
        }

        while (replayList.Count > 0)
        {
            activeSources[replayList[0]] = (activeSources[replayList[0]].source, activeSources[replayList[0]].count - 1);
            activeSources[replayList[0]].source.Play();
            replayList.RemoveAt(0);
        }
    }


    public void DisableAllChannel()
    {
        foreach (KeyValuePair<int, (AudioSource, int)> kv in activeSources)
        {
            UnableChannel(kv.Value.Item1);
        }
    }

    public void OnSceneLoadStart(string sceneName)
    {
        DisableAllChannel();
    }

    public void OnSceneLoadComplete(string sceneName)
    {

    }
}

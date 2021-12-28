using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("SoundManager");
                obj.AddComponent<SoundManager>();
                instance = obj.GetComponent<SoundManager>();
                instance.sourceQueue = new Queue<AudioSource>();
                instance.soundFilePath = string.Concat(Application.persistentDataPath, "/sound_file.txt");
                if (File.Exists(instance.soundFilePath))
                {
                    instance._baseVolume = float.Parse(File.ReadAllText(instance.soundFilePath));
                }
                else
                {
                    instance.baseVolume = 0.5f;
                }
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    public Queue<AudioSource> sourceQueue;
    private float _baseVolume;
    public float baseVolume
    {
        get { return _baseVolume; }
        set
        {
            _baseVolume = value;
            File.WriteAllText(soundFilePath, _baseVolume.ToString());
        }
    }

    public string soundFilePath;

    public AudioSource PlaySound(AudioClip clip, float volume, bool loop)
    {
        AudioSource source = null;
        
        if (sourceQueue.Count == 0)
        {
            GameObject obj = new GameObject("AudioSource");
            obj.transform.SetParent(transform);
            obj.AddComponent<AudioSource>();
            source = obj.GetComponent<AudioSource>();
            source.playOnAwake = false;
        }
        else
        {
            source = sourceQueue.Peek();
            if (source.isPlaying)
            {
                GameObject obj = new GameObject("AudioSource");
                obj.transform.SetParent(transform);
                obj.AddComponent<AudioSource>();
                source = obj.GetComponent<AudioSource>();
                source.playOnAwake = false;
            }
            else
            {
                sourceQueue.Dequeue();
            }
        }

        source.clip = clip;
        source.volume = volume;
        source.loop = loop;

        source.gameObject.SetActive(true);
        sourceQueue.Enqueue(source);

        return source;
    }
}

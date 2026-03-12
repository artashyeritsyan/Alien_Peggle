using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private int startSourcesCount = 5;
    private List<AudioSource> sources;

    private void Awake()
    {
        sources = new List<AudioSource>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < startSourcesCount; i++)
        {
            CreateSource();
        }
    }

    private AudioSource CreateSource()
    {
        GameObject obj = new GameObject("AudioSource_" + sources.Count);
        obj.transform.SetParent(transform);
        obj.transform.position = Vector3.zero;

        AudioSource source = obj.AddComponent<AudioSource>();
        sources.Add(source);

        return source;
    }

    public static void Play(AudioClip clip)
    {
        if (Instance == null || clip == null)
            return;

        AudioSource freeSource = null;

        for (int i = 0; i < Instance.sources.Count; i++)
        {
            if (!Instance.sources[i].isPlaying)
            {
                freeSource = Instance.sources[i];
                break;
            }
        }

        if (freeSource == null)
        {
            freeSource = Instance.CreateSource();
        }

        freeSource.transform.position = Vector3.zero;
        freeSource.clip = clip;
        freeSource.Play();
    }
}
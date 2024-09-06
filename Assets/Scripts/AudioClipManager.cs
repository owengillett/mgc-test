using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipManager", menuName = "Custom/AudioClipManager", order = 1)]
public class AudioClipManager : ScriptableObject
{
    public AudioClip [] clips;

    static AudioClipManager m_Instance;
    public static AudioClipManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = Resources.Load<AudioClipManager>("AudioClipManager");

            m_Instance.InitClipData();
            return m_Instance;
        }
    }

    public Dictionary<string, AudioClip> m_ClipData = null;

    void InitClipData()
    {
        m_ClipData = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in clips)
        {
            if (!m_ClipData.ContainsKey(clip.name))
                m_ClipData.Add(clip.name, clip);
        }
    }

    public bool GetAudioClip(string id, out AudioClip result)
    {
        if (id != null && m_ClipData.ContainsKey(id))
        {
            result = m_ClipData[id];
            return true;
        }

        result = null;
        return false;
    }
}
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private double musicDuration;
    private double goalTime;
    private int i = 0;

    [Header("------Audio Source------")]
    [SerializeField] AudioSource m_Source;


    [Header("------Audio Clip------")]
    [SerializeField] private AudioClip[] BGM;

    private void Start()
    {
        m_Source.volume = 0.1f;
    }

    private void Update()
    {
        if (AudioSettings.dspTime > goalTime - 1)
        {
            PlayBGM();
            i++;

            if(i == BGM.Length)
            {
                i = 0;
            }
        }
    }

    private void PlayBGM()
    {
        Debug.Log(i);
        goalTime = AudioSettings.dspTime + 3;

        m_Source.clip = BGM[i];
        m_Source.PlayScheduled(goalTime);

        musicDuration = (double)BGM[i].samples / BGM[i].frequency;
        goalTime = goalTime + musicDuration;
    }
}

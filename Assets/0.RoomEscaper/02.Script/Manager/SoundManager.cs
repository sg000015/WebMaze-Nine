using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager<SoundManager>
{

    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioSource sfxAudio;

    public void PlayBgm(int index)
    {
        if (index >= bgms.Length)
        {
            return;
        }
        bgmAudio.clip = bgms[index];
    }



    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] voices;
    [SerializeField] AudioClip[] sfxes;



    public void PlayVoiceSfx(int index)
    {
        return;
        //todo 드래그일경우랑 안겹치게
        //todo 드래그중에는 어지러워요 
        //todo 목소리 변경 

        if (index >= voices.Length)
        {
            return;
        }

        if (sfxAudio.isPlaying)
        {
            return;
        }
        sfxAudio.clip = voices[index];
        sfxAudio.Play();

        // sfxAudio.PlayOneShot(voices[index]);
    }

    ///<summary>
    ///0:Menu, 1:Jump, 2:ItemGet, 3:CoinGet, 4:next
    ///</summary>
    public void PlaySfx(int index)
    {
        sfxAudio.PlayOneShot(sfxes[index]);
    }



}

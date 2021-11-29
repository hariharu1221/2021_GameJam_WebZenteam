using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }

            return instance;
        }
    } 

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip; 
    [SerializeField]
    private AudioClip adventureBgmAudioClip;

    [SerializeField]
    private AudioClip[] sfxAudioClips; 

    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>(); 

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); 

        bgmPlayer = GameObject.Find("BGMSoundPlayer").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SFXSoundPlayer").GetComponent<AudioSource>();

        foreach (AudioClip audioclip in sfxAudioClips)
        {
            audioClipsDic.Add(audioclip.name, audioclip);
        }
    }

    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(name) == false)
        {
            Debug.Log(name + " is not Contained audioClipsDic");
            return;
        }
        sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
    }

    public void PlayBGMSound(float volume = 1f)
    {
        bgmPlayer.loop = true; 
        bgmPlayer.volume = volume * masterVolumeBGM;

        if (SceneManager.GetActiveScene().name == "Main")
        {
            bgmPlayer.clip = mainBgmAudioClip;
            bgmPlayer.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Ex")
        {
            bgmPlayer.clip = adventureBgmAudioClip;
            bgmPlayer.Play();
        }
    }

    private void Start()
    {
        PlayBGMSound(0.2f);
    }
}



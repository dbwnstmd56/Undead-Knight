using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioSource[] bgmPlayers; // 위치에 따라 다른 배경 음악을 재생할 플레이어 배열
    public AudioClip[] bgmClips; // 각 위치에서 재생할 배경 음악 클립 배열
    public int bgmChannels;
    public float bgmVolume;

    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("SoundManager");
                    instance = singleton.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
    public enum Sfx 
    {
        Attack,
        PowerAttack,
        SlashAttack
    }
    public enum Bgm 
    { 
        FirstBgm, 
        TownBgm, 
        BossBgm 
    }
    private void Awake()
    {
        Init();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayBgm(true, 0, Bgm.FirstBgm);
    }
    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannels]; // 배경 음악 플레이어 배열 생성
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>(); // 배경 음악 플레이어 생성 및 설정
            bgmPlayers[i].loop = true;
            bgmPlayers[i].volume = bgmVolume;
        }

        //효과음
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    public void PlayBgm(bool isPlay, int positionIndex, Bgm bgm)
    {
        // 선택한 위치의 오디오 플레이어와 BGM 클립 가져오기
        AudioSource player = bgmPlayers[positionIndex];
        AudioClip clip = bgmClips[(int)bgm];

        // 재생 상태에 따라 재생 또는 정지
        if (isPlay)
        {
            player.clip = clip; // 선택한 위치의 플레이어에 BGM 클립 설정
            player.Play(); // BGM 재생
        }
        else
        {
            player.Stop(); // BGM 정지
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length;i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[i].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioSource[] bgmPlayers; // ��ġ�� ���� �ٸ� ��� ������ ����� �÷��̾� �迭
    public AudioClip[] bgmClips; // �� ��ġ���� ����� ��� ���� Ŭ�� �迭
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
        bgmPlayers = new AudioSource[bgmChannels]; // ��� ���� �÷��̾� �迭 ����
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>(); // ��� ���� �÷��̾� ���� �� ����
            bgmPlayers[i].loop = true;
            bgmPlayers[i].volume = bgmVolume;
        }

        //ȿ����
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
        // ������ ��ġ�� ����� �÷��̾�� BGM Ŭ�� ��������
        AudioSource player = bgmPlayers[positionIndex];
        AudioClip clip = bgmClips[(int)bgm];

        // ��� ���¿� ���� ��� �Ǵ� ����
        if (isPlay)
        {
            player.clip = clip; // ������ ��ġ�� �÷��̾ BGM Ŭ�� ����
            player.Play(); // BGM ���
        }
        else
        {
            player.Stop(); // BGM ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    /** 自分のインスタンス*/
    public static SoundController me;

    /** オーディオソース*/
    private AudioSource[] sources;

    /** 効果音リスト*/
    public enum SE
    {
        START,
        GUAGEUP,
        BOUND,
        SHOOT,
        RESULT_ROLL,
        RESULT,
        HIGHSCORE,
        COUNT
    };

    private AudioClip[] _ses;
    private string[] _seNames =
    {
        "Audio/Se/Decision7",
        "Audio/Se/pekowave1",
        "Audio/Se/stupid3",
        "Audio/Se/flee1",
        "Audio/Se/drum-roll1",
        "Audio/Se/decision18",
        "Audio/Se/trumpet1",
    };
    /** BGMリスト*/
    public enum BGM
    {
        TITLE,
        BGM,
        COUNT
    }
    private AudioClip[] _bgms;
    private string[] _bgmNames =
    {
        "Audio/Bgm/13_yokann",
        "Audio/Bgm/cake5toubun",
    };

    // Use this for initialization
    void Start()
    {
        me = this;
        sources = GetComponents<AudioSource>();

        // サウンドリソース読み込み
        _ses = new AudioClip[(int)SE.COUNT];
        for (int i = 0; i < (int)SE.COUNT; i++)
        {
            _ses[i] = Resources.Load<AudioClip>(_seNames[i]);
        }
        _bgms = new AudioClip[(int)BGM.COUNT];
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            _bgms[i] = Resources.Load<AudioClip>(_bgmNames[i]);
        }
    }

    /**
     * 指定の効果音を鳴らす
     */
    public static void Play(SE snd)
    {
        me.sources[0].PlayOneShot(me._ses[(int)snd]);
    }
    /**
	 * オーディオソースのチャンネルを指定して鳴らす
	 * @params SE snd 鳴らしたい効果音
	 * @params int ch 0=通常 / 1=ループ / 2=1.5倍速
	 */
    public static void Play(SE snd, int ch)
    {
        me.sources[ch].PlayOneShot(me._ses[(int)snd]);
    }

    /**
     * BGMを再生
     */
    public static void PlayBGM(BGM bgm)
    {
        // 違う曲の場合、先の曲を停止してから、再生
        if (me.sources[1].clip != me._bgms[(int)bgm])
        {
            me.sources[1].Stop();
            me.sources[1].clip = me._bgms[(int)bgm];
            me.sources[1].Play();
        }
        else
        {
            // 設定済みの曲の場合、再生中のときは何もしない
            if (!me.sources[1].isPlaying)
            {
                me.sources[1].Play();
            }
        }
    }

    /**
     * BGMを停止
     */
    public static void StopBGM()
    {
        me.sources[1].Stop();
    }

    /**
     * BGMのボリュームを設定
     */
    public static void SetBGMVolume(float vol)
    {
        me.sources[1].volume = vol;
    }

    /**
     * 音を停止
     */
    public static void Stop()
    {
        me.sources[0].Stop();
    }
}
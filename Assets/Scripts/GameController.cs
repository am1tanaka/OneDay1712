using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour {
    private static GameController instance;

    [TooltipAttribute("デバッグのフラグ")]
    public bool IS_DEBUG = true;
    /** 外部から参照できるデバッグフラグ*/
    public static bool isDebug
    {
        get
        {
            return instance.IS_DEBUG;
        }
    }

    public enum SCENE
    {
        TITLE,
        GAME,
        NONE
    }

    [TooltipAttribute("開始時のシーン")]
    public SCENE StartScene = SCENE.TITLE;
    /** 現在のシーン*/
    [SerializeField]
    private SCENE nowScene;
    /** 次のシーン*/
    private SCENE nextScene;

    [TooltipAttribute("タイトルのバーチャルカメラ")]
    public GameObject titleVMCamera;

    /** ゲームシーケンス*/
    public enum GAME_PHASE
    {
        START_WAIT, // 開始待ち
        SET_TARGET, // 目標設定
        POWER,      // パワーゲージ
        FLYING,     // 飛ぶ
        RESULT,     // 結果
        NONE
    }
    [SerializeField]
    private GAME_PHASE _nowPhase;
    /** 現在のフェーズ*/
    public static GAME_PHASE NowPhase
    {
        get { return instance._nowPhase; }
    }
    /** 次のフェーズ*/
    private GAME_PHASE nextPhase = GAME_PHASE.NONE;

    /** ゲームを開始する時のロゴのアニメーションタイムライン*/
    public PlayableDirector timelineTitleStart;

    /** ゲーム開始からの待ち時間*/
    public float START_WAIT_TIME = 1f;

    /** プレイヤーオブジェクト*/
    public GameObject Player;

    private void Awake()
    {
        instance = this;
        nextScene = StartScene;
    }

    // Use this for initialization
    void Start () {
		
	}

    /** シーンの初期化処理*/
    void procInit()
    {
        switch (nowScene)
        {
            case SCENE.TITLE:
                titleVMCamera.SetActive(true);
                StartCoroutine(updateTitle());
                break;
            case SCENE.GAME:
                titleVMCamera.SetActive(false);
                nextPhase = GAME_PHASE.START_WAIT;
                timelineTitleStart.Play();
                StartCoroutine(updateGame());
                break;
        }
    }

    /** 次のフェーズを設定する*/
    public static void ChangePhaser(GAME_PHASE ph)
    {
        instance.nextPhase = ph;
    }

    /** シーンを切り替える*/
    public static void ChangeScene(SCENE next)
    {
        instance.nextScene = next;
    }

    // Update is called once per frame
    void Update () {
		// シーンの切り替えチェック
        if (nextScene != SCENE.NONE)
        {
            nowScene = nextScene;
            nextScene = SCENE.NONE;
            procInit();
        }
	}

    /** タイトルシーンの更新処理*/
    IEnumerator updateTitle()
    {
        while (true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                break;
            }
            yield return null;
        }
        nextScene = SCENE.GAME;
    }

    [TooltipAttribute("標的のUIのオブジェクト")]
    public GameObject uiTarget;

    [TooltipAttribute("マウスの場所を3D空間で示すオブジェクトの位置")]
    public Transform mouseTarget;

    /** フェーズの初期化*/
    void procInitPhase()
    {
        // フェーズの切り替え
        if (nextPhase != GAME_PHASE.NONE)
        {
            _nowPhase = nextPhase;
            nextPhase = GAME_PHASE.NONE;

            switch (NowPhase)
            {
                case GAME_PHASE.SET_TARGET:
                    uiTarget.SetActive(true);
                    Player.SendMessage("StartSetTarget");
                    break;

                    // 
                case GAME_PHASE.POWER:
                    uiTarget.SetActive(false);
                    Player.SendMessage("StartPower", mouseTarget.position);
                    break;
            }
        }
    }

    /** ゲームシーンの更新処理*/
    IEnumerator updateGame()
    {
        // ゲーム開始待ち
        yield return new WaitForSeconds(START_WAIT_TIME);

        nextPhase = GAME_PHASE.SET_TARGET;
        procInitPhase();

        // 操作
        while (NowPhase == GAME_PHASE.SET_TARGET || NowPhase == GAME_PHASE.POWER)
        {
            procInitPhase();
            yield return null;
        }

        // 飛び終わりを待つ
        while (NowPhase == GAME_PHASE.FLYING)
        {
            yield return null;
        }

        // シーンをタイトルに切り替える
        nextScene = SCENE.TITLE;
    }

}

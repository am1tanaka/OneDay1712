using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

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
    [TooltipAttribute("ターゲット設定のバーチャルカメラ")]
    public GameObject targetVMCamera;
    [TooltipAttribute("飛び中のバーチャルカメラ")]
    public GameObject flyingVMCamera;

    [TooltipAttribute("キロクのアニメーター")]
    public Animator KirokuAnime;
    [TooltipAttribute("キロクの数値のアニメーター")]
    public Animator KirokuNumAnime;

    [TooltipAttribute("キロク数値のテキスト")]
    public TextMeshProUGUI textKirokuNum;

    /** ゲームシーケンス*/
    public enum GAME_PHASE
    {
        START_WAIT, // 開始待ち
        SET_TARGET, // 目標設定
        POWER,      // パワーゲージ
        FLYING,     // 飛ぶ
        RESULT,     // 結果
        RESULT_COUNT,   // 数え上げ
        RESULT_DONE,    // 完了アニメ待ち
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

    [TooltipAttribute("タイトルを表示する時のロゴのアニメーションタイムライン")]
    public PlayableDirector timelineTitleDisp;
    [TooltipAttribute("ゲームを開始する時のロゴのアニメーションタイムライン")]
    public PlayableDirector timelineTitleStart;

    /** ゲーム開始からの待ち時間*/
    public float START_WAIT_TIME = 1f;

    /** プレイヤーオブジェクト*/
    public GameObject Player;

    /** プレイヤーの開始地点*/
    private Vector3 playerStartPosition;

    private void Awake()
    {
        instance = this;
        nextScene = StartScene;
        playerStartPosition = Player.transform.position;
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
                flyingVMCamera.SetActive(false);
                targetVMCamera.SetActive(false);
                StartCoroutine(updateTitle());
                timelineTitleDisp.Stop();
                timelineTitleDisp.time = 0;
                timelineTitleDisp.Play();
                Player.transform.position = playerStartPosition;
                SoundController.PlayBGM(SoundController.BGM.TITLE);
                break;
            case SCENE.GAME:
                targetVMCamera.SetActive(true);
                nextPhase = GAME_PHASE.START_WAIT;
                timelineTitleStart.Stop();
                timelineTitleStart.time = 0;
                timelineTitleStart.Play();
                StartCoroutine(updateGame());
                SoundController.PlayBGM(SoundController.BGM.BGM);
                break;
        }
    }

    /** 次のフェーズを設定する*/
    public static void ChangePhase(GAME_PHASE ph)
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
                SoundController.Play(SoundController.SE.START);
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

    [TooltipAttribute("最高速度")]
    public float MAX_SPEED = 100f;

    [TooltipAttribute("1秒辺りのカウント距離")]
    public float COUNTUP_RATE = 30f;

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
                    SoundController.Stop();
                    break;

                    // 
                case GAME_PHASE.POWER:
                    uiTarget.SetActive(false);
                    Player.SendMessage("StartPower", mouseTarget.position);
                    break;

                case GAME_PHASE.FLYING:
                    // Duckに速度設定
                    Vector3 dir = (mouseTarget.transform.position-Player.transform.position).normalized;
                    Vector3 add = dir * MAX_SPEED * (float)PlayerPower.power / 100f;
                    Player.GetComponent<Rigidbody>().velocity = add;
                    // カメラのターゲットをDuckに変更
                    flyingVMCamera.SetActive(true);
                    // 飛翔音
                    SoundController.Play(SoundController.SE.SHOOT);
                    break;

                case GAME_PHASE.RESULT:
                    // タイトルカメラに戻す
                    flyingVMCamera.SetActive(false);
                    targetVMCamera.SetActive(false);
                    SoundController.StopBGM();

                    // キロク表示
                    KirokuAnime.SetTrigger("In");
                    break;

                // カウント開始
                case GAME_PHASE.RESULT_COUNT:
                    KirokuNumAnime.SetTrigger("In");
                    SoundController.Play(SoundController.SE.RESULT_ROLL);
                    break;

                case GAME_PHASE.RESULT_DONE:
                    // 終了アニメ
                    KirokuAnime.SetTrigger("Out");
                    KirokuNumAnime.SetTrigger("Out");
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

        // 各状態の完了を待つ
        while (
            NowPhase == GAME_PHASE.SET_TARGET
            || NowPhase == GAME_PHASE.POWER
            || NowPhase == GAME_PHASE.FLYING
            || NowPhase == GAME_PHASE.RESULT)
        {
            procInitPhase();
            yield return null;
        }

        // 結果を表示
        float kiroku = (Player.transform.position - playerStartPosition).magnitude;
        for (float x=0f; x<=kiroku; x+=COUNTUP_RATE*Time.deltaTime)
        {
            textKirokuNum.text = x.ToString("F2") + "m";
            yield return null;
        }
        textKirokuNum.text = kiroku.ToString("F2") + "m";
        SoundController.Stop();
        SoundController.Play(SoundController.SE.RESULT);
        yield return null;

        // クリックされるのを待つ
        while (true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ChangePhase(GAME_PHASE.RESULT_DONE);
                procInitPhase();
                SoundController.Play(SoundController.SE.START);
                break;
            }
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManualController : MonoBehaviour {

    private static ManualController instance;
    private static TextMeshProUGUI tmpro;

    // 表示するテキスト
    private static string[] MANUAL = {
        "<color=#f03535>マウス</color>を動かして、<color=#35ff35>とびたい方向</color>を決めます。\n方向が決まったら、<color=#f03535>左クリック</color>！",
        "<color=#f03535>左クリック</color>：はっしゃ！　　　　\n<color=#3535f0>右クリック</color>：向きのえらびなおし"
    };

    private void Awake()
    {
        instance = this;
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.enabled = false;
    }

    /**
     * 説明を設定する。配列外のパラメーターを渡すと非表示にする
     * @params int pg ページ数(0以上)
     */
    public static void Page(int pg)
    {
        if (pg>=0 && pg<MANUAL.Length)
        {
            tmpro.enabled = true;
            tmpro.text = MANUAL[pg];
        }
        else
        {
            tmpro.enabled = false;
        }
    }
}

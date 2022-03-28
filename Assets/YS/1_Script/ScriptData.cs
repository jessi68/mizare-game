using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    struct ScriptData
    {
        public static readonly DialogScript[] scripts = new DialogScript[5]
        {
            new DialogScript("미자르", "미자르가 이야기합니다...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            }),
            new DialogScript("알코르", "<link=v_wave>알코르가 이야기합니다...</link>", () => {
                GameManager.Instance.leftSideImg.color = Color.white;
                GameManager.Instance.rightSideImg.color = new Color(0.3f, 0.3f, 0.3f);
            }),
            new DialogScript("미자르", "미자르가 두번째 이야기합니다...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            }),
            new DialogScript("알코르", "<link=shake>알코르</link>가 이야기합니다...", () => {
                GameManager.Instance.leftSideImg.color = Color.white;
                GameManager.Instance.rightSideImg.color = new Color(0.3f, 0.3f, 0.3f);
            }),
            new DialogScript("미자르", "미자르가 <color=#ff0000>마저</color> 이야기합니다...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            })
        };
    }
}
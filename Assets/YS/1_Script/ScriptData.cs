using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    struct ScriptData
    {
        public static readonly DialogScript[] scripts = new DialogScript[5]
        {
            new DialogScript("���ڸ�", "���ڸ��� �̾߱��մϴ�...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            }),
            new DialogScript("���ڸ�", "<link=v_wave>���ڸ��� �̾߱��մϴ�...</link>", () => {
                GameManager.Instance.leftSideImg.color = Color.white;
                GameManager.Instance.rightSideImg.color = new Color(0.3f, 0.3f, 0.3f);
            }),
            new DialogScript("���ڸ�", "���ڸ��� �ι�° �̾߱��մϴ�...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            }),
            new DialogScript("���ڸ�", "<link=shake>���ڸ�</link>�� �̾߱��մϴ�...", () => {
                GameManager.Instance.leftSideImg.color = Color.white;
                GameManager.Instance.rightSideImg.color = new Color(0.3f, 0.3f, 0.3f);
            }),
            new DialogScript("���ڸ�", "���ڸ��� <color=#ff0000>����</color> �̾߱��մϴ�...", () => {
                GameManager.Instance.leftSideImg.color = new Color(0.3f, 0.3f, 0.3f);
                GameManager.Instance.rightSideImg.color = Color.white;
            })
        };
    }
}
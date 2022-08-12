using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public enum TYPING_SPEED
    {
        SLOW,
        NORMAL,
        FAST
    }
    public class SettingManager : Singleton<SettingManager>
    {
        /// <summary>
        /// �������� �ؽ�Ʈ �̸����� ���
        /// </summary>
        public static IEnumerator TextPreview(CustomTMPEffect previewTMP)
        {
            WaitForSeconds wf100ms = CachedWaitForSeconds.Get(0.1f);
            WaitForSeconds wf1s = CachedWaitForSeconds.Get(1.0f);

            while (true)
            {
                yield return wf100ms;

                if (previewTMP.IsDoneTyping)
                {
                    yield return wf1s;
                    previewTMP.SetText("�ȳ��ϼ���. �ؽ�Ʈ Ÿ���� �ӵ� �̸������Դϴ�.\n<link=v_wave>����ġ�� ����</link>�� <link=shake>��鸮�� ����</link>�Դϴ�.");
                }
            }
        }
        /// <summary>
        /// Ÿ���� �ӵ� ����
        /// </summary>
        /// <param name="level">�ӵ� �ܰ�</param>
        public static void ChangeTypingSpeed(TYPING_SPEED level)
        {
            switch (level)
            {
                case TYPING_SPEED.SLOW:
                    CustomTMPEffect.TypingSpeed = 0.1f;
                    break;
                case TYPING_SPEED.NORMAL:
                    CustomTMPEffect.TypingSpeed = 0.05f;
                    break;
                case TYPING_SPEED.FAST:
                    CustomTMPEffect.TypingSpeed = 0.025f;
                    break;
            }
        }
        public static TYPING_SPEED GetTypingSpeed()
        {
            if (CustomTMPEffect.TypingSpeed == 0.1f)
                return TYPING_SPEED.SLOW;
            else if (CustomTMPEffect.TypingSpeed == 0.05f)
                return TYPING_SPEED.NORMAL;
            else
                return TYPING_SPEED.FAST;
        }
    }
}
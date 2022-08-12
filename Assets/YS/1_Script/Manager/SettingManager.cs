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
        /// 설정에서 텍스트 미리보기 기능
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
                    previewTMP.SetText("안녕하세요. 텍스트 타이핑 속도 미리보기입니다.\n<link=v_wave>물결치는 글자</link>와 <link=shake>흔들리는 글자</link>입니다.");
                }
            }
        }
        /// <summary>
        /// 타이핑 속도 변경
        /// </summary>
        /// <param name="level">속도 단계</param>
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
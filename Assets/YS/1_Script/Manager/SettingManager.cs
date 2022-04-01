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
        private CustomTMPEffect dialogTMP;
        private CustomTMPEffect previewTMP;
        private AudioSource audioBGM;
        private float typingSpeed;

        public float TypingSpeed => typingSpeed;

        /// <summary>
        /// 매 씬이 로드될때마다 초기화하는 함수
        /// </summary>
        public static void Initialize()
        {
            CustomTMPEffect[] tmps = FindObjectsOfType<CustomTMPEffect>(true);

            // 씬에 CustomTMPEffect는 단 2개 있을 예정
            if (tmps.Length == 1)
            {
                // 한개라면 타이틀 화면인 경우(프리뷰TMP만 존재)
                Instance.previewTMP = tmps[0];
            }
            else
            {
                // 두개라면 인게임 화면이므로 Tag를 통해 Dialog와 Preview를 구분
                if (tmps[0].tag == "Dialog")
                {
                    Instance.dialogTMP = tmps[0];
                    Instance.previewTMP = tmps[1];
                }
                else
                {
                    Instance.dialogTMP = tmps[1];
                    Instance.previewTMP = tmps[0];
                }
            }

            Instance.audioBGM = Instance.GetComponent<AudioSource>();
        }

        /// <summary>
        /// 설정에서 텍스트 미리보기 기능
        /// </summary>
        public static IEnumerator TextPreview()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (Instance.previewTMP.IsDoneTyping)
                {
                    yield return new WaitForSeconds(1.0f);
                    Instance.previewTMP.SetText("안녕하세요. 텍스트 타이핑 속도 미리보기입니다.\n<link=v_wave>물결치는 글자</link>와 <link=shake>흔들리는 글자</link>입니다.");
                }
            }
        }
        /// <summary>
        /// 설정창에서 타이핑 스피드 슬라이더의 이벤트 함수
        /// </summary>
        /// <param name="slider">조작된 슬라이더UI</param>
        public static void OnChangeTypingSpeedSlider(Slider slider)
        {
            ChangeTypingSpeed((TYPING_SPEED)slider.value);
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
                    Instance.typingSpeed = 0.1f;
                    break;
                case TYPING_SPEED.NORMAL:
                    Instance.typingSpeed = 0.05f;
                    break;
                case TYPING_SPEED.FAST:
                    Instance.typingSpeed = 0.025f;
                    break;
            }

            if (Instance.dialogTMP != null)
                Instance.dialogTMP.typingSpeed = Instance.typingSpeed;
            Instance.previewTMP.typingSpeed = Instance.typingSpeed;
        }
    }
}
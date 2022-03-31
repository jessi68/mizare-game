using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class SettingManager : Singleton<SettingManager>
    {
        private CustomTMPEffect dialogTMP;
        private CustomTMPEffect previewTMP;
        private AudioSource audioBGM;
        private float typingSpeed;

        public float TypingSpeed => typingSpeed;
        
        public static void Initialize()
        {
            TMPContainer tmpContainer = GameObject.FindGameObjectWithTag("TMPContainer").GetComponent<TMPContainer>();
            Instance.dialogTMP = tmpContainer.dialog;
            Instance.previewTMP = tmpContainer.preview;
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
        public static void OnChangeTypingSpeedSlider(Slider slider)
        {
            ChangeTypingSpeed((int)slider.value);
        }
        public static void ChangeTypingSpeed(int level)
        {
            switch (level)
            {
                case 0:
                    Instance.typingSpeed = 0.1f;
                    break;
                case 1:
                    Instance.typingSpeed = 0.05f;
                    break;
                case 2:
                    Instance.typingSpeed = 0.025f;
                    break;
            }

            if (Instance.dialogTMP != null)
                Instance.dialogTMP.typingSpeed = Instance.typingSpeed;
            Instance.previewTMP.typingSpeed = Instance.typingSpeed;
        }
    }
}
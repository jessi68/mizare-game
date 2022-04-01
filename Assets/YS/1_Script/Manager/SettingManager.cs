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
        /// �� ���� �ε�ɶ����� �ʱ�ȭ�ϴ� �Լ�
        /// </summary>
        public static void Initialize()
        {
            CustomTMPEffect[] tmps = FindObjectsOfType<CustomTMPEffect>(true);

            // ���� CustomTMPEffect�� �� 2�� ���� ����
            if (tmps.Length == 1)
            {
                // �Ѱ���� Ÿ��Ʋ ȭ���� ���(������TMP�� ����)
                Instance.previewTMP = tmps[0];
            }
            else
            {
                // �ΰ���� �ΰ��� ȭ���̹Ƿ� Tag�� ���� Dialog�� Preview�� ����
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
        /// �������� �ؽ�Ʈ �̸����� ���
        /// </summary>
        public static IEnumerator TextPreview()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (Instance.previewTMP.IsDoneTyping)
                {
                    yield return new WaitForSeconds(1.0f);
                    Instance.previewTMP.SetText("�ȳ��ϼ���. �ؽ�Ʈ Ÿ���� �ӵ� �̸������Դϴ�.\n<link=v_wave>����ġ�� ����</link>�� <link=shake>��鸮�� ����</link>�Դϴ�.");
                }
            }
        }
        /// <summary>
        /// ����â���� Ÿ���� ���ǵ� �����̴��� �̺�Ʈ �Լ�
        /// </summary>
        /// <param name="slider">���۵� �����̴�UI</param>
        public static void OnChangeTypingSpeedSlider(Slider slider)
        {
            ChangeTypingSpeed((TYPING_SPEED)slider.value);
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
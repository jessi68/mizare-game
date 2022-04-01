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
        
        /// <summary>
        /// �� ���� �ε�ɶ����� �ʱ�ȭ�ϴ� �Լ�
        /// </summary>
        public static void Initialize()
        {
            TMPContainer tmpContainer = GameObject.FindGameObjectWithTag("TMPContainer").GetComponent<TMPContainer>();
            Instance.dialogTMP = tmpContainer.dialog;
            Instance.previewTMP = tmpContainer.preview;
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
            ChangeTypingSpeed((int)slider.value);
        }
        /// <summary>
        /// Ÿ���� �ӵ� ����
        /// </summary>
        /// <param name="level">�ӵ� �ܰ�</param>
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
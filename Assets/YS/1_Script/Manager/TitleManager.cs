using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace YS
{
    public class TitleManager : MonoBehaviour
    {
        public enum TITLE_UI_STATE
        {
            TOUCH_TO_START,
            MENU,
            SETTING,
            GAMESTART,
            LOAD,
            GALLERY
        }
        #region Field
        [BoxGroup("�г�", true, true)]
        [LabelText("��ġ���� �г�")]
        public GameObject clickToStartPanel;
        [BoxGroup("�г�")]
        [LabelText("�޴� �г�"), Tooltip("��� �޴����� ���� �г�")]
        public GameObject menuPanel;
        [BoxGroup("�г�")]
        [LabelText("���� �г�")]
        public GameObject settingPanel;
        [BoxGroup("�г�")]
        [LabelText("���� �г�")]
        public GameObject mainPanel;
        [BoxGroup("�г�")]
        [LabelText("���ӽ��� �г�")]
        public GameObject startPanel;
        [BoxGroup("�г�")]
        [LabelText("�ҷ����� �г�")]
        public GameObject loadPanel;
        [BoxGroup("�г�")]
        [LabelText("����ø �г�")]
        public GameObject galleryPanel;

        [BoxGroup("���� �޴�", true, true)]
        [LabelText("���� ��ư")]
        public Button settingBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("�ڷΰ��� ��ư")]
        public Button backBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("���ӽ��� ��ư")]
        public Button startBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("�ҷ����� ��ư")]
        public Button loadBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("����ø ��ư")]
        public Button galleryBtn;

        [BoxGroup("���� �޴�/����", true, true)]
        [LabelText("���� �����̴�")]
        public Slider volumeSlider;
        [BoxGroup("���� �޴�/����")]
        [LabelText("Ÿ���� �����̴�")]
        public Slider typingSlider;
        [BoxGroup("���� �޴�/����")]
        [LabelText("Ÿ���� �̸�����")]
        public CustomTMPEffect previewTMP;

        [BoxGroup("���� �޴�/���ӽ���", true, true)]
        [LabelText("����1 ��ư")]
        public Button slot1StartBtn;
        [BoxGroup("���� �޴�/���ӽ���")]
        [LabelText("����2 ��ư")]
        public Button slot2StartBtn;
        [BoxGroup("���� �޴�/���ӽ���")]
        [LabelText("����3 ��ư")]
        public Button slot3StartBtn;

        [BoxGroup("���� �޴�/�ҷ�����", true, true)]
        [LabelText("������ ��ư")]
        public Button quickSlotLoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����1 ��ư")]
        public Button slot1LoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����2 ��ư")]
        public Button slot2LoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����3 ��ư")]
        public Button slot3LoadBtn;

        private Coroutine coroutineTextPreview;
        // UI ���� ����
        private Stack<TITLE_UI_STATE> stateStack = new Stack<TITLE_UI_STATE>();
        #endregion

        #region Properties
        public TITLE_UI_STATE CurrentState => stateStack.Peek();
        #endregion

        #region Unity Methods
        private void Start()
        {
            stateStack.Push(TITLE_UI_STATE.TOUCH_TO_START);

            settingBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.SETTING); });
            backBtn.onClick.AddListener(() => { PopState(); });
            startBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.GAMESTART); });
            loadBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.LOAD); });
            galleryBtn.onClick.AddListener(() => { PushState(TITLE_UI_STATE.GALLERY); });

            volumeSlider.onValueChanged.AddListener((float value) => { AudioManager.BaseBGMVolume = value; });
            typingSlider.onValueChanged.AddListener((float value) => { SettingManager.ChangeTypingSpeed((TYPING_SPEED)value); });
            volumeSlider.value = AudioManager.BaseBGMVolume;
            typingSlider.value = (float)SettingManager.GetTypingSpeed();

            slot1StartBtn.onClick.AddListener(() => { OnNewGame(1); });
            slot2StartBtn.onClick.AddListener(() => { OnNewGame(2); });
            slot3StartBtn.onClick.AddListener(() => { OnNewGame(3); });

            quickSlotLoadBtn.onClick.AddListener(() => { OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { OnStartGame(3); });
        }
        private void Update()
        {
            if (clickToStartPanel.activeInHierarchy && Input.anyKeyDown)
            {
                PopState();
                PushState(TITLE_UI_STATE.MENU);
            }
        }
        #endregion

        #region Methods
        #region UIEvent
        public void PushState(TITLE_UI_STATE pushState)
        {
            stateStack.Push(pushState);

            switch (pushState)
            {
                case TITLE_UI_STATE.MENU:
                    menuPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.SETTING:
                    settingPanel.SetActive(true);
                    coroutineTextPreview = StartCoroutine(SettingManager.TextPreview(previewTMP));
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    mainPanel.SetActive(false);
                    startPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    mainPanel.SetActive(false);
                    loadPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    mainPanel.SetActive(false);
                    galleryPanel.SetActive(true);
                    break;
            }
        }
        public void PopState()
        {
            switch (CurrentState)
            {
                case TITLE_UI_STATE.TOUCH_TO_START:
                    clickToStartPanel.SetActive(false);
                    break;
                case TITLE_UI_STATE.MENU:
                    // ���� ����
                    // ...
                    break;
                case TITLE_UI_STATE.SETTING:
                    StopCoroutine(coroutineTextPreview);
                    previewTMP.SkipTyping();
                    settingPanel.SetActive(false);
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    startPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    loadPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    galleryPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
            }
        }
        #endregion


        /// <summary>
        /// ���ο� ������ �����Ѵ�.
        /// </summary>
        /// <param name="saveIndex">���ο� ������ ������ ���彽��</param>
        public void OnNewGame(int saveIndex)
        {
            // ���ο� �����̹Ƿ�, saveIndex�� �ش��ϴ� ���̺굥���͸� ����� �� �������� ����� (���� ��� ���ο� ���ӿ� ���� ���̺굥���͸� �������� ������)
            OnOverwriteGame(saveIndex, new SaveData());

            // �� ���̺����Ͽ� ���ο� ������ �����ϴ°��� �ε���Ӱ� ���� ����� �ϹǷ� �ε���ӽ� ȣ��Ǵ� �Լ��� ���� �Լ� ȣ��
            OnStartGame(saveIndex);
        }
        /// <summary>
        /// newData�� saveIndex���Կ� �����
        /// </summary>
        /// <param name="saveIndex">������ save���� ��ȣ</param>
        /// <param name="newData">���Ӱ� ������ ������</param>
        public void OnOverwriteGame(int saveIndex, SaveData newData)
        {
            // saveIndex�� �ش� ���̺� ������ ���ͼ� newData�� ó��
            // ...
        }

        /// <summary>
        /// ���õ� save������ ������ �а� �ش� ������ �������� ������ �����Ѵ�
        /// </summary>
        /// <param name="saveIndex">save���� ��ȣ</param>
        public void OnStartGame(int saveIndex)
        {
            // �� ��ȯ�ϱ� �� ��� ���̺긦 ȣ���ߴ��� �ΰ��Ӿ����� �˾ƾ��ϹǷ� Ư�� ��ü�� ������ ��� DontDestroyOnLoad(������ ��� ��ü); �Լ� ȣ��
            // ...
            
            SceneManager.LoadScene(1);
            // �� ��ȯ
        }
        #endregion
    }
}
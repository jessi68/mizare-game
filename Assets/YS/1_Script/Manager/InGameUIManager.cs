using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace YS
{
    public class InGameUIManager : Singleton<InGameUIManager>
    {
        public enum INGAME_UI_STATE
        {
            GAME,
            INVEN,
            MENU,
            SAVE,
            LOAD,
            SETTING,
            LOG,
            EXIT
        }

        #region Field
        [BoxGroup("�г�", true, true)]
        [LabelText("�κ��丮 UI")]
        public InventoryComponent invenComp;
        [BoxGroup("�г�")]
        [LabelText("���θ޴� UI")]
        public GameObject ui;
        [BoxGroup("�г�")]
        [LabelText("�޴� �г� UI")]
        public SlideEffect menuPanel;
        [BoxGroup("�г�")]
        [LabelText("���̺� �г� UI")]
        public SlideEffect savePanel;
        [BoxGroup("�г�")]
        [LabelText("�α� �г� UI")]
        public GameObject logPanel;
        [BoxGroup("�г�")]
        [LabelText("���� �г� UI")]
        public GameObject settingPanel;

        [BoxGroup("UI", true, true)]
        [LabelText("�޴� ��ư")]
        public Button menuBtn;
        [BoxGroup("UI")]
        [LabelText("�κ��丮 ��ư")]
        public Button invenBtn;
        [BoxGroup("UI/�κ��丮", true, true)]
        [LabelText("�κ��丮 �ݱ� ��ư")]
        public Button invenExitBtn;
        [BoxGroup("UI/�޴�", true, true)]
        [LabelText("���� ��ư")]
        public Button saveBtn;
        [BoxGroup("UI/�޴�/����", true, true)]
        [LabelText("����1")]
        public Button slot1SaveBtn;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("����2")]
        public Button slot2SaveBtn;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("����3")]
        public Button slot3SaveBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("�ҷ����� ��ư")]
        public Button loadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����", true, true)]
        [LabelText("������")]
        public Button quickSlotLoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����1")]
        public Button slot1LoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����2")]
        public Button slot2LoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����3")]
        public Button slot3LoadBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("���� ��ư")]
        public Button settingBtn;
        [BoxGroup("UI/�޴�/����", true, true)]
        [LabelText("���� �ݱ� ��ư")]
        public Button closeSettingBtn;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("���� �����̴�")]
        public Slider volumeSlider;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("Ÿ���� �ӵ� �����̴�")]
        public Slider typingSlider;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("Ÿ���� �̸�����")]
        public CustomTMPEffect previewTMP;
        [BoxGroup("UI/�޴�")]
        [LabelText("�α� ��ư")]
        public Button logBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("���� ��ư")]
        public Button exitBtn;

        // UI ���� ����
        private Stack<INGAME_UI_STATE> stateStack = new Stack<INGAME_UI_STATE>();
        private Coroutine coroutineTextPreview;
        #endregion

        #region Properties
        public bool IsShowingInventory => stateStack.Count != 0 && stateStack.Peek() == INGAME_UI_STATE.INVEN;
        [ShowInInspector]
        public INGAME_UI_STATE CurrentState => stateStack.Count == 0 ? INGAME_UI_STATE.GAME : stateStack.Peek();
        public Stack<INGAME_UI_STATE> StateStack => stateStack;
        #endregion

        #region Unity Methods
        private void Start()
        {
            // ��ư�鿡 �̺�Ʈ ���
            menuBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.MENU); });
            
            invenBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.INVEN); });
            invenExitBtn.onClick.AddListener(() => { PopState(stateStack.Pop()); });
            
            saveBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.SAVE); });
            slot1SaveBtn.onClick.AddListener(() => { OnOverwriteGame(1, GameManager.Instance.CurrentData); });
            slot2SaveBtn.onClick.AddListener(() => { OnOverwriteGame(2, GameManager.Instance.CurrentData); });
            slot3SaveBtn.onClick.AddListener(() => { OnOverwriteGame(3, GameManager.Instance.CurrentData); });
            
            loadBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.LOAD); });
            quickSlotLoadBtn.onClick.AddListener(() => { OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { OnStartGame(3); });
            
            settingBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.SETTING); });
            closeSettingBtn.onClick.AddListener(() => { PopState(stateStack.Pop()); });
            volumeSlider.onValueChanged.AddListener((float value) => { AudioManager.BGMVolume = value; });
            typingSlider.onValueChanged.AddListener((float value) => { SettingManager.ChangeTypingSpeed((TYPING_SPEED)value); });
            volumeSlider.value = AudioManager.BGMVolume;
            typingSlider.value = (float)SettingManager.GetTypingSpeed();
            
            logBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.LOG); });
            exitBtn.onClick.AddListener(() => { OnPushState(INGAME_UI_STATE.EXIT); });
        }

        private void Update()
        {
            // escŰ ������ UI Pop
            if (stateStack.Count > 0 && Input.GetKeyDown(KeyCode.Escape))
                PopState(stateStack.Pop());
        }
        #endregion

        #region Methods
        public static bool IsGameState()
        {
            return Instance.stateStack.Count == 0;
        }
        /// <summary>
        /// UI���� ���ܰ�� ����
        /// </summary>
        /// <param name="popState">���ŵ� ����</param>
        public void PopState(INGAME_UI_STATE popState)
        {
            switch (popState)
            {
                case INGAME_UI_STATE.INVEN:
                    invenComp.CloseInventory();
                    ui.SetActive(true);
                    break;
                case INGAME_UI_STATE.MENU:
                    menuPanel.SetSlide(new Vector2(-500.0f, 0.0f), true);
                    break;
                case INGAME_UI_STATE.SAVE:
                case INGAME_UI_STATE.LOAD:
                    savePanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case INGAME_UI_STATE.SETTING:
                    StopCoroutine(coroutineTextPreview);
                    settingPanel.SetActive(false);
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(false);
                    break;
                case INGAME_UI_STATE.EXIT:
                    SceneManager.LoadScene("Seoyoon/Scenes/SeoyoonScene");
                    break;
            }
        }
        #region State Methods
        /// <summary>
        /// UI ���� �߰�
        /// </summary>
        /// <param name="pushState">�߰��� ����</param>
        private void OnPushState(INGAME_UI_STATE pushState)
        {
            if (stateStack.Count > 1)
            {
                INGAME_UI_STATE preState = stateStack.Pop();
                PopState(preState);
                if (preState == pushState)
                    return;
            }
            stateStack.Push(pushState);

            switch (pushState)
            {
                case INGAME_UI_STATE.INVEN:
                    ui.SetActive(false);
                    invenComp.OpenInventory();
                    break;
                case INGAME_UI_STATE.MENU:
                    if (stateStack.Count > 1)
                    {
                        stateStack.Pop();
                        while (stateStack.Count > 0)
                            PopState(stateStack.Pop());
                    }
                    else
                    {
                        menuPanel.gameObject.SetActive(true);
                        menuPanel.SetSlide(Vector3.zero, false);
                    }
                    break;
                case INGAME_UI_STATE.SAVE:
                case INGAME_UI_STATE.LOAD:
                    savePanel.gameObject.SetActive(true);
                    savePanel.SetSlide(new Vector2(0.0f, -340.0f), false);
                    break;
                case INGAME_UI_STATE.SETTING:
                    settingPanel.SetActive(true);
                    coroutineTextPreview = StartCoroutine(SettingManager.TextPreview(previewTMP));
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(true);
                    break;
                case INGAME_UI_STATE.EXIT:
                    ExitGame();
                    break;
            }
        }
        public void ExitGame()
        {
            SceneManager.LoadScene("YS/0_Scene/TitleProto");
            // Ÿ��Ʋ ������ ���ư��� ����
            // �ʿ��ϴٸ� �����Ұ��� ����°͵� �����ؾ��ҵ�
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

            SceneManager.LoadScene("YS/0_Scene/YSTestScene");
            // �� ��ȯ
        }
        #endregion
        #endregion
    }
}
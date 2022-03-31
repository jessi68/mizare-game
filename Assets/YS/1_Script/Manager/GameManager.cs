using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public struct SaveData
    {
        public int scriptIndex;
    }

    public class GameManager : Singleton<GameManager>
    {
        enum STATE
        {
            GAME,
            MENU,
            SAVE,
            LOAD,
            GALLERY,
            LOG
        }

        #region Field
        public SlideEffect menuPanel;
        public SlideEffect savePanel;

        public Image leftSideImg;
        public Image rightSideImg;

        public TMP_Text nameTMP;
        public CustomTMPEffect scriptTMP;
        public GameObject logUI;
        public TMP_Text logTMP;

        private static SaveData[] saveDatas = new SaveData[3];
        // UI ���� ����
        private STATE state;
        private int scriptIndex;
        private string log;
        #endregion

        #region Unity Methods
        void Start()
        {
            state = STATE.GAME;
            SetDialog();
        }
        void Update()
        {
            // UI�� �������� �ʰ� UI�� �ƴѰ��� ���콺 Ŭ���� �Ͼ����
            if (state == STATE.GAME && Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
                if (!scriptTMP.IsDoneTyping)
                    scriptTMP.SkipTyping();
                else
                    SetDialog();
            }

            // escŰ ������ UI �ڷΰ���
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (state)
                {
                    case STATE.GAME:
                        ShowMenu();
                        break;
                    case STATE.MENU:
                        CloseMenu();
                        break;
                    case STATE.SAVE:
                        CloseSave();
                        break;
                    case STATE.LOAD:
                        CloseLoad();
                        break;
                    case STATE.GALLERY:
                        CloseGallery();
                        break;
                    case STATE.LOG:
                        CloseLog();
                        break;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// ���� ����
        /// </summary>
        private void SaveGame()
        {

        }
        /// <summary>
        /// ���� �ҷ�����
        /// </summary>
        private void LoadGame()
        {

        }
        /// <summary>
        /// ���̾�α� ����
        /// (��ũ��Ʈ �ε����� �ϳ��� �ø��µ� �б⿡ ���� ��ȭ�� �ʿ��ϴٸ�, �̺κ��� �̺�Ʈ�Լ����� ó���ؾ��ҵ�. ���� �Ľ��ؼ� ����Ѵٸ� �Ľ��Ҷ� ���� �ε�����ȣ�� Ȯ���ؾ��� �ʿ䰡 �����������ϴ�)
        /// </summary>
        private void SetDialog()
        {
            log += "<b>" + ScriptData.scripts[scriptIndex].Name + "</b>\n<size=40>" + ScriptData.scripts[scriptIndex].Script + "</size>\n";
            nameTMP.SetText(ScriptData.scripts[scriptIndex].Name);
            scriptTMP.SetText(ScriptData.scripts[scriptIndex].Script);
            ScriptData.scripts[scriptIndex].OnScriptStart?.Invoke();
            ++scriptIndex;
        }
        /// <summary>
        /// UI���� ���ܰ�� ����
        /// (UI ������ �ִ� 2���ϰŰ��Ƽ� �׷� ��� �� ���·� �ǵ��ư��Բ� �ۼ��ߴµ�, ���Ŀ� �� ��� �����ϰ� ���� �����ؾ��ϴ°�� �����ڷᱸ�� ����ؼ� �����ؾ��Ұ� �����ϴ�)
        /// </summary>
        private void PopUIState()
        {
            switch (state)
            {
                case STATE.MENU:
                    CloseMenu();
                    break;
                case STATE.SAVE:
                    CloseSave();
                    break;
                case STATE.LOAD:
                    CloseLoad();
                    break;
                case STATE.GALLERY:
                    CloseGallery();
                    break;
                case STATE.LOG:
                    CloseLog();
                    break;
            }
        }
        #region State Methods
        /// <summary>
        /// �޴� ��ư Ŭ���� ȣ��
        /// </summary>
        public void OnClickMenuBtn()
        {
            switch (state)
            {
                case STATE.GAME:
                    ShowMenu();
                    return;
                case STATE.SAVE:
                    CloseSave();
                    break;
                case STATE.LOAD:
                    CloseLoad();
                    break;
                case STATE.GALLERY:
                    CloseGallery();
                    break;
                case STATE.LOG:
                    CloseLog();
                    break;
            }

            CloseMenu();
        }
        // �Ʒ� ���� ��ư Ŭ���� �ش� ��ư�� �ش��ϴ� ��ȣ�ۿ��Լ���
        public void ShowMenu()
        {
            state = STATE.MENU;
            menuPanel.SetSlide(Vector3.zero);
        }
        public void CloseMenu()
        {
            state = STATE.GAME;
            menuPanel.SetSlide(new Vector2(-500.0f, 0.0f));
        }
        public void ShowSave()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.SAVE;
            savePanel.SetSlide(new Vector2(0.0f, -340.0f));
        }
        public void CloseSave()
        {
            state = STATE.MENU;
            savePanel.SetSlide(new Vector2(-500.0f, -340.0f));
        }
        public void ShowLoad()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.LOAD;
            savePanel.SetSlide(new Vector2(0.0f, -340.0f));
        }
        public void CloseLoad()
        {
            state = STATE.MENU;
            savePanel.SetSlide(new Vector2(-500.0f, -340.0f));
        }
        public void ShowGallery()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.GALLERY;
        }
        public void CloseGallery()
        {
            state = STATE.MENU;
        }
        public void ShowLog()
        {
            if (state != STATE.MENU)
                PopUIState();
            state = STATE.LOG;
            logTMP.SetText(log);
            logUI.SetActive(true);
        }
        public void CloseLog()
        {
            state = STATE.MENU;
            logUI.SetActive(false);
        }
        public void ExitGame()
        {
            // Ÿ��Ʋ ������ ���ư��� ����
            // �ʿ��ϴٸ� �����Ұ��� ����°͵� �����ؾ��ҵ�
        }
        #endregion
        #endregion
    }
}
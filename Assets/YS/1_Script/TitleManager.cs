using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class TitleManager : MonoBehaviour
    {
        public GameObject newGameUI;
        public GameObject loadGameUI;

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Methods
        public void ShowNewGamePanel()
        {
            newGameUI.SetActive(true);
        }
        public void CloseNewGamePanel()
        {
            newGameUI.SetActive(false);
        }
        public void ShowLoadGamePanel()
        {
            loadGameUI.SetActive(true);
        }
        public void CloseLoadGamePanel()
        {
            loadGameUI.SetActive(false);
        }

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

            // �� ��ȯ
        }
        #endregion
    }
}
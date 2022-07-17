using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public abstract class BaseScriptEvent
    {
        [SerializeField, Tooltip("��濡 ���� �������� �����ݴϴ�.\n������ ���� ������ ���� �̺�Ʈ�� ����� ����մϴ�.")]
        private GameObject bgPrefab;
        [SerializeField, Tooltip("������� ����\n������ ���� ������ ���� �̺�Ʈ�� ��������� ����մϴ�.")]
        private AudioClip audioBGM;
        [SerializeField, Tooltip("ȿ���� ����")]
        private AudioClip audioFX;
        protected GameManager gm;


        public virtual void OnEnter()
        {
            gm = GameManager.Instance;
            gm.OnUpdateEvent += OnUpdate;

            if (bgPrefab != null)
                gm.ChangeBackground(Object.Instantiate(bgPrefab));

            if (audioBGM != null)
                AudioManager.PlayBGM(audioBGM);

            if (audioFX != null)
                AudioManager.PlayFX(audioFX);
            else
                AudioManager.StopFX();
        }
        public virtual void OnExit()
        {
            gm.OnUpdateEvent -= OnUpdate;
        }
        protected abstract void OnUpdate();
    }
}

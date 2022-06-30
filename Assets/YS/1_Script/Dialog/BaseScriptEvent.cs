using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public abstract class BaseScriptEvent
    {
        [SerializeField, Tooltip("배경에 대한 프리팹을 정해줍니다.\n지정해 주지 않으면 지난 이벤트의 배경을 사용합니다.")]
        private GameObject bgPrefab;

        protected GameManager gm;


        public virtual void OnEnter()
        {
            gm = GameManager.Instance;
            gm.OnUpdateEvent += OnUpdate;

            if (bgPrefab != null)
                gm.ChangeBackground(Object.Instantiate(bgPrefab));
        }
        public virtual void OnExit()
        {
            gm.OnUpdateEvent -= OnUpdate;
        }
        protected abstract void OnUpdate();
    }
}

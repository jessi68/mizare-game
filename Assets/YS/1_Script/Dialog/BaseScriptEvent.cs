using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public abstract class BaseScriptEvent
    {
        [SerializeField]
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

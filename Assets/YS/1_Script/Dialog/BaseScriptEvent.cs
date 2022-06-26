using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public enum SCREEN_EFFECT
    {
        NONE,
        FADE_IN,
        FADE_OUT,
        RED_FLASH
    }
    public enum CHARACTER_IMAGE_INDEX
    {
        NONE,
        MIZAR,
        ALCOR,
        MAX
    }
    public enum CHARACTER_EFFECT_INDEX
    {
        NONE,
        SHAKE_VERTICAL,
        SHAKE_HORIZONTAL,
        SHAKE_RANDOM,
        BOUNCE
    }

    [System.Serializable]
    public abstract class BaseScriptEvent
    {
        protected GameManager gm;

        public virtual void OnEnter()
        {
            gm = GameManager.Instance;
            gm.OnUpdateEvent += OnUpdate;
        }
        public virtual void OnExit()
        {
            gm.OnUpdateEvent -= OnUpdate;
        }
        protected virtual void OnUpdate() { }
    }
}

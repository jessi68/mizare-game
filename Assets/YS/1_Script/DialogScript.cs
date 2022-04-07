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
    public struct DialogScript
    {
        [System.Serializable]
        public struct ChoiceStruct
        {
            public string str;
            public uint nextIdx;
        }

        public SCREEN_EFFECT screenEffect;

        [Space(10.0f)]

        public CHARACTER_IMAGE_INDEX leftImage;
        public bool leftHighlight;
        public CHARACTER_EFFECT_INDEX leftEffect;
        public CHARACTER_IMAGE_INDEX rightImage;
        public bool rightHighlight;
        public CHARACTER_EFFECT_INDEX rightEffect;

        [Space(10.0f)]

        public ChoiceStruct[] choices;

        [Space(10.0f)]

        public string name;
        public string script;
        public uint nextIdx;
    }
}

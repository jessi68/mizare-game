using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeField]
        private BaseScriptEvent[] scripts;
        private BaseScriptEvent curScript;

        public BaseScriptEvent CurrentScript => curScript;

        public void SetScript(uint index)
        {
            curScript?.OnExit();
            curScript = scripts[index];
            curScript.OnEnter();
        }
    }
}
using UnityEngine;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeReference]
        [SerializeReferenceButton]
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
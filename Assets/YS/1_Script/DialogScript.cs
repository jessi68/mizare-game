using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public delegate void DialogEvent();
    public struct DialogScript
    {
        private string name;
        private string script;
        // 해당 스크립트에 작동하는 델리게이트 변수
        private DialogEvent onScriptStart;

        public string Name => name;
        public string Script => script;
        public DialogEvent OnScriptStart => onScriptStart;

        public DialogScript(string name, string script, DialogEvent startEvent)
        {
            this.name = name;
            this.script = script;
            onScriptStart = startEvent;
        }
    }
}

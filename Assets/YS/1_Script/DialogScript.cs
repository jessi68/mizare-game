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
        // �ش� ��ũ��Ʈ�� �۵��ϴ� ��������Ʈ ����
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

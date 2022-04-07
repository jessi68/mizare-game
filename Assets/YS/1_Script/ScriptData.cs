using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeField]
        private DialogScript[] scripts;

        public DialogScript this[uint index]
        {
            get
            {
                return scripts[index];
            }
        }
    }
}
using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Item
{
    [Serializable]
    public class MinMaxAmoutSeeds
    {
        [SerializeField] 
        private int _min;
        [SerializeField] 
        private int _max;

        public int Min => _min;
        public int Max => _max;
    }
}

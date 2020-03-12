using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Map
{
    [Serializable]
    public class MapAttributes
    {
        public MapAttributes(EnumSpawnType spawn)
        {
            _spawnType = spawn;
        }

        [SerializeField]
        [MinToAttribute(0, 50, "Tier Range")]
        private Vector2Int _tierMap;
        [SerializeField]
        private EnumSpawnType _spawnType;

        [Space]
        [SerializeField]
        private float _time;

        public Vector2Int TierRange => _tierMap;
        public EnumSpawnType SpawnType => _spawnType;
        public float Time => _time;
    }
}

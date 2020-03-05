using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Mob
{
    [CreateAssetMenu(fileName = "Mob", menuName = "ScriptableObjects/Mob", order = 1)]
    public class MobScriptable : ScriptableBase
    {
        [Header("Required Fields")]
        [SerializeField]
        private string _name;
        [SerializeField]
        private RuntimeAnimatorController _mobAnimator;
        [Space]
        [SerializeField]
        private GameObject _bossPrefab;

        [Header("Damage Taker options")]
        [SerializeField]
        private float _health;

        [Header("Damage Dealer options")]
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _cdwDamage;

        public string Name => _name;
        public RuntimeAnimatorController MobAnimator => _mobAnimator;
        public GameObject BossPrefab => _bossPrefab;
        public float Health => _health;
        public float Damage => _damage;
        public float CdwDamage => _cdwDamage;

        protected override void ValidateValues() { }
    }
}

using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Boss
{
    [CreateAssetMenu(fileName = "Boss", menuName = "ScriptableObjects/Boss", order = 1)]
    public class BossScriptable : ScriptableBase
    {
        [Header("Required Fields")]
        [SerializeField]
        private string _name;
        [SerializeField]
        private RuntimeAnimatorController _bossAniamtor;
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
        public RuntimeAnimatorController BossAnimator => _bossAniamtor;
        public GameObject BossPrefab => _bossPrefab;    
        public float Health => _health;
        public float Damage => _damage;
        public float CdwDamage => _cdwDamage;

        protected override void ValidateValues()
        {
            if (_name == null) Debug.LogError("The value of Name cannot be null!");
            if (_bossAniamtor == null) Debug.LogError("The value of BossAnimator cannot be null!");
            if (_bossPrefab == null) Debug.LogError("The value of BossPrefab cannot be null!");
        }

    }
}

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
        [SerializeField]
        [Range(0, 1)]
        private float _tierMultiplier;
        [Space]
        [SerializeField]
        private GameObject _mobPrefab;

        [Header("Damage Taker options")]
        [SerializeField]
        private float _health;

        [Header("Damage Dealer options")]
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _cdwDamage;

        private float _modifiedHealth;
        private float _modifiedDamage;

        public string Name => _name;
        public float TierMultiplier => _tierMultiplier;
        public RuntimeAnimatorController MobAnimator => _mobAnimator;
        public GameObject MobPrefab => _mobPrefab;
        public float Health
        {
            get
            {
                if (_modifiedHealth == 0) return _health;
                else return _modifiedHealth;
            }
        }
        public float Damage
        {
            get
            {
                if (_modifiedDamage == 0) return _damage;
                else return _modifiedDamage;
            }
        }
        public float CdwDamage => _cdwDamage;

        protected override void ValidateValues() { }

        public void ApplyMultiplier(int mapTier)
        {
            float multiplier = (mapTier * _tierMultiplier) + 1;

            _modifiedHealth = multiplier * _health;
            _modifiedDamage = multiplier * _damage;
        }
    }
}

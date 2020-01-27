using Assets.Scripts.ScriptableComponents.Poop;
using UnityEngine;

namespace Assets.Scripts.Components.CombatAttributes
{
    public class CombatAttributesComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public float CdwDamage => _cdwDamage;
        public float Damage => _damage;
        public float FullHealth { get; private set; }
        public float CurrentHealth { get => _health; set { _health = value; } }
        #endregion

        #region SERIALIZABLE ATTRIBUTES
        [Header("Confiuration Fields")]
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _cdwDamage;
        #endregion

        #region PUBLIC METHODS
        public void TurnItIntoAPooop(PoopScriptable poopScriptable)
        {
            _health = poopScriptable.Health;
            FullHealth = poopScriptable.Health;
            _damage = poopScriptable.Damage;
            _cdwDamage = poopScriptable.CdwDamage;
        }
        #endregion

        #region UNTIY METHODS
        private void Start()
        {
            FullHealth = _health;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_damage == 0) _damage = 1;
            if (_cdwDamage == 0)_cdwDamage = 0.5f;
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

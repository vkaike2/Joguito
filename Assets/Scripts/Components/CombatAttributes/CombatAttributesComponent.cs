using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.CombatAttributes
{
    public class CombatAttributesComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public float CdwDamage => _cdwDamage;
        public float Damage => _damage;
        public float Heath { get => _health; set { _health = value; } }
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

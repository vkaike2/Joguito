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
        #region SERIALIZABLE ATTRIBUTES
        [Header("Confiuration Fields")]
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _damage;
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_damage == 0) _damage = 1;
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

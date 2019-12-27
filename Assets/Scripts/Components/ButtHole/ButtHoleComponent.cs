using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.ButtHole
{
    public class ButtHoleComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Required Components")]
        [SerializeField]
        private Transform _spawnSpot;
        [SerializeField]
        private GameObject _mockPoop;
        #endregion

        #region PUBLIC METHODS
        public void Anim_SpawnPoop()
        {
            Instantiate(_mockPoop, _spawnSpot.position, Quaternion.identity);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
            if (_spawnSpot is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_spawnSpot), this.gameObject.name));
        }
        #endregion
    }
}

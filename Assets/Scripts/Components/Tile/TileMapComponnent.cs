using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Map;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    public class TileMapComponnent : BaseComponent
    {
        #region PUBLIC ATTRIBUTES
        public EnumSide Side => _side;
        public bool HasObstacle => _objectsList != null && _objectsList.Any();
        #endregion

        #region SERIALIZE ATTRIBUTES
        [Header("Required Methods")]
        [SerializeField]
        private EnumSide _side;
        #endregion

        #region PRIVATE ATTRIBUTES
        private MapAnimatorVariables _animatorVariables;
        private Animator _animator;
        private DamageTakerComponent _damageTaker;
        private List<GameObject> _objectsList;
        #endregion

        #region PUBLIC METHODS
        public void SpawnObject(GameObject pefab)
        {
            if (_objectsList is null) _objectsList = new List<GameObject>();

            GameObject gameObject = GameObject.Instantiate(pefab, this.transform.position, Quaternion.identity);
            _objectsList.Add(gameObject);
        }

        public void SetInitialAnimator(RuntimeAnimatorController animatorController, int layerId)
        {
            _animator.runtimeAnimatorController = animatorController;
            _animator.SetLayerWeight(layerId, 1);
            this.UpdateTileHealth(1);
        }
        public void UpdateTileHealth(float healtPercentage)
        {
            if (_animator == null || _damageTaker == null) return;

            _animator.SetFloat(_animatorVariables.LifePercentage, healtPercentage);

            // => if map die
            if (_damageTaker.HealthPercenage < 0.1)
            {
                RemoveEveryObject();
            }
        }
        #endregion

        #region PRIVATE METHODS


        private void RemoveEveryObject()
        {
            if (_objectsList is null) return;

            foreach (GameObject obj in _objectsList)
            {
                obj.SetActive(false);
            }
            _objectsList = null;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _animator = this.GetComponent<Animator>();
            _damageTaker = this.GetComponentInParent<DamageTakerComponent>();
            _animatorVariables = new MapAnimatorVariables();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }

    public enum EnumSide
    {
        LEFT_UP,
        UP,
        RIGHT_UP,
        RIGHT,
        LEFT,
        LEFT_BOTTOM,
        BOTTOM,
        BOTTOM_RIGHT,
        CENTER
    }
}

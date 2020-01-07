using System;
using Assets.Scripts.Components.GenericPoop;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Utils;
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
        private GameObject _genericPoopComponent;
        #endregion

        #region PRIVATE ATRIBUTES
        private PoopScriptable _currentPoop;
        private Animator _animator;
        private ButtHoleAnimatorVariables _animatorVariables;
        #endregion

        #region PUBLIC METHODS
        public void Anim_SpawnPoop()
        {
            GameObject gameObject = Instantiate(_genericPoopComponent, _spawnSpot.position, Quaternion.identity);
            gameObject.GetComponent<GenericPoopComponent>().SetCurrentPoopScriptable(_currentPoop);

            _currentPoop = null;
        }

        public void SetCurrentPoop(PoopScriptable currentPoop)
        {
            _currentPoop = currentPoop;
            _animator.SetTrigger(_animatorVariables.StartPoop);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _animator = this.GetComponent<Animator>();
            _animatorVariables = new ButtHoleAnimatorVariables();
        }

        protected override void ValidateValues()
        {
            if (_spawnSpot is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_spawnSpot), this.gameObject.name));
        }
        #endregion
    }
}

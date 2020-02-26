using System;
using System.Collections;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.ScriptableComponents.Poop;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components.DamageDealer
{
    public class DamageDealerComponent : BaseComponent
    {
        #region PUBLIC ATTRIBUTES
        public UnityEvent event_AlertAttack;

        public float CdwDamage => _cdwDamage;
        public float Damage => _damage;
        #endregion

        #region SERIALIZABLE ATTRIBUTES
        [Header("Alert combat options")]
        [SerializeField]
        private GameObject _alertObject;
        [SerializeField]
        private float _cdwToAlertOptions;

        [Header("Configuration Fields")]
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _cdwDamage;
        #endregion

        #region PRIVATE ATTRIBUTES
        private Animator _animator;
        private DamageDealerAnimatorVariables _animatorVariables;
        private DamageTakerComponent _enemyDamageTaker;
        private bool _canDealDamage = true;
        #endregion

        #region PUBLIC METHODS
        public void Animator_CantDealDamage()
        {
            _canDealDamage = false;
        }

        public void TurnItIntoAPoop(PoopScriptable poopScriptable)
        {
            _damage = poopScriptable.Damage;
            _cdwDamage = poopScriptable.CdwDamage;
        }

        public void StartAtackAnimation(DamageTakerComponent damageTakerComponent)
        {
            if (!_canDealDamage) return;
            _enemyDamageTaker = damageTakerComponent;
            _animator.SetTrigger(_animatorVariables.Attack);
        }

        public void Animator_EnemyTakeDamage()
        {
            if (_enemyDamageTaker is null) return;

            _enemyDamageTaker.TakeDamage(_damage);
        }
        #endregion

        #region PRIVATE METHODS
        private void Event_AlertAttack()
        {
            if (_alertObject is null) return;

            StartCoroutine(StartAtackAlert());
        }
        #endregion

        #region UNITY METHODS
        private void OnDestroy()
        {
            if (_enemyDamageTaker is null) return;
            _enemyDamageTaker.TryToRemoveFromEnemyList(this.GetInstanceID());
        }
        #endregion

        #region COROUTINES
        IEnumerator StartAtackAlert()
        {
            float _internalCdw = 0f;

            _alertObject.SetActive(true);

            while (_internalCdw <= _cdwToAlertOptions)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _alertObject.SetActive(false);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _animatorVariables = new DamageDealerAnimatorVariables();
            _animator = this.GetComponent<Animator>();
            event_AlertAttack.AddListener(Event_AlertAttack);
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

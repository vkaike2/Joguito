using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.LifeBar;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Structure.Player;
using UnityEngine;

namespace Assets.Scripts.Components.DamageTaker
{
    public class DamageTakerComponent : BaseComponent, IDamageTaker
    {
        #region PUBLIC ATRIBUTES
        public bool IsGround => IsGround;
        #endregion

        #region SERIALIZABLE ATTRIBUTES
        [Header("Damage Animators")]
        [SerializeField]
        private List<Animator> _damageAnimatorList;

        [Header("Configuration Fields")]
        [SerializeField]
        private float _health;
        [SerializeField]
        [Tooltip("Used to identify a boss")]
        private bool _isBoss;
        [SerializeField]
        [Tooltip("Used to identify a ground")]
        private bool _isGround;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerState;
        private MovementMouseCollider _colliderToStopMovement;
        private float _fullHealth;
        private bool _readyToCombat = true;
        private List<int> _instanceIdEnemyList;
        private Animator _animator;
        private DamageTakerAnimatorVariables _animatorVariables;
        private LifeBarComponent _lifeBarComponent;
        #endregion

        #region INTERFACE METHODS
        public int Order()
        {
            return 3;
        }

        public bool StartCombat(DamageDealerComponent damageDealer)
        {
            if (!_readyToCombat) return false;

            PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();

            InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();

            if (interactableComponent is null) return false;
            if (!interactableComponent.CheckIfCanAtack()) return false;

            damageDealer.event_AlertAttack.Invoke();
            interactableComponent.SetInteractableState(EnumInteractableState.Atack, this.GetInstanceID());

            // Go to the correct position to attack
            playerStructure.GetMovementMouseComponent().ObjectGoTo(this.transform.position, _colliderToStopMovement.GetInstanceID());

            return true;
        }
        #endregion

        #region PUBLIC METHODS
        public void TakeDamage(float damage)
        {
            if (!_readyToCombat) return;

            _health -= damage;

            _lifeBarComponent.event_UpdateLifeBar.Invoke(_health / _fullHealth);

            if (_health <= 0) // => Die
            {
                _readyToCombat = false;
                _animator.SetTrigger(_animatorVariables.Die);
            }
            else
            {
                this.StartAnimationDamage();
                _animator.SetTrigger(_animatorVariables.TakeDamage);
            }
        }

        public void StartDefenseOperation(DamageDealerComponent damageDealer)
        {
            if (damageDealer is null) return;
            if (_instanceIdEnemyList.Any(e => e == damageDealer.GetInstanceID())) return;
            _instanceIdEnemyList.Add(damageDealer.GetInstanceID());

            StartCoroutine(DefenseOperation(damageDealer));
        }

        public void Animator_IsReadyToCombat()
        {
            _readyToCombat = true;
        }

        public void Animator_IsntReadyToCombat()
        {
            _readyToCombat = false;
        }

        public void Animator_Destroy_Object()
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void TurnItIntoAPoop(PoopScriptable poopScriptable)
        {
            _health = poopScriptable.Health;
            _fullHealth = _health;
        }

        public void TurnItIntoABoss(BossScriptable bossScriptable)
        {
            _health = bossScriptable.Health;
            _fullHealth = _health;
        }

        public void TryToRemoveFromEnemyList(int enemyInstanceId)
        {
            if (!_instanceIdEnemyList.Any(e => e == enemyInstanceId)) return;

            _instanceIdEnemyList.Remove(enemyInstanceId);
        }
        #endregion

        #region COROUTINES
        IEnumerator DefenseOperation(DamageDealerComponent damageDealer)
        {
            float _internalCdw = 0f;

            if (damageDealer != null)
            {
                InteractableComponent enemyInteractableComponent = damageDealer.GetComponent<InteractableComponent>();

                while (_internalCdw <= damageDealer.CdwDamage)
                {
                    _internalCdw += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }

                if (enemyInteractableComponent.IsAttackingThisMonster(this.GetInstanceID()))
                {
                    damageDealer.StartAtackAnimation(this);
                    StartCoroutine(DefenseOperation(damageDealer));
                }
                else
                {
                    _instanceIdEnemyList.Remove(damageDealer.GetInstanceID());
                }
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void StartAnimationDamage()
        {
            if (!_damageAnimatorList.Any()) return;

            int count = _damageAnimatorList.Count();
            _damageAnimatorList[UnityEngine.Random.Range(0, count - 1)].SetTrigger(_animatorVariables.BasicDamage);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _colliderToStopMovement = this.GetComponentInChildren<MovementMouseCollider>();
            _animator = this.GetComponent<Animator>();
            _animatorVariables = new DamageTakerAnimatorVariables();
            _lifeBarComponent = GameObject.FindObjectsOfType<LifeBarComponent>().FirstOrDefault(e => e.IsBoss == _isBoss);

            _fullHealth = _health;
            _instanceIdEnemyList = new List<int>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

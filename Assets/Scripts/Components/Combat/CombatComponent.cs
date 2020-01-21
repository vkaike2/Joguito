using Assets.Scripts.Components.CombatAttributes;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Combat
{
    [RequireComponent(typeof(CombatAttributesComponent))]
    public class CombatComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Confiuration Fields")]
        [SerializeField]
        private float _radioToInteract;
        [SerializeField]
        private Animator _damageAnimator;

        [Header("Collider Stop Movement")]
        [SerializeField]
        private MovementMouseCollider _collider;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerState;
        private InputManager _inputManager;
        private CombatAttributesComponent _combatAtributtes;
        private bool _mousePressed = false;
        private List<int> _instanceIdenemyList;
        private Animator _animator;
        private CombatAnimatorVariables _animatorVariables;
        private bool _canReceiveNewDamage = true;
        private int? _playerStructureInstaceId => this.GetComponent<PlayerStructure>()?.GetInstanceID();
        private CombatComponent _enemyCombatComponent;
        #endregion

        #region UNITY METHODS
        private void OnMouseOver()
        {
            if (_inputManager.MouseLeftButton == 1 && !_mousePressed)
            {
                Debug.Log(this.gameObject.name);
                _mousePressed = true;

                PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();
                if (_playerStructureInstaceId != null && playerStructure.GetInstanceID() == _playerStructureInstaceId.GetValueOrDefault())
                    return;

                InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();

                if (interactableComponent is null) return;
                if (!interactableComponent.CheckIfCanAtack()) return;

                interactableComponent.SetInteractableState(EnumInteractableState.Atack, this.GetInstanceID());
                playerStructure.GetMovementMouseComponent().ObjectGoTo(this.transform.position, _collider.GetInstanceID());

            }
            else if (_inputManager.MouseLeftButton == 0 && _mousePressed)
                _mousePressed = false;
        }
        #endregion

        #region PUBLIC METHODS
        public void Animator_Destroy_Object()
        {
            Destroy(this.transform.parent.gameObject);
        }

        public void StopCombatActions()
        {
            _enemyCombatComponent = null;
        }

        #region USED TO ATTACK
        public void StartAtackAnimation(CombatComponent enemyCombatComponent)
        {
            _enemyCombatComponent = enemyCombatComponent;
            _animator.SetTrigger(_animatorVariables.Atack);
        }

        public void Animator_EnemyTakeDamage()
        {
            _enemyCombatComponent.TakeDamage(_combatAtributtes.Damage);
        }
        #endregion

        #region USED TO DEFEND
        public void StartDefenseOperation(CombatAttributesComponent combatAttributes)
        {
            if (_instanceIdenemyList.Any(e => e == combatAttributes.GetInstanceID())) return;

            _instanceIdenemyList.Add(combatAttributes.GetInstanceID());
            this.StartCoroutine(DefenseOperation(combatAttributes));
        }

        public void TakeDamage(float damage)
        {
            if (!_canReceiveNewDamage) return;

            _combatAtributtes.Heath -= damage;

            if (_combatAtributtes.Heath <= 0) // => DIe
            {
                _canReceiveNewDamage = false;
                _animator.SetTrigger(_animatorVariables.Die);
            }
            else
            {
                _damageAnimator.SetTrigger(_animatorVariables.BasicDamage);
                _animator.SetTrigger(_animatorVariables.TakeDamage);
            }
        }
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator DefenseOperation(CombatAttributesComponent combatAttributes)
        {
            float _internalCdw = 0f;
            InteractableComponent enemyInteractableComponent = combatAttributes.GetComponent<InteractableComponent>();
            CombatComponent enemyCombatComponent = combatAttributes.GetComponent<CombatComponent>();

            while (_internalCdw <= combatAttributes.CdwDamage)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            if (enemyInteractableComponent.IsAttackingThisMonster(this.GetInstanceID()))
            {
                enemyCombatComponent.StartAtackAnimation(this);
                StartCoroutine(DefenseOperation(combatAttributes));
            }
            else
            {
                this.StopCombatActions();
                enemyCombatComponent.StopCombatActions();
                _instanceIdenemyList.Remove(combatAttributes.GetInstanceID());
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            //PlayerStructure playerStructure = this.GetComponent<PlayerStructure>();
            //if(PlayerStructure != null)
            //_playerStructureInstaceId

            if (_radioToInteract == 0) _radioToInteract = 0.2f;

            _animatorVariables = new CombatAnimatorVariables();
            _animator = this.GetComponent<Animator>();
            _instanceIdenemyList = new List<int>();
            _combatAtributtes = this.GetComponent<CombatAttributesComponent>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

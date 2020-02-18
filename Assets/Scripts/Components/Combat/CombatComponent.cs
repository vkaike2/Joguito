using Assets.Scripts.Components.CombatAttributes;
using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.LifeBar;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Combat
{
    [RequireComponent(typeof(CombatAttributesComponent))]
    public class CombatComponent : BaseComponent, IDamageTaker
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Confiuration Fields")]
        [SerializeField]
        private float _radioToInteract;
        [SerializeField]
        private List<Animator> _damageAnimatorList;
        [SerializeField]
        private GameObject _alertObject;
        [SerializeField]
        private LifeBarComponent _lifeBarComponent;

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
        private bool _readyToCombat = true;
        #endregion

        #region INTERFACE METHODS

        public bool StartCombat(DamageDealerComponent damageDealerComponent)
        {
            PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();
            if (_playerStructureInstaceId != null && playerStructure.GetInstanceID() == _playerStructureInstaceId.GetValueOrDefault())
                return false;

            InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();

            if (interactableComponent is null) return false;
            if (!interactableComponent.CheckIfCanAtack()) return false;

            playerStructure.GetComponent<CombatComponent>().StartAtackAlertCoroutine();
            interactableComponent.SetInteractableState(EnumInteractableState.Atack, this.GetInstanceID());
            playerStructure.GetMovementMouseComponent().ObjectGoTo(this.transform.position, _collider.GetInstanceID());

            return true;
        }

        public int Order()
        {
            return 3;
        }
        #endregion

        #region PUBLIC METHODS
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

        public void StartAtackAlertCoroutine()
        {
            StartCoroutine(StartAtackAlert());
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

            _combatAtributtes.CurrentHealth -= damage;
            //_lifeBarComponent.PercentageHp(_combatAtributtes.CurrentHealth / _combatAtributtes.FullHealth);
            if (_combatAtributtes.CurrentHealth <= 0) // => DIe
            {
                _canReceiveNewDamage = false;
                _animator.SetTrigger(_animatorVariables.Die);
            }
            else
            {
                this.StartAnimationDamage();
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

        IEnumerator StartAtackAlert()
        {
            float _internalCdw = 0f;

            _alertObject.SetActive(true);

            while (_internalCdw <= 0.3f)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _alertObject.SetActive(false);
        }
        #endregion

        #region PRIVATE METHORDS
        private void StartAnimationDamage()
        {
            int count = _damageAnimatorList.Count();
            _damageAnimatorList[Random.Range(0, count - 1)].SetTrigger(_animatorVariables.BasicDamage);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {

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

using Assets.Scripts.Components;
using Assets.Scripts.Components.ActivePlayers;
using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.MiniMap;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Utils;
using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Structure.Player
{
    public class PlayerStructure : StructureBase, IPlayer
    {
        #region PUBLIC ATRIBUTES
        public bool IsMainPlayer => _isMainPlayer;
        public bool IsActive { get; private set; }
        public Sprite SpriteForActiveStatus => _spriteForActiveStatus;
        public bool CanInteract => _canInteract;
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private GameObject _hightlight;

        [Header("Configuration Fields")]
        [SerializeField]
        private bool _isMainPlayer;
        [SerializeField]
        private bool _canMoveByClick;
        [SerializeField]
        private bool _canInteract;
        [SerializeField]
        private bool _canPoop;
        [SerializeField]
        private Sprite _spriteForActiveStatus;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerStateManage;
        private CinemachineVirtualCamera _cinemachine;
        private UIManager _uIManager;
        private ActivePlayersUIComponent _activePlayerUI;
        private int? slotInstanceId = null;
        private MovementMouseComponent _movementMouseComponent;
        private InteractableComponent _interactableComponent;
        private StomachComponent _stomachComponent;
        private PlayerAnimatorVariables _animatorVariables;
        private DamageDealerComponent _damageDealerComponent;
        private DamageTakerComponent _damageTakerComponent;
        private Animator _animator;
        private AudioComponent _audioComponent;
        private MiniMapComponent _miniMapComponent;
        #endregion

        #region PUBLIC METHODS
        public void ActivatePlayerStructure(bool value)
        {
            if (value)
            {
                _audioComponent.Audio_ChosePlayer();
                _cinemachine.Follow = this.transform;
            }

            if (_hightlight != null)
                _hightlight.SetActive(value);

            _activePlayerUI.ActivatePlayerSlot(slotInstanceId.Value);

            _uIManager.ActivateInventory(_isMainPlayer);
            _uIManager.ActivateActionSlots(_isMainPlayer);
            _uIManager.ActivateStomach(_canPoop);

            IsActive = value;
            if (_canMoveByClick) _movementMouseComponent.SetActivationComponent(value);
            if (_canInteract) _interactableComponent.SetActivationComponent(value);
            if (_canPoop) _stomachComponent.SetActivationComponent(value);

        }

        public void TurnItIntoAPoop(PoopScriptable poopScriptable)
        {
            // => Player Structure
            _animator.runtimeAnimatorController = poopScriptable.PoopAnimator;
            _isMainPlayer = false;
            _canMoveByClick = poopScriptable.CanMoveByClick;
            _canInteract = poopScriptable.CanInteract;
            _canPoop = poopScriptable.CanPoop;
            _spriteForActiveStatus = poopScriptable.SpriteForActiveStatus;

            // => Interact Component
            this._interactableComponent.TurnItIntoAPoop(poopScriptable);

            // => DamageTakerOptions
            _damageTakerComponent.TurnItIntoAPoop(poopScriptable);

            // => DamageDealerOptions
            _damageDealerComponent.TurnItIntoAPoop(poopScriptable);

            this.StartCoroutine(StartDeathCooldown(poopScriptable.DeathCooldown));
        }

        public StomachComponent GetStomachComponent()
        {
            if (!_canInteract) return null;

            return _stomachComponent;
        }

        public MovementMouseComponent GetMovementMouseComponent()
        {
            if (!_canMoveByClick) return null;
            return _movementMouseComponent;
        }

        public InteractableComponent GetInteractableComponent()
        {
            if (!_canInteract) return null;

            return _interactableComponent;
        }
        public DamageDealerComponent GetDamageDealerComponent()
        {
            InteractableComponent interactableComponent = this.GetInteractableComponent();

            if (interactableComponent is null) return null;

            if (!interactableComponent.CheckIfCanAtack()) return null;

            return _damageDealerComponent;
        }

        public void Animator_PoopKamikaze()
        {
            Destroy(this.gameObject);
        }
        #endregion

        #region INTERFACE METHODS
        public bool SwitchPlayer()
        {
            _playerStateManage.ActiveNewPlayerStructure(this.GetInstanceID());
            this.StartCoroutine(FreezeForSomeCooldown(0.2f));
            return true;
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            slotInstanceId = _activePlayerUI.CreateNewPlayerSlotCompoennt(this);
            _playerStateManage.SetNewPlayerStrucutre(this);
        }

        private void OnEnable()
        {
            if (_miniMapComponent != null)
                _miniMapComponent.AddPlayerStrucute(this);
        }

        private void OnDestroy()
        {
            _activePlayerUI.DesactivePlayerSlot(slotInstanceId.Value);
            _playerStateManage.RemoveOnePlayerStructure(this);

            if (_miniMapComponent != null)
                _miniMapComponent.RemovePlayerStructure(this);
        }
        #endregion

        #region COROUTINES

        IEnumerator FreezeForSomeCooldown(float cooldown)
        {
            float internalCdw = 0f;

            _movementMouseComponent.Animator_CantMove();

            while (internalCdw <= cooldown)
            {
                internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _movementMouseComponent.Animator_CanMove();
        }

        IEnumerator StartDeathCooldown(float deathCooldown)
        {
            float internalCdw = 0f;

            while (internalCdw <= deathCooldown)
            {
                internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            _animator.SetTrigger(_animatorVariables.Die);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _animatorVariables = new PlayerAnimatorVariables();
            _cinemachine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            _uIManager = GameObject.FindObjectOfType<UIManager>();
            _playerStateManage = GameObject.FindObjectOfType<PlayerStateManager>();
            _activePlayerUI = GameObject.FindObjectOfType<ActivePlayersUIComponent>();
            _animator = this.GetComponent<Animator>();
            _audioComponent = this.GetComponent<AudioComponent>();

            _damageDealerComponent = this.GetComponent<DamageDealerComponent>();
            _damageTakerComponent = this.GetComponent<DamageTakerComponent>();

            _miniMapComponent = GameObject.FindObjectOfType<MiniMapComponent>();


            if (_canMoveByClick) _movementMouseComponent = this.GetComponent<MovementMouseComponent>();
            if (_canInteract) _interactableComponent = this.GetComponent<InteractableComponent>();
            if (_canPoop) _stomachComponent = this.GetComponent<StomachComponent>();
        }

        protected override void ValidateValues()
        {
            if (_canMoveByClick && _movementMouseComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_movementMouseComponent), this.gameObject.name));
            if (_canInteract && _interactableComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_interactableComponent), this.gameObject.name));
            if (_canPoop && _stomachComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_stomachComponent), this.gameObject.name));
        }
        #endregion
    }
}

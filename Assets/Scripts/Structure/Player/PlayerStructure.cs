using Assets.Scripts.Components.ActivePlayers;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.Utils;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Structure.Player
{
    public class PlayerStructure : StructureBase
    {
        #region PUBLIC ATRIBUTES
        public bool IsMainPlayer => _isMainPlayer;
        public bool IsActive { get; private set; }
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("Configuration Fields")]
        [SerializeField]
        private bool _isMainPlayer;
        [SerializeField]
        private bool _canMoveByClick;
        [SerializeField]
        private bool _canInteract;
        [SerializeField]
        private bool _canPoop;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerStateManage;
        private CinemachineVirtualCamera _cinemachine;
        private UIManager _uIManager;
        private ActivePlayersUIComponent _activePlyarUI;
        private int? slotInstanceId = null;
        private MovementMouseComponent _movementMouseComponent;
        private InteractableComponent _interactableComponent;
        private StomachComponent _stomachComponent;
        #endregion

        #region PUBLIC METHODS
        public void ActivatePlayerStructure(bool value)
        {
            if (value)
            {
                _cinemachine.Follow = this.transform;
            }

            _activePlyarUI.ActivatePlayerSlot(slotInstanceId.Value);

            _uIManager.ActivateInventory(_isMainPlayer);
            _uIManager.ActivateActionSlots(_isMainPlayer);
            _uIManager.ActivateStomach(_canPoop);

            IsActive = value;
            if (_canMoveByClick) _movementMouseComponent.SetActivationComponent(value);
            if (_canInteract) _interactableComponent.SetActivationComponent(value);
            if (_canPoop) _stomachComponent.SetActivationComponent(value);
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
            if (!_interactableComponent) return null;

            return _interactableComponent;
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            slotInstanceId = _activePlyarUI.CreateNewPlayerSlotCompoennt();
            _playerStateManage.SetNewPlayerStrucutre(this);
        }

        private void OnDestroy()
        {
            _playerStateManage.RemoveOnePlayerStructure(this);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _cinemachine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            _uIManager = GameObject.FindObjectOfType<UIManager>();
            _playerStateManage = GameObject.FindObjectOfType<PlayerStateManager>();
            _activePlyarUI = GameObject.FindObjectOfType<ActivePlayersUIComponent>();
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

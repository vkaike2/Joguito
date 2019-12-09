using Assets.Scripts.Components.ItemDrop;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.Interactable
{
    public class InteractableComponent : BaseComponent
    {
        private InputManager _inputManager;
        private PlayerStateManager _playerStateManager;
        private EnumInteractableState _currentInteractableState;
        private int? _interactableInstanceId;

        public bool Active => true;

        private void Start()
        {
            _playerStateManager.SetInteractableComponent(this);
        }

        private void OnDestroy()
        {
            _playerStateManager.RemoveInteractableComponent(this);
        }

        public void RemoveInteractableState()
        {
            _currentInteractableState = EnumInteractableState.Nothing;
            _interactableInstanceId = null;
        }

        public void SetInteractableState(EnumInteractableState interactableState, int instanceId)
        {
            _currentInteractableState = interactableState;
            _interactableInstanceId = instanceId;
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _currentInteractableState = EnumInteractableState.Nothing;
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_playerStateManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerStateManager), this.gameObject.name));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (_currentInteractableState)
            {
                case EnumInteractableState.Nothing:
                    break;
                case EnumInteractableState.PickupItem:

                    if (_interactableInstanceId == null) return;

                    ItemDropComponent itemDropComponent = collision.gameObject.GetComponentInParent<ItemDropComponent>();
                    if (itemDropComponent == null) return;
                    if (itemDropComponent.GetInstanceID() != _interactableInstanceId) return;


                    itemDropComponent.PickupThisItem();
                    this.RemoveInteractableState();
                    break;
                default:
                    break;
            }


        }

        private void OnTriggerStay2D(Collider2D collision)
        {

        }

    }
}

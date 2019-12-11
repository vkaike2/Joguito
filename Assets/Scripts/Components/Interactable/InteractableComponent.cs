using Assets.Scripts.Components.Inventory;
using Assets.Scripts.Components.ItemDrop;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.Interactable
{
    [HelpURL("https://slimwiki.com/vkaike9/interactablecomponent")]
    public class InteractableComponent : BaseComponent
    {
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        private InventoryComponent _inventoryComponent;

        private InputManager _inputManager;
        private PlayerStateManager _playerStateManager;
        private EnumInteractableState _currentInteractableState;
        private int? _interactableInstanceId;
#pragma warning restore 0649

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
            if (_inventoryComponent == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inventoryComponent), this.gameObject.name));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (_currentInteractableState)
            {
                case EnumInteractableState.Nothing:
                    break;
                case EnumInteractableState.PickupItem:
                    this.PickUpItem(collision);
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            switch (_currentInteractableState)
            {
                case EnumInteractableState.Nothing:
                    break;
                case EnumInteractableState.PickupItem:
                    this.PickUpItem(collision);
                    break;
                default:
                    break;
            }
        }

        private void PickUpItem(Collider2D collision)
        {
            if (_interactableInstanceId == null) return;

            ItemDropComponent itemDropComponent = collision.gameObject.GetComponentInParent<ItemDropComponent>();
            if (itemDropComponent == null) return;
            if (itemDropComponent.GetInstanceID() != _interactableInstanceId) return;

            ItemDTO itemDto = itemDropComponent.PickupThisItem();
            itemDropComponent.DestroyGameObject();
            _inventoryComponent.AddItem(itemDto);

            this.RemoveInteractableState();
        }

    }
}

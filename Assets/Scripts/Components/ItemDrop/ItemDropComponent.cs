using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.DTOs;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.ItemDrop
{
    /// <summary>
    ///     Represents an iten drop on the floor
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ItemDropComponent : BaseComponent, IPickable
    {
        #region PUBLIC ATRIBUTES
        public GameObject ParentGameObject => transform.parent.gameObject;
        #endregion

        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private TextMeshProUGUI _txtItemName;

        [Header("Configuration")]
        [SerializeField]
        [Range(0, 3)]
        private float _radioToPickup;
        [SerializeField]
        private int _interactOrder;
        #endregion

        #region PRIVATE FIELDS
        private InputManager _inputManager;
        private PlayerStateManager _playerState;
        private Animator _animator;
        private ItemDTO currentItem;
        private UIManager _uiManager;
        private int _instanceIDForGenereciUI;
        #endregion

        #region INTERFACE METHODS
        public bool PickUp()
        {
            if (_playerState.PlayerIsDoingSomeAction) return false;
            if (_uiManager.GenericUIList.Any(e => e.MouseInUI && e.GetInstanceID() != _instanceIDForGenereciUI)) return false;

            _playerState.GetActivePlayerStructure().GetMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToPickup);
            _playerState.GetActivePlayerStructure().GetInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.PickupItem, this.GetInstanceID());

            return true;
        }

        public int Order()
        {
            return _interactOrder;
        }
        #endregion

        #region PUBLIC METHODS
        public void SetCurrentItem(ItemDTO item)
        {
            currentItem = item;
            _txtItemName.SetText(currentItem.Item.Name);
            _animator.runtimeAnimatorController = currentItem.Item.DropAnimatorController;
        }

        public ItemDTO PickupThisItem()
        {
            return currentItem;
        }

        public void DestroyGameObject()
        {
            Destroy(ParentGameObject);
        }

        public void OnClickObject()
        {
            if (_playerState.PlayerIsDoingSomeAction) return;
            if (_uiManager.GenericUIList.Any(e => e.MouseInUI && e.GetInstanceID() != _instanceIDForGenereciUI)) return;

            _playerState.GetActivePlayerStructure().GetMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToPickup);
            _playerState.GetActivePlayerStructure().GetInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.PickupItem, this.GetInstanceID());
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _animator = this.GetComponent<Animator>();

            _uiManager = GameObject.FindObjectOfType<UIManager>();

            if (_radioToPickup == 0) _radioToPickup = 1.5f;
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_playerState == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerState), this.gameObject.name));
            if (_animator == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_animator), this.gameObject.name));
            if (_txtItemName == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_txtItemName), this.gameObject.name));
        }
        #endregion
    }
}

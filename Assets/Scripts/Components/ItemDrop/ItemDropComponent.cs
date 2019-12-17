using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.ItemDrop
{
    /// <summary>
    ///     Represents an iten drop on the floor
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ItemDropComponent : BaseComponent
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
        #endregion

        #region PRIVATE FIELDS
        private InputManager _inputManager;
        private PlayerStateManager _playerState;
        private Animator _animator;
        private ItemDTO currentItem;
        private bool _mousePressed = false;
        #endregion

        #region UNITY METHODS
        private void OnMouseOver()
        {
            ManageThePlayersClick();
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
            _playerState.GetActiveMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToPickup);
            _playerState.GetActiveInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.PickupItem, this.GetInstanceID());
        }
        #endregion

        #region PRIVATE METHODS
        private void ManageThePlayersClick()
        {
            if (_inputManager.MouseLeftButton == 1 && !_mousePressed)
            {
                _mousePressed = true;
                OnClickObject();
            }
            else if (_inputManager.MouseLeftButton == 0 && _mousePressed)
            {
                _mousePressed = false;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _animator = this.GetComponent<Animator>();

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

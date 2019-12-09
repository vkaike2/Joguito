using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.ItemDrop
{
    public class ItemDropComponent : BaseComponent
    {
        
        [Header("Configuration")]
        [SerializeField]
        [Range(0,3)]
        private float _radioToPickup;

        private InputManager _inputManager;
        private PlayerStateManager _playerState;
       
        private bool _mousePressed = false;

        private void OnMouseOver()
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

        public void PickupThisItem()
        {
            Debug.Log("Pegou o item");
            Destroy(this.transform.parent.gameObject);
        }

        public void OnClickObject()
        {
            _playerState.GetActiveMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToPickup);
            _playerState.GetActiveInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.PickupItem, this.GetInstanceID());
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();

            if (_radioToPickup == 0) _radioToPickup = 1.5f;
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_playerState == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerState), this.gameObject.name));
        }
    }
}

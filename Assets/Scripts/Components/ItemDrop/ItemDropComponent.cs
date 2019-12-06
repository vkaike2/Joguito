using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.ItemDrop
{
    public class ItemDropComponent : BaseComponent
    {
        private InputManager _inputManager;
        private MovementMouseComponent _movementMouse;

        private bool _mousePressed = false;
        private bool _playerWant = false;

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

        public void OnClickObject()
        {
            _movementMouse.ObjectGoTo(this.transform.position);
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _movementMouse = GameObject.FindObjectOfType<MovementMouseComponent>();
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_movementMouse == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_movementMouse), this.gameObject.name));
        }
    }
}

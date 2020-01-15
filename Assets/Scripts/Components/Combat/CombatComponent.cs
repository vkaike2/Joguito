using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Combat
{
    public class CombatComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public int ColliderInstanceId => _collider.GetInstanceID();
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("Confiuration Fields")]
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _radioToInteract;

        [Header("Collider Stop Movement")]
        [SerializeField]
        private Collider2D _collider;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerState;
        private InputManager _inputManager;
        private bool _mousePressed = false;
        #endregion

        #region UNITY METHODS
        private void OnMouseOver()
        {
            if (_inputManager.MouseLeftButton == 1 && !_mousePressed)
            {
                _mousePressed = true;

                PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();
                InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();

                if (!interactableComponent.CheckIfCanAtack())
                    return;

                interactableComponent.SetInteractableState(EnumInteractableState.Atack, this.GetInstanceID());
                playerStructure.GetMovementMouseComponent().ObjectGoTo(this.transform.position, _collider.GetInstanceID());

            }
            else if (_inputManager.MouseLeftButton == 0 && _mousePressed)
                _mousePressed = false;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_damage == 0) _damage = 1;
            if (_radioToInteract == 0) _radioToInteract = 0.2f;

            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

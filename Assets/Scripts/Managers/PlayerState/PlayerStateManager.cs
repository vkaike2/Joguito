using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Interactable;

namespace Assets.Scripts.Managers.PlayerState
{
    /// <summary>
    ///     Manage every state from player
    /// </summary>
    public class PlayerStateManager : BaseManager
    {
        #region PUBLIC ATRIBUTES
        public bool PlayerCantMove => _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);
        #endregion

        #region SERIALIZABLE ATRIBUTES
#pragma warning disable 0649
        [Header("RequiredFields")]
        [SerializeField]
        private UIManager _uiManager;
#pragma warning restore 0649
        #endregion

        #region PRIVATE ATRIBUTES
        private List<MovementMouseComponent> _movementMouseComponentList;
        private List<InteractableComponent> _interactableComponentList;
        #endregion

        #region PUBLIC METHODS
        public MovementMouseComponent GetActiveMovementMouseComponent()
        {
            return _movementMouseComponentList.FirstOrDefault(e => e.Active);
        }

        public void SetMovementMouseComponent(MovementMouseComponent movementMouseComponent)
        {
            _movementMouseComponentList.Add(movementMouseComponent);
        }

        public void RemoveMovementMouseComponent(MovementMouseComponent movementMouseComponent)
        {
            _movementMouseComponentList.Remove(movementMouseComponent);
        }

        public InteractableComponent GetActiveInteractableComponent()
        {
            return _interactableComponentList.FirstOrDefault(e => e.Active);
        }

        public void SetInteractableComponent(InteractableComponent interactableComponent)
        {
            _interactableComponentList.Add(interactableComponent);
        }

        public void RemoveInteractableComponent(InteractableComponent interactableComponent)
        {
            _interactableComponentList.Remove(interactableComponent);
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void ValidateValues()
        {
            // => Required Fields
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }
        protected override void SetInitialValues()
        {
            _movementMouseComponentList = new List<MovementMouseComponent>();
            _interactableComponentList = new List<InteractableComponent>();
        }
        #endregion
    }
}

using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.Stomach;

namespace Assets.Scripts.Managers.PlayerState
{
    /// <summary>
    ///     Manage every state from player
    /// </summary>
    public class PlayerStateManager : BaseManager
    {
        #region PUBLIC ATRIBUTES
        public bool PlayerCantMove
        {
            get
            {
                bool mouseIsOverSomeUi = _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);
                bool playerIsDoingSomeAction = this.GetActiveStomachComponent().IsPooping || this.GetActiveInteractableComponent().IsPlantingOrEating;

                return mouseIsOverSomeUi || playerIsDoingSomeAction;
            }
        }

        public bool PlayerIsDoingSomeAction { get; set; }
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
        private List<StomachComponent> _stomachComponentList;
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

        public StomachComponent GetActiveStomachComponent()
        {
            return _stomachComponentList.FirstOrDefault(e => e.Active);
        }
        public void SetStomachComponent(StomachComponent stomachComponent)
        {
            _stomachComponentList.Add(stomachComponent);
        }
        public void RemoveStomachComponent(StomachComponent stomachComponent)
        {
            _stomachComponentList.Remove(stomachComponent);
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
            _stomachComponentList = new List<StomachComponent>();
        }
        #endregion
    }
}

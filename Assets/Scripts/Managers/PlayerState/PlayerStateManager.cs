using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Interactable;

namespace Assets.Scripts.Managers.PlayerState
{
    public class PlayerStateManager : BaseManager
    {
#pragma warning disable 0649
        [Header("RequiredFields")]
        [SerializeField]
        private UIManager _uiManager;

        private List<MovementMouseComponent> _movementMouseComponentList;
        private List<InteractableComponent> _interactableComponentList;
#pragma warning restore 0649

        public bool PlayerCantMove => _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);

        protected override void SetInitialValues()
        {
            _movementMouseComponentList = new List<MovementMouseComponent>();
            _interactableComponentList = new List<InteractableComponent>();
        }

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

        protected override void ValidateValues()
        {
            // => Required Fields
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }
    }
}

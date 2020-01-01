using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Structure.Player
{
    public class PlayerStructure : StructureBase
    {
        #region PUBLIC ATRIBUTES
        public bool IsMainPlayer => _isMainPlayer;
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("Configuration Fields")]
        [SerializeField]
        private bool _isMainPlayer;
        [SerializeField]
        private bool _canMoveByClick;
        [SerializeField]
        private bool _canInteract;
        [SerializeField]
        private bool _canPoop;
        #endregion

        #region PRIVATE ATRIBUTES
        private MovementMouseComponent _movementMouseComponent;
        private InteractableComponent _interactableComponent;
        private StomachComponent _stomachComponent;
        #endregion


        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_canMoveByClick) _movementMouseComponent = this.GetComponent<MovementMouseComponent>();
            if (_canInteract) _interactableComponent = this.GetComponent<InteractableComponent>();
            if (_canPoop) _stomachComponent = this.GetComponent<StomachComponent>();
        }

        protected override void ValidateValues()
        {
            if (_canMoveByClick && _movementMouseComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_movementMouseComponent),this.gameObject.name));
            if (_canInteract && _interactableComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_interactableComponent), this.gameObject.name));
            if (_canPoop && _stomachComponent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_stomachComponent), this.gameObject.name));
        }
        #endregion
    }
}

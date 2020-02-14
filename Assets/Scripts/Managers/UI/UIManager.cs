using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.ActionSlot;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    /// <summary>
    ///     Manage every UI in the game
    /// </summary>
    public class UIManager : BaseManager
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Required Fields")]
        [SerializeField]
        private GameObject _stomachUIComponent;
        [SerializeField]
        private GameObject _inventoryComponent;
        [SerializeField]
        private GameObject _actionSlot;

        [SerializeField]
        [Tooltip("All the slots 4 max")]
        private ActionSlotComponent[] _actionSlotComponentlist;
        private InputManager _inputManager;
        #endregion


        #region PUBLIC METHODS
        public ActionSlotComponent GetSelectedActionSlot()
        {
            return _actionSlotComponentlist.FirstOrDefault(e => e.IsSelected);
        }
        #endregion

        #region UNTIY METHODS
        private void Start()
        {
            _actionSlotComponentlist[0].SelectSlot();
        }

        internal void ActivateActionSlots(bool value)
        {
            _actionSlot.SetActive(value);
        }

        internal void ActivateStomach(bool value)
        {
            _stomachUIComponent.SetActive(value);
        }

        internal void ActivateInventory(bool value)
        {
            _inventoryComponent.SetActive(value);
        }

        private void Update()
        {
            this.ControllTheSelectedSlots();
        }
        #endregion

        #region PRIVATE METHODS
        private void ControllTheSelectedSlots()
        {
            if (_inputManager.SlotOne == 1 && !_actionSlotComponentlist[0].IsSelected)
            {
                DiselectEveryActionSlot();
                _actionSlotComponentlist[0].SelectSlot();
            }

            if (_inputManager.SlotTwo == 1 && !_actionSlotComponentlist[1].IsSelected)
            {
                DiselectEveryActionSlot();
                _actionSlotComponentlist[1].SelectSlot();
            }

            if (_inputManager.SlotThree == 1 && !_actionSlotComponentlist[2].IsSelected)
            {
                DiselectEveryActionSlot();
                _actionSlotComponentlist[2].SelectSlot();
            }

            if (_inputManager.SlotFor == 1 && !_actionSlotComponentlist[3].IsSelected)
            {
                DiselectEveryActionSlot();
                _actionSlotComponentlist[3].SelectSlot();
            }
        }

        private void DiselectEveryActionSlot()
        {
            foreach (var actionSlot in _actionSlotComponentlist)
            {
                actionSlot.DeselectSlot();
            }  
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues()
        {
            if (!_actionSlotComponentlist.Any()) Debug.LogError("UIManager need to have at least one ActionSlot");
            if (_actionSlotComponentlist.Count() != 4) Debug.LogError("This manager is prepared to have only 4 slots!");
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
        }
        #endregion
    }
}

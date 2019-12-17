using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.ActionSlot;
using Assets.Scripts.Components.GenericUI;
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
        #region PUBLIC ATRIBUTES
        public List<GenericUIComponent> GenericUIList { get; private set; }
        #endregion

        #region SERIALIZABLE ATRIBUTES
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        [Tooltip("All the slots 4 max")]
        private ActionSlotComponent[] _actionSlotComponentlist;
        private InputManager _inputManager;
#pragma warning restore 0649
        #endregion

        #region PUBLIC METHODS
        public ActionSlotComponent GetSelectedActionSlot()
        {
            return _actionSlotComponentlist.FirstOrDefault(e => e.IsSelected);
        }

        public void SetGenericUIComponent(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Add(genericUIComponent);
        }

        public void RemoveGenericUIComponent(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Remove(genericUIComponent);
        }

        #endregion

        #region UNTIY METHODS
        private void Start()
        {
            _actionSlotComponentlist[0].SelectSlot();
        }

        private void Update()
        {
            this.ControllTheSelectedSlots();
        }
        #endregion

        #region PRIVATE ATRIBUTES
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
            GenericUIList = new List<GenericUIComponent>();
        }
        #endregion
    }
}

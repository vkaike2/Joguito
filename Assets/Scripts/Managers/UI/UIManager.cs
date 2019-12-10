using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.ActionSlot;
using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    public class UIManager : BaseManager
    {
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        [Tooltip("All the slots 4 max")]
        private ActionSlotComponent[] ActionSlotComponentlist;
        public List<GenericUIComponent> GenericUIList { get; private set; }

        private InputManager _inputManager;
#pragma warning restore 0649

        private void Start()
        {
            ActionSlotComponentlist[0].SelectSlot();
        }

        private void Update()
        {
            this.ControllTheSelectedSlots();
        }

        private void ControllTheSelectedSlots()
        {
            if (_inputManager.SlotOne == 1 && !ActionSlotComponentlist[0].IsSelected)
            {
                DiselectEveryActionSlot();
                ActionSlotComponentlist[0].SelectSlot();
            }

            if (_inputManager.SlotTwo == 1 && !ActionSlotComponentlist[1].IsSelected)
            {
                DiselectEveryActionSlot();
                ActionSlotComponentlist[1].SelectSlot();
            }

            if (_inputManager.SlotThree == 1 && !ActionSlotComponentlist[2].IsSelected)
            {
                DiselectEveryActionSlot();
                ActionSlotComponentlist[2].SelectSlot();
            }

            if (_inputManager.SlotFor == 1 && !ActionSlotComponentlist[3].IsSelected)
            {
                DiselectEveryActionSlot();
                ActionSlotComponentlist[3].SelectSlot();
            }
        }

        private void DiselectEveryActionSlot()
        {
            foreach (var actionSlot in ActionSlotComponentlist)
            {
                actionSlot.DeselectSlot();
            }  
        }

        public void SetGenericUIComponent(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Add(genericUIComponent);
        }

        public void RemoveGenericUIComponent(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Remove(genericUIComponent);
        }

        protected override void ValidateValues()
        {
            if (!ActionSlotComponentlist.Any()) Debug.LogError("UIManager need to have at least one ActionSlot");
            if (ActionSlotComponentlist.Count() != 4) Debug.LogError("This manager is prepared to have only 4 slots!");
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            GenericUIList = new List<GenericUIComponent>();
        }
    }
}

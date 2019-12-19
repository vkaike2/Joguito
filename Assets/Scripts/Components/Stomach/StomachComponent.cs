using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Stomach
{
    /// <summary>
    ///     Represents the player stomach
    /// </summary>
    public class StomachComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private StomachSlotComponent[] _stomachSlotComponentList;
        private InputManager _inputManager;
        private bool _isPooping = false;
        private PoopScriptable[] _poopList;
        #endregion

        #region PUBLIC METHODS
        public void AddFood(ItemDTO food)
        {
            if (food.Amount > 1 || food.Amount == 0) Debug.LogError("You can only eat one flower at ounce!");
            if (food.Item.ItemType != EnumItemScriptableType.Flower) Debug.LogError("You can only eat flowerrs in this world!");

            StomachSlotComponent firstEmptySlot = _stomachSlotComponentList.FirstOrDefault(e => e.StomachSlotState == EnumStomachSlotState.Empty);
            firstEmptySlot.AddFood(food.Item);
        }

        public bool StomachCantAcceptNewFood()
        {
            return !_stomachSlotComponentList.Any(e => e.StomachSlotState == EnumStomachSlotState.Empty);
        }
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            ManageTheButHole();
        }
        #endregion

        #region PRIVATE METHODS
        private void ManageTheButHole()
        {
            if (_inputManager.PoopButton == 1 && !_isPooping && _stomachSlotComponentList.Any(e => e.StomachSlotState == EnumStomachSlotState.ReadyToPoop))
            {
                List<StomachSlotComponent> currentStomachSlotComponents = _stomachSlotComponentList.Where(e => e.StomachSlotState == EnumStomachSlotState.ReadyToPoop).ToList();
                string currentFlowersRecipeInStomach = String.Join("-", currentStomachSlotComponents.Select(e => e.FlowerHashCode).OrderBy(e => e).ToList());

                PoopScriptable curentPoop = _poopList.FirstOrDefault(e => e.Recipe == currentFlowersRecipeInStomach);
                Debug.Log(curentPoop.Name);

                // => Remove the plant from the stomach after the poop
                foreach (var stomachSlot in currentStomachSlotComponents)
                {
                    stomachSlot.RemoveFood();
                }
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _stomachSlotComponentList = this.GetComponentsInChildren<StomachSlotComponent>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _poopList = Resources.LoadAll<PoopScriptable>("ScriptableObjects/Poops");
        }

        protected override void ValidateValues()
        {
            if (!_stomachSlotComponentList.Any()) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_stomachSlotComponentList), this.gameObject.name));
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }
        #endregion
    }
}

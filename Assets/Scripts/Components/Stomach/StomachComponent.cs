using Assets.Scripts.DTOs;
using Assets.Scripts.ScriptableComponents.Item;
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

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _stomachSlotComponentList = this.GetComponentsInChildren<StomachSlotComponent>();
        }

        protected override void ValidateValues()
        {
            if (!_stomachSlotComponentList.Any()) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_stomachSlotComponentList), this.gameObject.name));
        }
        #endregion
    }
}

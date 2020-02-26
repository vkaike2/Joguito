using Assets.Scripts.DTOs;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Stomach
{
    public class StomachUIComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private StomachSlotComponent[] _stomachSlotComponentList;
        private StomachComponent _currentStomachComponent;
        #endregion

        #region PUBLIC METHODS
        public void SetStomach(StomachComponent stomachComponent)
        {
            // => Remove every old foods
            foreach (StomachSlotComponent stomahcSlot in _stomachSlotComponentList)
                stomahcSlot.RemoveFood();

            _currentStomachComponent = stomachComponent;

            // => Add new foods
            foreach (StomachItemDTO stomachItem in _currentStomachComponent.StomachItemList)
                _stomachSlotComponentList.FirstOrDefault(e => e.StomachSlotState == EnumStomachSlotState.Empty).AddFood(stomachItem);
        }

        public void AddFood(StomachItemDTO stomachItem)
        {
            _stomachSlotComponentList.FirstOrDefault(e => e.StomachSlotState == EnumStomachSlotState.Empty).AddFood(stomachItem);
        }

        public void UpdateCdwFillAmountDigestionBar(StomachItemDTO stomachItem)
        {
            _stomachSlotComponentList.FirstOrDefault(e => e.StomachSlotState == EnumStomachSlotState.HasFood && e.StomachItemId == stomachItem.Id).UpdateDigestionBar(stomachItem.DigestionPercent);
        }

        public void RemoveFoods(List<Guid> foodToPoopIdList)
        {
            List<StomachSlotComponent> foodsToRemoveList = _stomachSlotComponentList.Where(e => e.StomachSlotState == EnumStomachSlotState.HasFood && foodToPoopIdList.Contains(e.StomachItemId)).ToList();
            foreach (StomachSlotComponent stomachSlot in foodsToRemoveList)
            {
                stomachSlot.RemoveFood();
            }
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

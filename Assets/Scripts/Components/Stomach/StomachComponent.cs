using Assets.Scripts.DTOs;
using Assets.Scripts.ScriptableComponents.Item;
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
        #region SERIALIZABLE ATRIBUTES
        [Header("Configuration")]
        [SerializeField]
        private int _totalAmout;
        private List<ItemDTO> _foodList;
        #endregion

        #region PUBLIC METHODS
        public void AddFood(ItemDTO food)
        {
            if (food.Amount > 1 || food.Amount == 0) Debug.LogError("You can only eat one flower at ounce!");
            if (food.Item.ItemType != EnumItemScriptableType.Flower) Debug.LogError("You can only eat flowerrs in this world!");
            if(_foodList.Count > _totalAmout) Debug.LogError($"Your somach can only have {_totalAmout} of foods!");

            _foodList.Add(food);
        }

        public bool StomachCantAcceptNewFood()
        {
            return _foodList.Count >= _totalAmout;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _foodList = new List<ItemDTO>();

            if (_totalAmout == 0) _totalAmout = 3;
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

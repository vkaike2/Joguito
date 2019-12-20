using Assets.Scripts.DTOs;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Stomach
{
    /// <summary>
    ///     Every slot of stomach has one of those
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class StomachSlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public EnumStomachSlotState StomachSlotState { get; private set; }
        public Guid StomachItemId => _currentStomchItem.Id;
        #endregion

        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private Image _cdwDigestionBar;
        #endregion

        #region PRIVATE ATRIBUTES
        private Image _image;
        private StomachItemDTO _currentStomchItem;
        #endregion

        #region PUBLIC METHODS
        public void AddFood(StomachItemDTO stomachItem)
        {
            _currentStomchItem = stomachItem;
            _image.enabled = true;
            _cdwDigestionBar.enabled = true;
            _cdwDigestionBar.fillAmount = 0f;
            _image.sprite = stomachItem.Item.InventorySprite;
            StomachSlotState = EnumStomachSlotState.HasFood;
        }

        public void RemoveFood()
        {
            _currentStomchItem = null;
            _cdwDigestionBar.enabled = false;
            _image.sprite = null;
            StomachSlotState = EnumStomachSlotState.Empty;
        }

        public void UpdateDigestionBar(float digestionPercent)
        {
            _cdwDigestionBar.fillAmount = digestionPercent;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _image = this.GetComponent<Image>();
            StomachSlotState = EnumStomachSlotState.Empty;
        }

        protected override void ValidateValues()
        {
            if (_image == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_image), this.gameObject.name));
            if (_cdwDigestionBar == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_cdwDigestionBar), this.gameObject.name));
        }
        #endregion
    }
}

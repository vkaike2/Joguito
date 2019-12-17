using Assets.Scripts.Managers.Item;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.ToggleableItem
{
    /// <summary>
    ///     Represents a UI object that will be toggled whe player press Z or alt
    /// </summary>
    public class ToggleableItemComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private Canvas _childrenCanvas;
        private ItemManager _itemManager;
        #endregion

        #region PUBLIC METHODS
        public void SetActive(bool value)
        {
            _childrenCanvas.gameObject.SetActive(value);
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.SetActive(_itemManager.ItemNameIsVisible);
            _itemManager.SetToggleabeItem(this);
        }

        private void OnDestroy()
        {
            _itemManager.RemoveToggleableItem(this);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _childrenCanvas = this.gameObject.GetComponentInChildren<Canvas>();
            _itemManager = GameObject.FindObjectOfType<ItemManager>();
        }

        protected override void ValidateValues()
        {
            if (_childrenCanvas == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_childrenCanvas), this.gameObject.name));
            if (_itemManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_itemManager), this.gameObject.name));
        }
        #endregion
    }
}

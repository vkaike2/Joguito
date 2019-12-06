using Assets.Scripts.Managers.Item;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.ToggleableItem
{
    public class ToggleableItemComponent : BaseComponent
    {
        private Canvas _childrenCanvas;
        private ItemManager _itemManager;
        public void SetActive(bool value)
        {
            _childrenCanvas.gameObject.SetActive(value);
        }

        private void Start()
        {
            this.SetActive(_itemManager.ItemNameIsVisible);
            _itemManager.SetToggleabeItem(this);
        }

        private void OnDestroy()
        {
            _itemManager.RemoveToggleableItem(this);
        }

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
    }
}

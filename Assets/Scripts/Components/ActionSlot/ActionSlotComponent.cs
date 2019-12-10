using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.ActionSlot
{
    [RequireComponent(typeof(InventorySlotComponent))]
    [RequireComponent(typeof(Image))]
    public class ActionSlotComponent : BaseComponent
    {
        private Image _image;
        private InventorySlotComponent _inventorySlotComponent;

        public bool IsSelected { get; private set; }

        public void SelectSlot()
        {
            IsSelected = true;
            _image.color = Color.red;
        }

        public void DeselectSlot()
        {
            IsSelected = false;
            _image.color = Color.white;
        }

        protected override void SetInitialValues()
        {
            IsSelected = false;
            _image = this.GetComponent<Image>();
            _inventorySlotComponent = this.GetComponent<InventorySlotComponent>();
        }

        protected override void ValidateValues()
        {
            if (_image == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_image), this.gameObject.name));
            if (_inventorySlotComponent == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inventorySlotComponent), this.gameObject.name));
        }
    }
}

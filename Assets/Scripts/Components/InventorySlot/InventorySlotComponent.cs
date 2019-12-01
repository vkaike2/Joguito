using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Components.Draggable;
using Assets.Scripts.ScriptableComponents.Item;
using TMPro;

namespace Assets.Scripts.Components.InventorySlot
{
    public class InventorySlotComponent : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private Image _currentImage; // itemChild image
        [SerializeField]
        private InventoryDraggableItemComponent _draggableItem;
        [SerializeField]
        private TextMeshProUGUI _txtAmount;

        public Image CurrentImage => _currentImage;
        public ItemScriptable CurrentItem { get; private set; }
        public bool HasItem => CurrentItem != null && _currentImage.enabled;
        public int Amout { get; set; }

        private void Start()
        {
            _txtAmount.enabled = false;
            //_txtAmount.SetText(Amout == 0 ? "" : Amout.ToString());
            this.AddAmount(Amout);
        }

        public void OnClick()
        {
            if (HasItem)
            {
                _currentImage.enabled = false;
                _txtAmount.enabled = false;
                _draggableItem.StartDragging(this);
            }
        }

        public void EnableSlot()
        {
            _txtAmount.enabled = true;
            CurrentImage.enabled = true;
        }

        public void AddItem(ItemScriptable newItem)
        {
            if (!HasItem)
            {
                _currentImage.sprite = newItem.InventorySprite;
                //Amout = newItem.TotalAmout;
                _currentImage.enabled = true;
                CurrentItem = newItem;

                if (newItem.Stackable)
                {
                    this.AddAmount(Amout);
                    _txtAmount.enabled = true;
                }
            }
            else
            {
                //this.AddAmount(Amout + newItem.TotalAmout);
                _txtAmount.enabled = true;
            }
        }

        public void RemoveItem()
        {
            if (!HasItem) return;

            CurrentItem = null;
            _currentImage.enabled = false;
            _txtAmount.SetText("");
        }

        public void AddAmount(int amount)
        {
            if (!HasItem || !CurrentItem.Stackable) return;

            _txtAmount.SetText(Amout == 0 ? "" : amount.ToString());
            Amout = amount;
            //CurrentItem.TotalAmout = amount;
        }
    }
}

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
            _txtAmount.SetText(Amout == 0 ? "" : Amout.ToString());
        }

        public void OnClick()
        {
            if (HasItem)
            {
                _currentImage.enabled = false;
                _draggableItem.StartDragging(this);
            }
        }

        public void AddItem(ItemScriptable newItem)
        {
            if (HasItem) return;

            _currentImage.sprite = newItem.InventorySprite;
            Amout = newItem.TotalAmout;
            _currentImage.enabled = true;


            if (newItem.Stackable)
                _txtAmount.SetText(Amout == 0 ? "" : Amout.ToString());

            CurrentItem = newItem;
        }

        public void RemoveItem()
        {
            if (!HasItem) return;

            CurrentItem = null;
            _currentImage.enabled = false;
            _txtAmount.SetText("");
        }

    }
}

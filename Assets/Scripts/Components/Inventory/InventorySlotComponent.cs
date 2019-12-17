using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Components.Draggable;
using TMPro;
using Assets.Scripts.Utils;
using Assets.Scripts.DTOs;

namespace Assets.Scripts.Components.InventorySlot
{
    /// <summary>
    ///     Represents a inventory slot or a slot from action slot
    /// </summary>
    public class InventorySlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public Image CurrentImage => _currentImage;
        public ItemDTO CurrentItem { get; private set; }
        #endregion

        #region SERIALIZABLE ATRIBUTES
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        private Image _currentImage; // itemChild image
        [SerializeField]
        private TextMeshProUGUI _txtAmount;
        private InventoryDraggableItemComponent _draggableItem;
#pragma warning restore 0649
        #endregion

        #region PUBLIC METHODS
        // => This event will call when player click on the inventory Slot
        public void OnClick()
        {
            if (CurrentItem?.Item != null)
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

        public bool CheckIfCanAcceptItem(ItemDTO item)
        {
            if (CurrentItem == null || CurrentItem.Item == null) return true;
            else
            {
                // => Have item but isnt stackable
                if (!CurrentItem.Item.Stackable) return false;
                else
                {
                    // => Diferent Item
                    if (CurrentItem.Item.Name != item.Item.Name) return false;
                    else
                    {
                        // => Same item and can Stack
                        if (CurrentItem.Amount + item.Amount <= item.Item.MaxStackableAmout) return true;
                    }
                }
            }

            return false;
        }

        public void SetItem(ItemDTO newItem)
        {
            if (CurrentItem == null || CurrentItem.Item == null)
            {
                CurrentItem = newItem;
                _currentImage.sprite = newItem.Item.InventorySprite;
                _currentImage.enabled = true;
                UpdateAmount();
                if (newItem.Item.Stackable) _txtAmount.enabled = true;
            }
            else if (CurrentItem.Item.Stackable &&
                CurrentItem.Item.Name == newItem.Item.Name &&
                CurrentItem.Amount + newItem.Amount <= newItem.Item.MaxStackableAmout)
            {
                CurrentItem.Amount += newItem.Amount;
                UpdateAmount();
            }
        }

        public void RemoveItem()
        {
            if (CurrentItem == null || CurrentItem.Item == null) return;

            CurrentItem = null;
            _currentImage.enabled = false;
            _txtAmount.enabled = false;
        }

        public ItemDTO GetOneItem()
        {
            if (CurrentItem == null || CurrentItem.Item == null) Debug.LogError("You are tring to get one item from a empty slot");

            ItemDTO itemToReturn = new ItemDTO()
            {
                Amount = 1,
                Item = CurrentItem.Item
            };

            // => Has only one item
            if (CurrentItem.Amount == 1)
            {
                this.RemoveItem();
                return itemToReturn;
            }

            CurrentItem.Amount--;
            this.UpdateAmount();
            return itemToReturn;
        }
        #endregion

        #region PRIVATE METHODS
        private void UpdateAmount()
        {
            _txtAmount.enabled = CurrentItem.Item.Stackable;
            _txtAmount.SetText(CurrentItem.Amount == 0 ? "" : CurrentItem.Amount.ToString());
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void ValidateValues()
        {
            if (_currentImage == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_currentImage), this.gameObject.name));
            if (_draggableItem == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_draggableItem), this.gameObject.name));
            if (_txtAmount == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_txtAmount), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _txtAmount.enabled = false;
            _draggableItem = GameObject.FindObjectOfType<InventoryDraggableItemComponent>();
        }
        #endregion
    }
}

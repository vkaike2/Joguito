using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.ScriptableComponents.Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Inventory
{
    /// <summary>
    ///     Used to be a inventory, and store itens
    /// </summary>
    public class InventoryComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private GameObject _inventoryPanel;
        [SerializeField]
        private List<InventorySlotComponent> _slotList;
        #endregion

        #region PRIVATE ATRIBUTES
        private InputManager _inputManager;
        private bool _keyPressedInventory = false;
        #endregion

        public ItemScriptable mockItem;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddItem(new ItemDTO() { Item = mockItem, Amount = 1});
            }

            this.ToggleInventoryPannel();
        }

        private void ToggleInventoryPannel()
        {
            if (!_keyPressedInventory && _inputManager.Inventory == 1)
            {
                _keyPressedInventory = true;

                _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
            }

            if (_keyPressedInventory && _inputManager.Inventory == 0)
            {
                _keyPressedInventory = false;
            }
        }

        #region PUBLIC METHODS
        public bool InventoryIsFull()
        {
            return !_slotList.Any(e => e.CurrentItem == null);
        }

        public void AddItem(ItemDTO newItem)
        {
            InventorySlotComponent emptySlot = _slotList.FirstOrDefault(e => e.CheckIfCanAcceptItem(newItem));

            if (emptySlot == null)
            {
                Debug.LogError("Your inventory is full!");
                return;
            }

            emptySlot.SetItem(newItem);
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void SetInitialValues()
        {
            _slotList.AddRange(this.GetComponentsInChildren<InventorySlotComponent>());
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _inventoryPanel.SetActive(false);
        }
        protected override void ValidateValues()
        {
            if (!_slotList.Any()) Debug.LogError("A inventory must have his own slots!");
        }
        #endregion
    }
}

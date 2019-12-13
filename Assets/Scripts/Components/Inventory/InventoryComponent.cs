using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.ScriptableComponents.Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Inventory
{
    [HelpURL("https://slimwiki.com/vkaike9/inventorycomponent")]
    public class InventoryComponent : BaseComponent
    {
        [Header("RequiredFields")]
        [SerializeField]
        private GameObject _inventoryPanel;
        [SerializeField]
        private List<InventorySlotComponent> _slotList;
        private InputManager _inputManager;

        private bool _keyPressedInventory = false;
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

        public ItemScriptable mockItem;
        public ItemScriptable mockItem2;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddItem(new ItemDTO() { Item = mockItem, Amount = 1});
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                AddItem(new ItemDTO() { Item = mockItem2, Amount = 1 });
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
    }
}

using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.ScriptableComponents.Item;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Inventory
{
    public class InventoryComponent : BaseComponent
    {
        private InventorySlotComponent[] _slotList;

        protected override void SetInitialValues()
        {
            _slotList = this.GetComponentsInChildren<InventorySlotComponent>();
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

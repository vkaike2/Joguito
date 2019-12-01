using Assets.Scripts.Components.Bag;
using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.ScriptableComponents.Item;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private BagComponent _bagComponent;
        private InventorySlotComponent[] _slotList;

        [Header("Mock")]
        public ItemScriptable item01;
        public ItemScriptable item02;

        private void Start()
        {
            _slotList = this.GetComponentsInChildren<InventorySlotComponent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                //item01.TotalAmout = 2;
                this.AddItem(item01);
            }
            if (Input.GetKeyDown(KeyCode.Y))
                this.AddItem(item02);
        }

        public void AddItem(ItemScriptable newItem)
        {
            if (!_bagComponent.HasEmptySlots())
            {
                Debug.Log("My inventory is full!");
                return;
            }

            // => Add item to the player Bag
            _bagComponent.AddItem(newItem);

            //InventorySlotComponent emptySlot = _slotList
            //    .FirstOrDefault(e => !e.HasItem ||
            //                   (e.HasItem && e.CurrentItem.Name == newItem.Name && e.Amout + newItem.TotalAmout <= newItem.MaxStackableAmout));

            //emptySlot.AddItem(newItem);
        }

        public void RemoveItem(ItemScriptable item, int amount = 1)
        {

        }
    }
}

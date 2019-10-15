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
        public ItemScriptable item;

        private void Start()
        {
            _slotList = this.GetComponentsInChildren<InventorySlotComponent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                this.AddItem(item);
            }
        }

        public void AddItem(ItemScriptable newItem)
        {
            InventorySlotComponent emptySlot = _slotList.FirstOrDefault(e => !e.HasItem);
            if(emptySlot == null)
            {
                Debug.LogWarning("Your Inventory Is Full");
                return;
            }

            Vector2? position = emptySlot.AddItem(newItem.InventorySprite, newItem.TotalAmout);

            // => Add item to the player Bag
            _bagComponent.AddItem(newItem);
        }
        
        public void RemoveItem(ItemScriptable item, int amount = 1)
        {

        }
    }
}

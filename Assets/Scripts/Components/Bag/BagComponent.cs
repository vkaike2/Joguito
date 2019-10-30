using Assets.Scripts.ScriptableComponents.Item;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Bag
{
    public class BagComponent : MonoBehaviour
    {
        private int MAX_ITENS = 9;

        [SerializeField]
        private List<ItemScriptable> _itemList = new List<ItemScriptable>();

        public bool HasEmptySlots()
        {
            return _itemList.Count < MAX_ITENS;
        }

        public void AddItem(ItemScriptable newItem)
        {
            if (HasEmptySlots())
                _itemList.Add(newItem);
        }
    }
}

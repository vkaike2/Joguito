using Assets.Scripts.ScriptableComponents.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Bag
{
    public class BagComponent : MonoBehaviour
    {
        private int MAX_ITENS = 9;

        private ItemScriptable[] _itemList;

        private void Awake()
        {
            _itemList = new ItemScriptable[MAX_ITENS];
        }

        public void AddItem(ItemScriptable newItem)
        {
            // => Add item in to the bag
        }
    }
}

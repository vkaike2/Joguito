using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemScriptable : ScriptableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Stackable { get; set; }
        public int StackableAmout { get; set; }
        public Sprite InventorySprite { get; set; }

        private int _totalAmout { get; set; }
        public int TotalAmout
        {
            get
            {
                if (Stackable)
                    return _totalAmout;
                else
                    return 1;
            }
            set
            {
                _totalAmout = value;
            }
        }
    }
}

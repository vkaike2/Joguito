using Assets.Scripts.ScriptableComponents.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DTOs
{
    [Serializable]
    public class ItemDTO
    {
        [SerializeField]
        private ItemScriptable _item;
        [SerializeField]
        private int _amount;

        public ItemScriptable Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }
    }
}

using Assets.Scripts.ScriptableComponents.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DTOs
{
    public class ItemDTO
    {
        public ItemScriptable Item { get; set; }
        public int Amount { get; set; }
    }
}

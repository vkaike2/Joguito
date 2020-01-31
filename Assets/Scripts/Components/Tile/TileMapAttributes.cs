using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    [Serializable]
    public class TileMapAttributes
    {
        [SerializeField]
        private Sprite _sprite;
        [SerializeField]
        [Range(1,10)]
        private int _weight;

        public int Weight => _weight;
        public Sprite Sprite => _sprite;
    }
}

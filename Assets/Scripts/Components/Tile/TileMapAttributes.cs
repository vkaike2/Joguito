using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    [Serializable]
    public abstract class TileMapAttributes
    {
        [SerializeField]
        [Range(1, 10)]
        private int _weight;

        public int Weight => _weight;
    }

    [Serializable]
    public class TileMapSpriteAttributes : TileMapAttributes
    {
        [SerializeField]
        private Sprite _sprite;

        public Sprite Sprite => _sprite;
    }

    [Serializable]
    public class TileMapObjectsAttributes : TileMapAttributes
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private int _amount;

        public int Amount => _amount;
        public GameObject Prefab => _prefab;
    }
}

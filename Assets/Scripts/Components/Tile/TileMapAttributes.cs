using Assets.Scripts.ScriptableComponents.Boss;
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
    public class TileMapAnimatorAttribute
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private RuntimeAnimatorController _animatorController;
        [SerializeField]
        private List<TileMapAnimatorLayerAttribute> _animatorLayerIdList;
        [SerializeField]
        private EnumSide _enumSide;

        public RuntimeAnimatorController AnimatorController => _animatorController;
        public List<TileMapAnimatorLayerAttribute> AnimatorLayerIdList => _animatorLayerIdList;
        public EnumSide EnumSide => _enumSide;
    }

    [Serializable]
    public class TileMapAnimatorLayerAttribute: TileMapAttributes
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private int _layerId;

        public int LayerId => _layerId;
    }

    [Serializable]
    public class TileMapBossAttributes : TileMapAttributes
    {
        [SerializeField]
        private BossScriptable _bossScriptable;

        public BossScriptable BossScriptable => _bossScriptable;
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

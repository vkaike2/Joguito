using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    public class TileMapRandomGeneratorComponent : BaseComponent
    {
        #region SERIALIZABLE ATTRIBUTES
        [Header("Random Sprites")]
        [SerializeField]
        private List<TileMapSpriteAttributes> LeftUpSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> UpSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> RightUpSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> RightSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> LeftSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> BottomLeftSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> BottomSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> BottomRightSpriteList;
        [SerializeField]
        private List<TileMapSpriteAttributes> CenterSpriteList;
        #endregion

        #region PRIVATE ATTRIBUTES
        private List<TileMapComponnent> _tileMapChildrensList;
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.StartEveryTile();
        }
        #endregion

        #region PRIVATE METHODS
        private void StartEveryTile()
        {
            foreach (TileMapComponnent tileMap in _tileMapChildrensList)
            {
                switch (tileMap.Side)
                {
                    case EnumSide.LEFT_UP:
                        tileMap.SetInitialSprite(LeftUpSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.UP:
                        tileMap.SetInitialSprite(UpSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.RIGHT_UP:
                        tileMap.SetInitialSprite(RightUpSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.RIGHT:
                        tileMap.SetInitialSprite(RightSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.LEFT:
                        tileMap.SetInitialSprite(LeftSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.LEFT_BOTTOM:
                        tileMap.SetInitialSprite(BottomLeftSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.BOTTOM:
                        tileMap.SetInitialSprite(BottomSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.BOTTOM_RIGHT:
                        tileMap.SetInitialSprite(BottomRightSpriteList.GetRandomSprite());
                        break;
                    case EnumSide.CENTER:
                        tileMap.SetInitialSprite(CenterSpriteList.GetRandomSprite());
                        break;
                    default:
                        break;
                }
            }
        }

        public void SpawnNewObjects(GameObject prefab, int amount)
        {
            if (_tileMapChildrensList is null) _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>().ToList();
            for (int i = 0; i < amount; i++)
            {
                TileMapComponnent tileMap = _tileMapChildrensList.GetRandomTileMapWihtoutObstacle(new List<EnumSide>() { EnumSide.CENTER });

                tileMap.SpawnObject(prefab);
                if (tileMap is null) return;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_tileMapChildrensList is null) _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>().ToList();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

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
        private List<TileMapAttributes> LeftUpSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> UpSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> RightUpSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> RightSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> LeftSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> BottomLeftSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> BottomSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> BottomRightSpriteList;
        [SerializeField] 
        private List<TileMapAttributes> CenterSpriteList;
        #endregion

        #region PRIVATE ATTRIBUTES
        private TileMapComponnent[] _tileMapChildrensList;
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
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

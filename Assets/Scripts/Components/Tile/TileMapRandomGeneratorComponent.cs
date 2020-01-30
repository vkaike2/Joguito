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
        [Header("Configuration Fields")]
        [SerializeField] 
        private List<Sprite> LeftUpSpriteList;
        [SerializeField] 
        private List<Sprite> UpSpriteList;
        [SerializeField] 
        private List<Sprite> RightUpSpriteList;
        [SerializeField] 
        private List<Sprite> RightSpriteList;
        [SerializeField] 
        private List<Sprite> LeftSpriteList;
        [SerializeField] 
        private List<Sprite> BottomLeftSpriteList;
        [SerializeField] 
        private List<Sprite> BottomSpriteList;
        [SerializeField] 
        private List<Sprite> BottomRightSpriteList;
        [SerializeField] 
        private List<Sprite> CenterSpriteList;
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
                        tileMap.SetInitialSprite(LeftUpSpriteList.GetRandomElement());
                        break;
                    case EnumSide.UP:
                        tileMap.SetInitialSprite(UpSpriteList.GetRandomElement());
                        break;
                    case EnumSide.RIGHT_UP:
                        tileMap.SetInitialSprite(RightUpSpriteList.GetRandomElement());
                        break;
                    case EnumSide.RIGHT:
                        tileMap.SetInitialSprite(RightSpriteList.GetRandomElement());
                        break;
                    case EnumSide.LEFT:
                        tileMap.SetInitialSprite(LeftSpriteList.GetRandomElement());
                        break;
                    case EnumSide.LEFT_BOTTOM:
                        tileMap.SetInitialSprite(BottomLeftSpriteList.GetRandomElement());
                        break;
                    case EnumSide.BOTTOM:
                        tileMap.SetInitialSprite(BottomSpriteList.GetRandomElement());
                        break;
                    case EnumSide.BOTTOM_RIGHT:
                        tileMap.SetInitialSprite(BottomRightSpriteList.GetRandomElement());
                        break;
                    case EnumSide.CENTER:
                        tileMap.SetInitialSprite(CenterSpriteList.GetRandomElement());
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

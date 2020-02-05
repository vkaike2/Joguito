using Assets.Scripts.Components.Tile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TileMapAttributesExtensions
    {
        public static Sprite GetRandomSprite(this List<TileMapSpriteAttributes> list)
        {
            TileMapSpriteAttributes attribute = GetRandomAttribute(list);

            return attribute.Sprite;
        }

        public static (GameObject, int) GetRandomObject(this List<TileMapObjectsAttributes> list)
        {
            TileMapObjectsAttributes attribute = GetRandomAttribute(list);

            return (attribute.Prefab, attribute.Amount);
        }

        private static T GetRandomAttribute<T>(this List<T> list) where T : TileMapAttributes
        {
            int fullWeight = list.Sum(e => e.Weight);

            int randomWeight = Random.Range(1, fullWeight);

            int randomCount = 0;
            int currentWeigth = 0;

            for (int i = 0; i < list.Count(); i++)
            {
                currentWeigth += list[i].Weight;

                if (randomWeight <= currentWeigth)
                {
                    randomCount = i;
                    break;
                }
            }
            return list[randomCount];
        }

        public static TileMapComponnent GetRandomTileMapWihtoutObstacle(this List<TileMapComponnent> list, List<EnumSide> sideList = null)
        {
            if (sideList is null)
                list = list.Where(e => !e.HasObstacle).ToList();
            else
                list = list.Where(e => !e.HasObstacle && sideList.Contains(e.Side)).ToList();

            return list.GetRandomElement<TileMapComponnent>();
        }
    }
}

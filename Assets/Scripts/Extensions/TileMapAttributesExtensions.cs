using Assets.Scripts.Components.Tile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TileMapAttributesExtensions
    {
        public static Sprite GetRandomSprite(this List<TileMapAttributes> list)
        {
            int fullWeight = list.Sum(e => e.Weight);

            int randomWeight = Random.Range(1, fullWeight);

            int randomCount = 0;
            int currentWeigth = 0;

            for (int i = 0; i < list.Count(); i++)
            {
                currentWeigth += list[i].Weight;

                if(randomWeight <= currentWeigth)
                {
                    randomCount = i;
                    break;
                }
            }

            return list[randomCount].Sprite;
        }
    }
}

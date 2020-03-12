using Assets.Scripts.Components.Tile;
using Assets.Scripts.DTOs;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.ScriptableComponents.Mob;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TileMapAttributesExtensions
    {
        public static int GetRandomLayer(this List<TileMapAnimatorLayerAttribute> list)
        {
            TileMapAnimatorLayerAttribute attribute = GetRandomAttribute(list);

            return attribute.LayerId;
        }
        public static Sprite GetRandomSprite(this List<TileMapSpriteAttributes> list)
        {
            TileMapSpriteAttributes attribute = GetRandomAttribute(list);

            return attribute.Sprite;
        }

        public static (GameObject, int?) GetRandomGameObject(this List<TileMapObjectsAttributes> list, int tierMap)
        {
            List<TileMapObjectsAttributes> currentList = list.FilterByRange(tierMap);

            TileMapObjectsAttributes attribute = currentList.GetRandomAttribute();
            return (attribute?.Prefab, attribute?.Amount);
        }

        public static (MobScriptable, int?) GetRandomMob(this List<TileMapMobAttributes> list, int tierMap)
        {
            List<TileMapMobAttributes> currentList = list.FilterByRange(tierMap);

            TileMapMobAttributes attribute = currentList.GetRandomAttribute();
            return (attribute?.MobScriptable, attribute?.Amount);
        }

        public static BossScriptable GetRandomBoss(this List<TileMapBossAttributes> list, int tierMap)
        {
            List<TileMapBossAttributes> currentList = list.FilterByRange(tierMap);

            TileMapBossAttributes attribute = currentList.GetRandomAttribute();
            return attribute?.BossScriptable;
        }

        private static T GetRandomAttribute<T>(this List<T> list) where T : TileMapAttributes
        {
            if (!list.Any()) return null;

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

        private static List<T> FilterByRange<T>(this List<T> list, int tierMap) where T : TileMapAttributes
        {
            return list.Where(e =>
            {
                if (e.TierRange.x > tierMap || e.TierRange.y < tierMap) return false;
                return true;
            }).ToList();
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

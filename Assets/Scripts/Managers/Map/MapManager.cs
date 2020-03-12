using Assets.Scripts.Components.Map;
using Assets.Scripts.Components.Tile;
using Assets.Scripts.Extensions;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.ScriptableComponents.Mob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Map
{
    public class MapManager : BaseManager
    {
        #region SERIALIZABLE ATTRIBUTES
        [Header("Mock")] //TODO: Change to map prefab
        [SerializeField]
        private GameObject _mapPrefab;

        [Header("MAP CONFIGURATIONS")]
        [SerializeField]
        private List<MapAttributes> _mapAttributesList;
        
        [Header("OBJECT CONFIGURATIONS")]
        [SerializeField]
        private List<TileMapObjectsAttributes> _plantSpotList;

        [Header("MOBS/BOSS CONFIGURATIONS")]
        [SerializeField]
        private List<TileMapBossAttributes> _bossList;
        [SerializeField]
        private List<TileMapMobAttributes> _mobList;
        #endregion

        #region PRIVATE ATTRIBUTES
        private List<MapComponent> _mapComponentList = new List<MapComponent>();
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                this.SpawnNewRandomMap();
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void SpawnNewRandomMap()
        {
            List<MapComponent> mapsWithEmptySlots = _mapComponentList.Where(e => e.HasEmptySlots).ToList();

            MapComponent mapComponent = mapsWithEmptySlots[UnityEngine.Random.Range(0, mapsWithEmptySlots.Count())];
           
            if (mapComponent is null) return;
            mapComponent.SpawnNewMap(_mapPrefab);
        }

        public void AddMapComponent(MapComponent component)
        {
            component.Tier = _mapComponentList.Count;
            _mapComponentList.Add(component);
            SpawNewObjects(component);
            SetMapAttributes(component);
        }
        public void RemoveMapComponent(MapComponent component)
        {
            _mapComponentList.Remove(component);
        }
        #endregion

        #region PRIVATE METHODS

        private void SetMapAttributes(MapComponent mapComponent)
        {
            MapAttributes mapAttribute = _mapAttributesList.GetMapAttributeForTier(mapComponent.Tier);
            mapComponent.SetSpawnType(mapAttribute.SpawnType, mapAttribute.Time);
        }
        private void SpawNewObjects(MapComponent mapComponent)
        {
            TileMapRandomGeneratorComponent randomGeneratorComponent = mapComponent.GetComponentInChildren<TileMapRandomGeneratorComponent>();

            // => PlantSpot
            (GameObject, int?) randomPlantSpot = _plantSpotList.GetRandomGameObject(mapComponent.Tier);
            if (randomPlantSpot.Item1 != null)
                randomGeneratorComponent.SpawnNewObjects(randomPlantSpot.Item1, randomPlantSpot.Item2.GetValueOrDefault());

            // => Boss
            BossScriptable randomBoss = _bossList.GetRandomBoss(mapComponent.Tier);
            if (randomBoss != null)
                randomGeneratorComponent.SpawnNewBoss(randomBoss);

            // => Mob
            (MobScriptable, int?) randomMobs = _mobList.GetRandomMob(mapComponent.Tier);
            if (randomMobs.Item1 != null)
                randomGeneratorComponent.SpawnNewMobs(randomMobs.Item1, randomMobs.Item2.GetValueOrDefault());

        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

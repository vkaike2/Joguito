using Assets.Scripts.Components.Map;
using Assets.Scripts.Components.Tile;
using Assets.Scripts.Extensions;
using Assets.Scripts.ScriptableComponents.Boss;
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
        [Header("Mock")]///TODO: Change to map prefab
        [SerializeField]
        private GameObject _mapPrefab;

        [Header("Configurations")]
        [SerializeField]
        private List<TileMapObjectsAttributes> _plantSpotList;
        [SerializeField]
        private List<TileMapBossAttributes> _bossList;
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
            _mapComponentList.Add(component);
            SpawNewObjects(component);
        }
        public void RemoveMapComponent(MapComponent component)
        {
            _mapComponentList.Remove(component);
        }
        #endregion

        #region PRIVATE METHODS
        private void SpawNewObjects(MapComponent component)
        {
            TileMapRandomGeneratorComponent randomGeneratorComponent = component.GetComponentInChildren<TileMapRandomGeneratorComponent>();
            // => PlantSpot
            (GameObject, int) randomPlantSpot = _plantSpotList.GetRandomObject();
            randomGeneratorComponent.SpawnNewObjects(randomPlantSpot.Item1, randomPlantSpot.Item2);

            BossScriptable randomBoss = _bossList.GetRandomBoss();
            randomGeneratorComponent.SpawnNewBoss(randomBoss);
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

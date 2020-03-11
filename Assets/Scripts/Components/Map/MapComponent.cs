using Assets.Scripts.Components.MiniMap;
using Assets.Scripts.Managers.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Map
{
    public class MapComponent : BaseComponent
    {
        #region PUBLIC ATTRIBUTES
        public bool HasEmptySlots => _spawnPointList.Any(e => !e.HasMap);
        public Vector2 Coordinates { get; set; }
        public int Tier { get; set; }
        #endregion

        #region PRIVATE ATTRIBUTES
        private MapManager _mapManager;
        private List<MapSpawnComponent> _spawnPointList;
        private List<int> _mobList;
        private bool _canSpawnNewMap = false;
        private MiniMapComponent _miniMapComponent;
        #endregion

        #region PUBLIC METHODS
        public MapComponent SpawnNewMap(GameObject mapPrefab)
        {
            List<MapSpawnComponent> spanwPointsList = _spawnPointList.Where(e => !e.HasMap).ToList();
            if (!spanwPointsList.Any())
            {
                Debug.LogError("You're trying to spawn a Map into a spot that already has a map!");
                return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, spanwPointsList.Count());
            GameObject spawnedMapGameObject = Instantiate(mapPrefab, spanwPointsList[randomIndex].Transform.position, Quaternion.identity);
            MapComponent spawnedMap = spawnedMapGameObject.GetComponent<MapComponent>();

            switch (spanwPointsList[randomIndex].Position)
            {
                case EnumMapPosition.UP:
                    spawnedMap.Coordinates = new Vector2(Coordinates.x, Coordinates.y + 1);
                    break;
                case EnumMapPosition.DOWN:
                    spawnedMap.Coordinates = new Vector2(Coordinates.x, Coordinates.y - 1);
                    break;
                case EnumMapPosition.LEFT:
                    spawnedMap.Coordinates = new Vector2(Coordinates.x - 1, Coordinates.y);
                    break;
                case EnumMapPosition.RIGHT:
                    spawnedMap.Coordinates = new Vector2(Coordinates.x + 1, Coordinates.y);
                    break;
            }

            return spawnedMap;
        }

        public void AddNewMob(int mobInstanceID)
        {
            if (_mobList is null)
                _mobList = new List<int>();

            _mobList.Add(mobInstanceID);
        }

        public void RemoveNewMob(int mobInstanceID)
        {
            if (!_mobList.Any(e => e == mobInstanceID)) return;
            _mobList.Remove(mobInstanceID);

            if (!_mobList.Any())
                _canSpawnNewMap = true;
        }
        #endregion

        #region UNITY METHODS

        private void FixedUpdate()
        {
            if (_canSpawnNewMap)
            {
                _mapManager.SpawnNewRandomMap();
                _canSpawnNewMap = false;
            }
        }
        private void OnEnable()
        {
            _mapManager.AddMapComponent(this);

            if (_miniMapComponent != null)
                _miniMapComponent.AddMapComponent(this);
        }

        private void OnDisable()
        {
            _mapManager.RemoveMapComponent(this);

            if (_miniMapComponent != null)
                _miniMapComponent.RemoveMapComponent(this);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            Coordinates = new Vector2();
            _mapManager = GameObject.FindObjectOfType<MapManager>();
            _spawnPointList = this.GetComponentsInChildren<MapSpawnComponent>().ToList();
            _miniMapComponent = GameObject.FindObjectOfType<MiniMapComponent>();
            _mobList = new List<int>();
        }

        protected override void ValidateValues() { }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DTOs;
using UnityEngine;

namespace Assets.Scripts.Components.MiniMap
{
    public class MiniMapDrawerComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("PREFABS")]
        [SerializeField]
        private GameObject _mapSlotPrefab;

        [Header("SPAWN POINTS")]
        [SerializeField]
        private Transform _defaultSpawnPoint;
        #endregion

        #region PRIVATE ATRIBUTES
        private List<MapSlotComponent> _mapSlotComponentList;
        #endregion

        #region PUBLIC METHODS
        public void DrawMiniMap(MiniMapDTO miniMap)
        {
            if (_mapSlotComponentList is null) _mapSlotComponentList = new List<MapSlotComponent>();

            MapSlotComponent mapSlotComponent = _mapSlotComponentList.Where(e => e.MiniMap.Coordinates == miniMap.Coordinates).FirstOrDefault();

            // => SpawnNewMiniMap
            if (mapSlotComponent is null)
            {
                Transform spotToSpawn = _defaultSpawnPoint;
                // => GetAdjasentSpot
                if (_mapSlotComponentList.Any())
                {
                    spotToSpawn = _mapSlotComponentList
                        .Where(e => e.MiniMap.SpawnPointDTOList.Any(s => s.Coordinates == miniMap.Coordinates))
                        .Select(e => e.MiniMap.SpawnPointDTOList.FirstOrDefault(s => s.Coordinates == miniMap.Coordinates).SpawnPointTransform)
                        .FirstOrDefault();
                }

                GameObject gameObjectMap = Instantiate(_mapSlotPrefab, spotToSpawn);
                gameObjectMap.transform.parent = _defaultSpawnPoint.transform;
                gameObjectMap.transform.localScale = _mapSlotPrefab.transform.localScale;

                mapSlotComponent = gameObjectMap.GetComponent<MapSlotComponent>();
                mapSlotComponent.SetMiniMap(miniMap);

                _mapSlotComponentList.Add(mapSlotComponent);
            }
            // => UpdateCurrentMiniMapSlot;
            else
            {
                mapSlotComponent.UpdateMiniMap(miniMap);
            }

        }
        #endregion

        #region ABSTRACT METHDOS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

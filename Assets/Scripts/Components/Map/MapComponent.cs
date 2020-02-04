using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Map
{
    public class MapComponent : BaseComponent
    {
        #region SERIALIZABLE FIELD
        [Header("SPAWN POINTS")]
        [SerializeField]
        private List<SpawnPointDTO> _spawnPointList;

        public GameObject mapPrefab;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                this.SpawnNewMap(mapPrefab);
            }
        }


        #region PUBLIC METHODS
        public MapComponent SpawnNewMap(GameObject mapPrefab)
        {
            List<SpawnPointDTO> spanwPointsList = _spawnPointList.Where(e => !e.HasMap).ToList();
            if (!spanwPointsList.Any())
            {
                Debug.LogError("You're trying to spawn a Map into a spot that already has a map!");
                return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, spanwPointsList.Count());

            spanwPointsList[randomIndex].HasMap = true;
            GameObject spawnedMapGameObject = Instantiate(mapPrefab, spanwPointsList[randomIndex].Transform.position, Quaternion.identity);

            return spawnedMapGameObject.GetComponent<MapComponent>();
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues() { }

        protected override void ValidateValues() { }
        #endregion
    }

    [Serializable]
    public class SpawnPointDTO
    {
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private EnumPosition _position;


        public Transform Transform => _transform;
        public EnumPosition Position => _position;

        public bool HasMap { get; set; }
    }

    public enum EnumPosition
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}

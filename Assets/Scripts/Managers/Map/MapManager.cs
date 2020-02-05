using Assets.Scripts.Components.Map;
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
        #endregion

        #region PRIVATE ATTRIBUTES
        private List<MapComponent> _mapComponentList;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                List<MapComponent> mapsWithEmptySlots = _mapComponentList.Where(e => e.HasEmptySlots).ToList();
                mapsWithEmptySlots[UnityEngine.Random.Range(0, mapsWithEmptySlots.Count())].SpawnNewMap(_mapPrefab);
            }
        }

        #region PUBLIC METHODS
        public void AddMapComponent(MapComponent component)
        {
            _mapComponentList.Add(component);
        }

        public void RemoveMapComponent(MapComponent component)
        {
            _mapComponentList.Remove(component);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _mapComponentList = new List<MapComponent>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

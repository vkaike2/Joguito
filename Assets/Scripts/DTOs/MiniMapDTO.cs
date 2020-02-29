using Assets.Scripts.Components.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DTOs
{
    public class MiniMapDTO
    {
        public MiniMapDTO(MapComponent mapComponent)
        {
            MapComponent = mapComponent;
        }

        #region PLAYER
        public bool HasPlayer { get; set; }
        public bool HasPoop { get; set; }
        #endregion

        #region MAP
        public Vector2 Coordinates => MapComponent.Coordinates;
        public List<SlotSpawnDTO> SpawnPointDTOList { get; set; }
        public float HpPercentage { get; set; }
        #endregion

        public MapComponent MapComponent { get; set; }
    }

    public class SlotSpawnDTO
    {
        public Vector2 Coordinates { get; set; }
        public Transform SpawnPointTransform { get; set; }
    }
}

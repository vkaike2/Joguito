using Assets.Scripts.DTOs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.MiniMap
{
    public class MapSlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public MiniMapDTO MiniMap { get; private set; }
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("SPAWN POINTS")]
        [SerializeField]
        private Transform _spawnUp;
        [SerializeField]
        private Transform _spawnDown;
        [SerializeField]
        private Transform _spawnLeft;
        [SerializeField]
        private Transform _spawnRight;

        [Header("UI")]
        [SerializeField]
        private Image _hasPlayer;
        [SerializeField]
        private Image _hasPoop;
        #endregion

        #region PRIVATE ATRIBUTES
        private Image _image;
        #endregion

        #region PUBLIC METHODS
        public void SetMiniMap(MiniMapDTO miniMap)
        {
            MiniMap = miniMap;

            MiniMap.SpawnPointDTOList = new List<SlotSpawnDTO>();
            MiniMap.SpawnPointDTOList.Add(new SlotSpawnDTO()
            {
                Coordinates = new Vector2(MiniMap.Coordinates.x + 1, MiniMap.Coordinates.y),
                SpawnPointTransform = _spawnRight
            });
            MiniMap.SpawnPointDTOList.Add(new SlotSpawnDTO()
            {
                Coordinates = new Vector2(MiniMap.Coordinates.x - 1, MiniMap.Coordinates.y),
                SpawnPointTransform = _spawnLeft
            });
            MiniMap.SpawnPointDTOList.Add(new SlotSpawnDTO()
            {
                Coordinates = new Vector2(MiniMap.Coordinates.x, MiniMap.Coordinates.y + 1),
                SpawnPointTransform = _spawnUp
            });
            MiniMap.SpawnPointDTOList.Add(new SlotSpawnDTO()
            {
                Coordinates = new Vector2(MiniMap.Coordinates.x, MiniMap.Coordinates.y - 1),
                SpawnPointTransform = _spawnDown
            });

            this.UpdateUI();
        }

        public void UpdateMiniMap(MiniMapDTO miniMap)
        {
            MiniMap.HasPlayer = miniMap.HasPlayer;
            MiniMap.HasPoop = miniMap.HasPoop;

            this.UpdateUI();
        }
        #endregion

        #region PRIVATE METHODS
        private void UpdateUI()
        {
            _hasPlayer.gameObject.SetActive(MiniMap.HasPlayer);
            _hasPoop.gameObject.SetActive(MiniMap.HasPoop);

            if(MiniMap.HpPercentage >= 0.7f)
            {
                _image.color = Color.white;
            }
            else if (MiniMap.HpPercentage < 0.7f && MiniMap.HpPercentage >= 0.3f)
            {
                _image.color = Color.yellow;
            }
            else if (MiniMap.HpPercentage < 0.3f)
            {
                _image.color = Color.red;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _image = this.GetComponent<Image>();
        }

        protected override void ValidateValues()
        {
        }

        #endregion
    }
}

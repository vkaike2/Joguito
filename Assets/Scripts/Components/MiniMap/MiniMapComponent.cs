using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Map;
using Assets.Scripts.DTOs;
using Assets.Scripts.Structure.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.MiniMap
{
    public class MiniMapComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private List<MapComponent> _mapComponentList;
        private List<PlayerStructure> _playerStructueList;
        private List<MiniMapDTO> _miniMapList;
        private MiniMapDrawerComponent _miniMapDrawerComponent;
        #endregion

        #region PUBLIC METHODS
        public void AddMapComponent(MapComponent map)
        {
            if (_miniMapList is null)
                _miniMapList = new List<MiniMapDTO>();
            if (_mapComponentList is null)
                _mapComponentList = new List<MapComponent>();

            _mapComponentList.Add(map);
            _miniMapList.Add(new MiniMapDTO(map));
        }

        public void RemoveMapComponent(MapComponent map)
        {
            _mapComponentList.Remove(map);
            MiniMapDTO currentMinimap = _miniMapList.FirstOrDefault(e => e.MapComponent.GetInstanceID() == map.GetInstanceID());
            _miniMapList.Remove(currentMinimap);
        }

        public void AddPlayerStrucute(PlayerStructure player)
        {
            if (_playerStructueList is null)
                _playerStructueList = new List<PlayerStructure>();
            _playerStructueList.Add(player);
        }
        public void RemovePlayerStructure(PlayerStructure player)
        {
            _playerStructueList.Remove(player);
        }
        #endregion

        #region UNITY METHODS
        private void FixedUpdate()
        {
            UpdateMiniMap();
        }
        #endregion

        #region PRIVATE METHODS
        private void UpdateMiniMap()
        {
            if (!_miniMapList.Any()) return;

            List<PlayerStructureCollidingWIhtMapDTO> playerCollidingWithList = this.GetAllPlayerPositions();

            foreach (MiniMapDTO miniMap in _miniMapList)
            {
                // => Update the Player Status
                List<PlayerStructureCollidingWIhtMapDTO> currentPlayerCollidingWithList = playerCollidingWithList.Where(e => e.MapInstanceId == miniMap.MapComponent.GetInstanceID()).ToList();
                if (currentPlayerCollidingWithList.Any())
                {
                    List<int?> playersInstanceIdInMap = currentPlayerCollidingWithList.Select(e => e.PlayerStructureInstanceId).ToList();
                    List<PlayerStructure> playersInMap = _playerStructueList.Where(e => playersInstanceIdInMap.Contains(e.GetInstanceID())).ToList();

                    miniMap.HasPlayer = playersInMap.Any(e => e.IsMainPlayer);
                    miniMap.HasPoop = playersInMap.Any(e => !e.IsMainPlayer);
                }
                else
                {
                    miniMap.HasPlayer = false;
                    miniMap.HasPoop = false;
                }

                // => Update the Map HP
                DamageTakerComponent damageTakerComponent = miniMap.MapComponent.GetComponent<DamageTakerComponent>();
                miniMap.HpPercentage = damageTakerComponent.HealthPercenage; 

                _miniMapDrawerComponent.DrawMiniMap(miniMap);
            }
        }

        private List<PlayerStructureCollidingWIhtMapDTO> GetAllPlayerPositions()
        {
            List<PlayerStructureCollidingWIhtMapDTO> playerCollidingWithList = _playerStructueList
                                                                                .Select(e => new PlayerStructureCollidingWIhtMapDTO()
                                                                                {
                                                                                    PlayerStructureInstanceId = e.GetInstanceID(),
                                                                                    PlayerStructureTransform = e.transform
                                                                                })
                                                                                .ToList();
            foreach (PlayerStructureCollidingWIhtMapDTO playerCollidingWith in playerCollidingWithList)
            {
                RaycastHit2D[] hitList = Physics2D.RaycastAll(playerCollidingWith.PlayerStructureTransform.position, Vector2.zero);
                playerCollidingWith.MapInstanceId = hitList.Where(e => e.collider.GetComponent<MapComponent>() != null).Select(e => e.collider.GetComponent<MapComponent>().GetInstanceID()).FirstOrDefault();
            }

            return playerCollidingWithList;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _miniMapDrawerComponent = this.GetComponent<MiniMapDrawerComponent>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

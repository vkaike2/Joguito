using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.ActivePlayers
{
    public class ActivePlayersUIComponent : BaseComponent
    {
        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private GameObject _ActivePlayerSlotPrefab;
        #endregion

        #region PRIVATE ATRIBUTES
        private List<ActivePlayerSlotComponent> _playerSlotComponentList;
        private PlayerStateManager _playerStateManager;
        #endregion

        #region PUBLIC METHODS
        public int CreateNewPlayerSlotCompoennt(PlayerStructure playerStructure)
        {
            ActivePlayerSlotComponent nextSlot = GameObject.Instantiate(_ActivePlayerSlotPrefab, this.transform).GetComponent<ActivePlayerSlotComponent>();
            nextSlot.SetPlayerStructure(playerStructure);
            _playerSlotComponentList.Add(nextSlot);
            return nextSlot.GetInstanceID();
        }

        public void ActivatePlayerSlot(int instanceId)
        {
            foreach (ActivePlayerSlotComponent playerSlot in _playerSlotComponentList)
            {
                playerSlot.ActivateSlot(playerSlot.GetInstanceID() == instanceId);
            }
        }


        public void DesactivePlayerSlot(int instanceId)
        {

            ActivePlayerSlotComponent currentSlot = _playerSlotComponentList.FirstOrDefault(e => e != null && e.GetInstanceID() == instanceId);
            if (currentSlot is null) return;

            _playerSlotComponentList.Remove(currentSlot);

            Destroy(currentSlot.gameObject);

            _playerStateManager.ActiveNewPlayerStructure(_playerSlotComponentList.Select(e => e.CurrentPLayerStructureInstanceId).FirstOrDefault());
        }
        #endregion


        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _playerSlotComponentList = this.GetComponentsInChildren<ActivePlayerSlotComponent>().ToList();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

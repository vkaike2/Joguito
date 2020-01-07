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
        #endregion


        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _playerSlotComponentList = this.GetComponentsInChildren<ActivePlayerSlotComponent>().ToList();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

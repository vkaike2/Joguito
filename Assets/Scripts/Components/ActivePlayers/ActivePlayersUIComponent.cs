using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components.ActivePlayers
{
    public class ActivePlayersUIComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private List<ActivePlayerSlotComponent> _playerSlotComponentList;
        #endregion

        #region PUBLIC METHODS
        public int CreateNewPlayerSlotCompoennt()
        {
            ActivePlayerSlotComponent lastSpawnedPlayerSlot;
            if(_playerSlotComponentList.Count > 1)
            {
                lastSpawnedPlayerSlot = _playerSlotComponentList.LastOrDefault();
            }
            else
            {
                lastSpawnedPlayerSlot = _playerSlotComponentList.FirstOrDefault();
            }


            ActivePlayerSlotComponent nextSlot = lastSpawnedPlayerSlot.SpanwNewPlayerSlot();

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

using Assets.Scripts.Components.Stomach;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Structure.Player;
using UnityEngine;

namespace Assets.Scripts.Components.GenericPoop
{
    public class GenericPoopComponent : BaseComponent
    {
        #region PRIVATE ATRIBUTES
        private PoopScriptable _currentPoop;
        private StomachComponent _poopedBy;
        #endregion

        #region PUBLIC METHODS
        public void SetCurrentPoopScriptable(PoopScriptable poop, StomachComponent poopedBy)
        {
            _currentPoop = poop;
            _poopedBy = poopedBy;
        }

        public void Animator_SpawnPoop()
        {
            GameObject.Instantiate(_currentPoop.PoopPrefab, this.transform.position, Quaternion.identity).GetComponent<PlayerStructure>().TurnItIntoAPoop(_currentPoop, _poopedBy);
            Destroy(this.gameObject);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

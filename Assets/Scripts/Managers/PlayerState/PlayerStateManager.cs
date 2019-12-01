using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Managers.PlayerState
{
    public class PlayerStateManager : BaseManager
    {
#pragma warning disable 0649
        #region Required Fields
        [Header("RequiredFields")]
        [SerializeField]
        private UIManager _uiManager;
        #endregion
#pragma warning restore 0649

        #region Public Fields
        public bool PlayerCantMove => _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);
        #endregion

        protected override void SetInitialValues() { }

        protected override void ValidateValues()
        {
            // => Required Fields
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }
    }
}

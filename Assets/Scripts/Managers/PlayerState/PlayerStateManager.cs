using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Managers.PlayerState
{
    public class PlayerStateManager : BaseManager
    {
#pragma warning disable 0649
        [Header("RequiredFields")]
        [SerializeField]
        private UIManager _uiManager;
#pragma warning restore 0649

        public bool PlayerCantMove => _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);

        protected override void SetInitialValues() { }

        protected override void ValidateValues()
        {
            // => Required Fields
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }
    }
}

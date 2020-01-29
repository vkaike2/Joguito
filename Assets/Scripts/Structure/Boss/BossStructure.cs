using Assets.Scripts.Components.CombatAttributes;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.Structure.Player;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Structure.Boss
{
    public class BossStructure : StructureBase
    {
        #region SERIALIZABLE ATTRIBUTES
        [Header("Required Fields")]
        [SerializeField]
        private BossScriptable _bossScriptable;
        #endregion

        #region PRIVATE ATTRIBUTES
        private CombatAttributesComponent _combatAttributesComponnent;
        private Animator _bossAnimator;
        #endregion

        #region PUBLIC METHODS
        public void TunIntoABoss(BossScriptable bossScriptable)
        {
            _bossScriptable = bossScriptable;
            _bossAnimator.runtimeAnimatorController = _bossScriptable.BossAnimator;

            _combatAttributesComponnent.TurnItIntoABoss(bossScriptable);
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.TunIntoABoss(_bossScriptable);
        }
        #endregion

        

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _bossAnimator = this.GetComponent<Animator>();
            _combatAttributesComponnent = this.GetComponent<CombatAttributesComponent>();
        }

        protected override void ValidateValues()
        {
            if (_bossAnimator is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_bossAnimator), this.gameObject.name));
        }
        #endregion
    }
}

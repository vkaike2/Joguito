using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Map;
using Assets.Scripts.ScriptableComponents.Boss;
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
        private Animator _bossAnimator;
        private DamageTakerComponent _damageTakerComponent;
        private DamageDealerComponent _damageDealerComponent;
        private MapComponent _mapComponent;
        #endregion

        #region PUBLIC METHODS
        public void TunIntoABoss(BossScriptable bossScriptable)
        {
            _bossScriptable = bossScriptable;
            _bossAnimator.runtimeAnimatorController = _bossScriptable.BossAnimator;

            // => DamageTakerOptions 
            _damageTakerComponent.TurnItIntoABoss(bossScriptable);

            // => DamageDealerOptions
            _damageDealerComponent.TurnItIntoABoss(bossScriptable);
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.TunIntoABoss(_bossScriptable);
        }

        private void OnEnable()
        {
            RaycastHit2D[] hitList = Physics2D.RaycastAll(this.transform.position, Vector2.zero);

            foreach (RaycastHit2D hit in hitList)
            {
                if (_mapComponent != null) break;
                _mapComponent = hit.collider.GetComponent<MapComponent>();
            }

            if (_mapComponent is null) return;
            _mapComponent.AddNewMob(this.GetInstanceID());
        }

        public void OnDisable()
        {
            _mapComponent.RemoveNewMob(this.GetInstanceID());
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _damageDealerComponent = this.GetComponentInChildren<DamageDealerComponent>();
            _bossAnimator = this.GetComponent<Animator>();
            _damageTakerComponent = this.GetComponent<DamageTakerComponent>();
        }

        protected override void ValidateValues()
        {
            if (_bossAnimator is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_bossAnimator), this.gameObject.name));
        }
        #endregion
    }
}

using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Map;
using Assets.Scripts.ScriptableComponents.Mob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Structure.Mob
{
    public class MobStructure : StructureBase
    {
        #region SERIALIZABLE ATTRIBUTES
        [Header("Required Fields")]
        [SerializeField]
        private MobScriptable _mobScriptable;
        #endregion

        #region PRIVATE ATTRIBUTES
        private Animator _mobAnimator;
        private DamageTakerComponent _damageTakerComponent;
        private DamageDealerComponent _damageDealerComponent;
        private MapComponent _mapComponent;
        private bool _hasTurnedIntoAMob;
        #endregion

        #region PUBLIC METHODS
        public void TunIntoAMob(MobScriptable mobScriptable, int mapTier = 0)
        {
            if (_hasTurnedIntoAMob) return;

            _hasTurnedIntoAMob = true;
            _mobScriptable = mobScriptable;
            _mobAnimator.runtimeAnimatorController = _mobScriptable.MobAnimator;

            mobScriptable.ApplyMultiplier(mapTier);

            // => DamageTakerOptions 
            _damageTakerComponent.TurnItIntoAMob(mobScriptable);

            // => DamageDealerOptions
            _damageDealerComponent.TurnItIntoAMob(mobScriptable);
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.TunIntoAMob(_mobScriptable);
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
            _mapComponent?.RemoveNewMob(this.GetInstanceID());
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _hasTurnedIntoAMob = false;
            _damageDealerComponent = this.GetComponent<DamageDealerComponent>();
            _mobAnimator = this.GetComponent<Animator>();
            _damageTakerComponent = this.GetComponent<DamageTakerComponent>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}

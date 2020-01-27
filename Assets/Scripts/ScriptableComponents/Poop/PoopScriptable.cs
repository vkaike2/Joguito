using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Poop
{
    [CreateAssetMenu(fileName = "Poop", menuName = "ScriptableObjects/Poop", order = 1)]
    public class PoopScriptable : ScriptableBase
    {
        [Header("Required Fields")]
        [SerializeField]
        private string _name;
        [SerializeField]
        private RuntimeAnimatorController _poopAnimator;
        [SerializeField]
        private ItemScriptable[] _recipe;
        [Space]
        [SerializeField]
        private GameObject _poopPrefab;

        [Header("Configuration Fields")]
        [SerializeField]
        private float _deathCooldown;

        [Header("Player Structure")]
        [SerializeField]
        private bool _canMoveByClick;
        [SerializeField]
        private bool _canInteract;
        [SerializeField]
        private bool _canPoop;
        [SerializeField]
        private Sprite _spriteForActiveStatus;

        [Header("Interactable options")]
        [SerializeField]
        private bool _canPickupItem;
        [SerializeField]
        private bool _canPlant;
        [SerializeField]
        private bool _canTakeSeed;
        [SerializeField]
        private bool _canTakePlant;
        [SerializeField]
        private bool _canEat;
        [SerializeField]
        private bool _canAttack;

        [Header("Combat Attributes")]
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _cdwDamage;


        #region Common Options
        public string Name => _name;
        public RuntimeAnimatorController PoopAnimator => _poopAnimator;
        public string Recipe
        {
            get
            {
                List<int> hashCodeList = _recipe.Select(e => e.GetHashCode()).OrderBy(e => e).ToList();
                return String.Join("-", hashCodeList);
            }
        }
        public GameObject PoopPrefab => _poopPrefab;
        public float DeathCooldown => _deathCooldown;
        #endregion

        #region Player Structure
        public bool CanMoveByClick => _canMoveByClick;
        public bool CanInteract => _canInteract;
        public bool CanPoop => _canPoop;
        public Sprite SpriteForActiveStatus => _spriteForActiveStatus;
        #endregion

        #region Interactable Component
        public bool CanPickupItem => _canPickupItem;
        public bool CanPlant => _canPlant;
        public bool CanTakeSeed => _canTakeSeed;
        public bool CanTakePlant => _canTakePlant;
        public bool CanEat => _canEat;
        public bool CanAttack => _canAttack;
        #endregion

        #region Combat Attributes
        public float Health => _health;
        public float Damage => _damage;
        public float CdwDamage => _cdwDamage;
        #endregion

        protected override void ValidateValues()
        {
            if (_name == null) Debug.LogError("The value of Name cannot be null in some poop");
            if (_recipe == null) Debug.LogError("The value of Recipe cannot be null in some poop");
            if (_poopPrefab == null) Debug.LogError("The value of PoopPrefab cannot be null in some poop");
            if (_deathCooldown == 0) Debug.LogError("The value of _deathCooldown cannot be 0 in some poop");
        }
    }
}

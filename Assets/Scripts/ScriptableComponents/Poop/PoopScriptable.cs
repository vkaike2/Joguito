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
        [Header("Configuration Fields")]
        [SerializeField]
        private string _name;
        [SerializeField]
        private ItemScriptable[] _recipe;
        [SerializeField]
        private GameObject _poopPrefab;
        [SerializeField]
        private float _deathCooldown;


        public string Name => _name;
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

        protected override void ValidateValues()
        {
            if (_name == null) Debug.LogError("The value of Name cannot be null in some poop");
            if (_recipe == null) Debug.LogError("The value of Recipe cannot be null in some poop");
            if (_poopPrefab == null) Debug.LogError("The value of PoopPrefab cannot be null in some poop");
            if (_deathCooldown == 0) Debug.LogError("The value of _deathCooldown cannot be 0 in some poop");
        }
    }
}

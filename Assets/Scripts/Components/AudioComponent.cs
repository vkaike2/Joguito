using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class AudioComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("WALK")]
        [SerializeField]
        private AudioSource _walkSource;
        [SerializeField]
        private List<AudioClip> _walkClipList;

        [Header("PLANT")]
        [SerializeField]
        private AudioSource _plantSource;
        [SerializeField]
        private List<AudioClip> _plantClipList;

        [Header("EAT")]
        [SerializeField]
        private AudioSource _harvestSource;
        [SerializeField]
        private List<AudioClip> _harvestClipList;

        [Header("POOPING")]
        [SerializeField]
        private AudioSource _poopSource;
        [SerializeField]
        private List<AudioClip> _poopClipList;

        [Header("CLICK")]
        [SerializeField]
        private AudioSource _clickSource;
        [SerializeField]
        private List<AudioClip> _clickClipList;

        [Header("CHOSE PLAYER")]
        [SerializeField]
        private AudioSource _chosePlayerSource;
        [SerializeField]
        private List<AudioClip> _chosePlayerClipList;

        [Header("TAKING DAMAGE")]
        [SerializeField]
        private AudioSource _takingDamageSource;
        [SerializeField]
        private List<AudioClip> _takingDamageClipList;

        [Header("ATTACKING")]
        [SerializeField]
        private AudioSource _attackSource;
        [SerializeField]
        private List<AudioClip> _attackClipList;

        [Header("BOOS SPAWNING")]
        [SerializeField]
        private AudioSource _bossSpawningSource;
        [SerializeField]
        private List<AudioClip> _bossSpawningClipList;

        [Header("SHOUT")]
        [SerializeField]
        private AudioSource _shoutSource;
        [SerializeField]
        private List<AudioClip> _shoutClipList;
        #endregion

        public void Audio_Walk()
        {
            _walkSource.pitch = Random.Range(1, 1.5f);
            _walkSource.clip = _walkClipList[Random.Range(0, _walkClipList.Count())];
            _walkSource.Play();
        }

        public void Audio_Plant()
        {
            _plantSource.clip = _plantClipList[Random.Range(0, _walkClipList.Count())];
            _plantSource.Play();
        }

        public void Audio_Harvest()
        {
            _harvestSource.clip = _harvestClipList[Random.Range(0, _harvestClipList.Count())];
            _harvestSource.Play();
        }

        public void Audio_Poop()
        {
            _poopSource.clip = _poopClipList[Random.Range(0, _poopClipList.Count())];
            _poopSource.Play();
        }

        public void Audio_Click()
        {
            _clickSource.clip = _clickClipList[Random.Range(0, _clickClipList.Count())];
            _clickSource.Play();
        }

        public void Audio_ChosePlayer()
        {
            _chosePlayerSource.clip = _chosePlayerClipList[Random.Range(0, _chosePlayerClipList.Count())];
            _chosePlayerSource.Play();
        }

        public void Audio_TakingDamage()
        {
            _takingDamageSource.pitch = Random.Range(1, 1.5f);
            _takingDamageSource.clip = _takingDamageClipList[Random.Range(0, _takingDamageClipList.Count())];
            _takingDamageSource.Play();
        }

        public void Audio_Attack()
        {
            _attackSource.pitch = Random.Range(1, 1.5f);
            _attackSource.clip = _attackClipList[Random.Range(0, _attackClipList.Count())];
            _attackSource.Play();
        }

        public void Audio_BossSpawning()
        {
            _bossSpawningSource.clip = _bossSpawningClipList[Random.Range(0, _bossSpawningClipList.Count())];
            _bossSpawningSource.Play();
        }

        public void Audio_Shout()
        {
            _shoutSource.clip = _shoutClipList[Random.Range(0, _shoutClipList.Count())];
            _shoutSource.Play();
        }

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

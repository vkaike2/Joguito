using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DTOs
{
    [Serializable]
    public class AudioDTO
    {
        public string Name => _name;

        [SerializeField]
        private string _name;
        [SerializeField]
        private AudioSource _source;
        [SerializeField]
        private List<AudioClip> _clipList;
        [SerializeField]
        private bool _changePitch;


        public void Play()
        {
            if (_changePitch)
                _source.pitch = UnityEngine.Random.Range(1, 1.5f);

            _source.clip = _clipList[UnityEngine.Random.Range(0, _clipList.Count())];
            _source.Play();
        }
    }
}

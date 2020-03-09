using Assets.Scripts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class AudioComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("AUDIOS")]
        [SerializeField]
        public List<AudioDTO> _audioList;
        #endregion

        #region PUBLIC METHODS
        public void PlayAudio(string name)
        {
            _audioList.FirstOrDefault(e => e.Name == name).Play();
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

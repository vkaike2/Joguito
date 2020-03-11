using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DTOs
{
    [Serializable]
    public class RangeDTO
    {
        [SerializeField]
        private int _from;
        [SerializeField]
        private int _to;

        public int From => _from;
        public int To => _to;
    }
}

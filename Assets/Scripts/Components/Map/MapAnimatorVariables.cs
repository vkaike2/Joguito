using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Map
{
    public class MapAnimatorVariables
    {
        public int LifePercentage => Animator.StringToHash("life-percentage");
    }
}

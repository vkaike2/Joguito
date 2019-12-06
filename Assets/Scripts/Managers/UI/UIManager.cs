using System;
using System.Collections.Generic;
using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Components.Inventory;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    public class UIManager : BaseManager
    {
        public List<GenericUIComponent> GenericUIList { get; private set; }

        internal void SetGenericUI(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Add(genericUIComponent);
        }

        internal void RemoveGenericUI(GenericUIComponent genericUIComponent)
        {
            GenericUIList.Remove(genericUIComponent);
        }

        protected override void ValidateValues()
        {
        }

        protected override void SetInitialValues()
        {
            GenericUIList = new List<GenericUIComponent>();
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.Components
{
    public abstract class BaseComponent : BaseObject
    {
        protected bool _isActive = false;

        public virtual void SetActivationComponent(bool value)
        {
            _isActive = value;
        }
    }
}

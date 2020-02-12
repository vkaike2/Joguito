using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interface
{
    public interface IPlantable : IInteractable
    {
        bool Interact();
        void MouseOver(bool value);
    }
}

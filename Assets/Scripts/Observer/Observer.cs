using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public interface IObserver
    {
        void OnNotify(GameObject entity, Message message);
    }
}

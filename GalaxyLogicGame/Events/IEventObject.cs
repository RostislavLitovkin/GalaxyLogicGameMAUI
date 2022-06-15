using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyLogicGame.Events
{
    public interface IEventObject
    {
        Task Move();
    }
}

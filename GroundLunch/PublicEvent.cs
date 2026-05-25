using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundLunch
{
    internal class PublicEvent
    {
    }

    public static class MessageEvents
    {
        public static event Action<string> OnMessageMsnUpdateReceived;

        public static void SendMsnUpdateMessage(string message)
        {
            OnMessageMsnUpdateReceived?.Invoke(message);
        }
    }
}

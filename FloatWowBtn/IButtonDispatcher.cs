using System.Windows.Forms;

namespace FloatWowBtn
{
    public interface IButtonDispatcher
    {
        void Scan();
        bool SwitchAndActive(KeysAndMouseEvent mouseevents, int eachWowDelayMs = 1000);
    }

    public class KeysAndMouseUnion {
        public Keys Key { get; }
        public WindowApi.MouseType[] Events { get; }

        public KeysAndMouseUnion(KeysAndMouseEvent e) {
            Key = e.Key;
            Events = e.Events;
        }
    }

    /// <summary>
    /// 二选一的参数
    /// </summary>
    public class KeysAndMouseEvent {
        public KeysAndMouseEvent(Keys keys, bool active, bool switchToFront) {
            this.Key = keys;
            this.Active = active;
            this.SwitchToFront = switchToFront;
        }

        public KeysAndMouseEvent(WindowApi.MouseType[] events, bool active, bool switchToFront)
        {
            this.Events = events;
            this.Active = active;
            this.SwitchToFront = switchToFront;
        }
 
        public Keys Key { get; }
        public WindowApi.MouseType[] Events { get; }
        public bool Active { get; }
        public bool SwitchToFront { get; }
    }
}

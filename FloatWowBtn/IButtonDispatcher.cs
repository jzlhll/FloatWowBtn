using System.Windows.Forms;

namespace FloatWowBtn
{
    public interface IButtonDispatcher
    {
        void Scan();
        bool SwitchAndActive(bool active, Keys keys, int eachWowDelayMs=1000);
    }
}

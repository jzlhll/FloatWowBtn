using System;

namespace FloatWowBtn
{
    class ClickNoDouble
    {
        public static readonly ClickNoDouble Instance = new ClickNoDouble();

        private long lastClickTime;
        private const long DELAY_TIME = 1000L;

        public bool IsAccept() {
            var cur = DateTime.Now.Ticks / 10000;
            if (cur - lastClickTime > DELAY_TIME) {
                lastClickTime = cur;
                return true;
            }

            return false;
        }
    }
}

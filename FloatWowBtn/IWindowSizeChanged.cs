using System.Collections.Generic;

namespace FloatWowBtn
{
    class WindowSizeDispatcher {
        public delegate void IWindowSizeChanged(bool toBig);

        private static readonly WindowSizeDispatcher instance = new WindowSizeDispatcher();
        private WindowSizeDispatcher() { }

        public static WindowSizeDispatcher Instance { get { return instance; } }

        private readonly List<IWindowSizeChanged> windowSizeChangeds = new List<IWindowSizeChanged>();

        private bool mState = true; //当前是否是全屏
        public bool getState() {
            return mState;
        }

        public void Register(IWindowSizeChanged c) {
            if (!windowSizeChangeds.Contains(c)) windowSizeChangeds.Add(c);
        }

        public void Unregister(IWindowSizeChanged c)
        {
            if (windowSizeChangeds.Contains(c))
            {
                windowSizeChangeds.Remove(c);
            }
        }

        public void SwitchChange() {
            bool newState = !mState;
            foreach (var c in windowSizeChangeds)
            {
                c.Invoke(newState);
            }
            mState = newState;
        }
    }
}

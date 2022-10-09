using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static FloatWowBtn.FloatWindowBtnForm;

namespace FloatWowBtn
{
    class FloatSwitchButtonDispatcher : IButtonDispatcher
    {
        public static int[] SWITCH_TIMES = new int[] { 100, 150, 200, 180};
        private int GetSwitchDelayTime() {
           int index = new Random().Next(SWITCH_TIMES.Length);
            return SWITCH_TIMES[index];
        }

        private List<IntPtr> CurrentWowPtrs() {
            IntPtr intPtr = WindowApi.FindWindow(null, "魔兽世界");
            Console.WriteLine("first wow window str: " + intPtr);
            List<IntPtr> list = new List<IntPtr>();
            IntPtr intPtr2 = intPtr;
            while (!intPtr2.Equals(IntPtr.Zero))
            {
                Console.WriteLine("found wow window str: " + intPtr2);
                list.Add(intPtr2);
                intPtr2 = WindowApi.FindWindowEx(IntPtr.Zero, intPtr2, null, "魔兽世界");
            }
            return list;
        }

        public void Scan()
        {
            var list = CurrentWowPtrs();
            Context.Instance.SetAllWowPtrsAndNotified(list);

            RegisterOb();
        }

        private IntPtr mLastSwitchFrontAddr = IntPtr.Zero;

        private bool _isRegistObserverList = false;
        private void RegisterOb() {
            if (!_isRegistObserverList) {
                _isRegistObserverList = true;
                Context.Instance.AllWowPtrsChangedEvent += () => {
                    mLastSwitchFrontAddr = IntPtr.Zero;
                };
            }
        }

        private static int IndexOf(List<IntPtr> indexes, IntPtr addr) {
            for (int i = 0; i < indexes.Count; i++)
            {
                if (indexes[i] == addr) {
                    return i;
                }
            }

            return -1;
        }

        public bool SwitchAndActive(KeysAndMouseEvent mouseevents)
        {
            return SwitchAndActive(mouseevents, GetSwitchDelayTime());
        }

        public bool SwitchAndActive(KeysAndMouseEvent mouseevents, int eachWowDelayMs) {
            var list = CurrentWowPtrs();

            var curAllList = Context.Instance.CachedAllWowPtrs;
            if (list.Count != curAllList.Count) {
                return false;
            }

            for (int i = 0; i < list.Count; i++) {
                var index = IndexOf(curAllList, list[i]);
                if (index < 0)
                {
                    return false;
                }
            }

            var selectedList = Context.Instance.CurrentSelectedPtrs;
            var union = new KeysAndMouseUnion(mouseevents);
            bool toFront = mouseevents.SwitchToFront; //表示本次事件；是否允许所有窗口，即使不勾选的也要恢复到当前

            new Task(() =>
            {
                int num = 50;
                foreach (var p in curAllList)
                {
                    if (selectedList.Contains(p))
                    {
                        show(p, num, true, mouseevents.Active, union);
                        num += eachWowDelayMs;
                    }
                    else 
                    {
                        if (toFront) {
                            show(p, num, true, false, union); //由于不在勾选列表；我们就不做激活；但是允许toFront
                            num += eachWowDelayMs;
                        }
                    }
                }
            }).Start();
            
            return true;
        }
 
        private void show(IntPtr p, int delayTime, bool toFront, bool active, KeysAndMouseUnion obj)
        {
            if (!(p == IntPtr.Zero))
            {
                Thread.Sleep(delayTime);
                ALog.DAndTime("windows <" + p + "> to front:" + toFront + ", active " + active);
                if (toFront)
                {
                    WindowApi.SetForegroundWindow(p);
                }

                if (active && obj != null)
                {
                    if (obj.Events != null)
                    {
                        ALog.DAndTime("windows <" + p + " key: " + obj.Events);
                        WindowApi.ClickMouse(obj.Events);
                    } else
                    {
                        ALog.DAndTime("windows <" + p + " key: " + obj.Events);
                        WindowApi.ClickKeyboard(obj.Key);
                    }
                }
            }
        }

        public void Resort()
        {
            var curList = Context.Instance.CachedAllWowPtrs;
            var list = CurrentWowPtrs();
            if (Utils.IsSame(curList, list))
            {
                Utils.ListRandomByOffset1(list);
            }
            Context.Instance.SetAllWowPtrsAndNotified(list);
        }

        public bool SwitchIt()
        {
            var list = Context.Instance.CachedAllWowPtrs;

            if (mLastSwitchFrontAddr == IntPtr.Zero && list.Count > 0) {
                mLastSwitchFrontAddr = list[0];
            }
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == mLastSwitchFrontAddr)
                {
                    i++;
                    if (i == list.Count) {
                        i = 0;
                    }

                    IntPtr last = mLastSwitchFrontAddr;
                    show(last, 50, true, false, null);

                    mLastSwitchFrontAddr = list[i];
                    break;
                }
            }

            return true;
        }
    }

}

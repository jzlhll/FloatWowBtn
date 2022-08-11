using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FloatWowBtn
{
    class FloatSwitchButtonDispatcher : IButtonDispatcher
    {
        public static readonly int[] SWITCH_TIMES = new int[] { 100, 150, 200, 180};
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
            var wowPtrIndexes = new List<IntPtrIndex>();
            for (int i = 0; i < list.Count; i++) {
                wowPtrIndexes.Add(new IntPtrIndex() {
                    Addr = list[i],
                    Index = i
                });
            }
            Context.Instance.AllWowPtrs = wowPtrIndexes;
        }

        private static int IndexOf(List<IntPtrIndex> indexes, IntPtr addr) {
            for (int i = 0; i < indexes.Count; i++)
            {
                if (indexes[i].Addr == addr) {
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

            var curAllList = Context.Instance.AllWowPtrs;
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

            var selectedList = Context.Instance.CurrentSelectedPtrs();
            var allList = Context.Instance.AllWowPtrs;
            var union = new KeysAndMouseUnion(mouseevents);
            bool toFront = mouseevents.SwitchToFront; //表示本次事件；是否允许所有窗口，即使不勾选的也要恢复到当前

            new Task(() =>
            {
                int num = 50;
                foreach (var p in allList)
                {
                    if (selectedList.Contains(p.Addr))
                    {
                        show(p.Addr, num, true, mouseevents.Active, union);
                        num += eachWowDelayMs;
                    }
                    else 
                    {
                        if (toFront) {
                            show(p.Addr, num, true, false, union); //由于不在勾选列表；我们就不做激活；但是允许toFront
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

                if (active)
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
    }

    public class IntPtrIndex
    {
        public IntPtr Addr { set; get; }
        public int Index { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public bool SwitchAndActive(bool active, Keys keys)
        {
            return SwitchAndActive(active, keys, GetSwitchDelayTime());
        }

        public bool SwitchAndActive(bool active, Keys keys, int eachWowDelayMs) {
            var list = CurrentWowPtrs();

            var curAllList = Context.Instance.AllWowPtrs;
            if (list.Count != curAllList.Count) {
                return false;
            }

            for (int i = 0; i < list.Count;i++) {
                var index = IndexOf(curAllList, list[i]);
                if (index < 0)
                {
                    return false;
                }
            }

            var selectedList = Context.Instance.CurrentSelectedPtrs();

            int num = 50;
            foreach (var p in list)
            {
                if (selectedList.Contains(p)) {
                    OnceShowAsync(p, num, active, keys);
                    num += eachWowDelayMs;
                }
            }

            return true;
        }

        private async void OnceShowAsync(IntPtr p, int delayTime, bool active, Keys keys)
        {
            if (!(p == IntPtr.Zero))
            {
                await Task.Delay(delayTime);
                await Task.Run(delegate
                {
                    ALog.DAndTime("windows <" + p + "> to front:" + active);
                    WindowApi.SetForegroundWindow(p);

                    if (active) {
                        ALog.DAndTime("windows <" + p + " key: " + keys);
                        WindowApi.ClickKeyboard(keys);
                    }
                });
            }
        }
    }

    public class IntPtrIndex
    {
        public IntPtr Addr { set; get; }
        public int Index { set; get; }
    }
}

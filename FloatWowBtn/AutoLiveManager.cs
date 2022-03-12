using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatWowBtn
{
    class StartingInfo {
        public bool IsStarted;
        public long Ts;
    }

    class RandomKeyHelp {
        private static Keys Random(List<Keys> keysList) {
            var count = keysList.Count;
            var index = new Random().Next(count);
            return keysList[index];
        }

        public Keys Next() {
            var k = Random(AutoLiveManager.DefaultAutoLiveKeys);
            return k;
        }
    }

    class AutoLiveManager
    {
        public static readonly AutoLiveManager Instance = new AutoLiveManager();

        public delegate void InfoCallbackDelegate(string s);
        public InfoCallbackDelegate InfoCallback { set; private get; }

        public bool Enabled { set; get; } = false;

        private static readonly int[] DelayTimeList = { 190, 200, 210, 220, 230};
        public static readonly List<Keys> DefaultAutoLiveKeys = new List<Keys>() { Keys.D8, Keys.Space};

        private StartingInfo mStartInfo;

        private readonly FloatSwitchButtonDispatcher mFloatSwitchDispatcher = new FloatSwitchButtonDispatcher();

        public void Start() {
            if (Enabled) {
                InfoCallback("已经开始啦。");
                return;
            }

            var si = new StartingInfo
            {
                IsStarted = true,
                Ts = DateTime.Now.Ticks
            };

            new Task(()=>{
                StartingInfo thisInfo = si;
                mStartInfo = si;
                Enabled = true;
                RandomKeyHelp status = new RandomKeyHelp();
                long whileCount = 0;

                while (thisInfo.IsStarted) {
                    var ts = DelayTimeList[new Random().Next(DelayTimeList.Length)];
                    InfoCallback("延迟休眠时间为：" + ts + "秒；");
                    if (whileCount++ == 0)
                    {
                        Thread.Sleep(ts * 200);
                    }
                    else 
                    {
                        Thread.Sleep(ts * 1000);
                    }

                    if (!thisInfo.IsStarted) {
                        InfoCallback("延迟完发现已经关闭啦！");
                        break;
                    }
                    var k = status.Next();
                    InfoCallback("延迟后按键：" + k);
                    if (!mFloatSwitchDispatcher.SwitchAndActive(true, k)) {
                        InfoCallback("wow进程发生变化。停止工作。");
                        Stop();
                        break;
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            if (!Enabled)
            {
                InfoCallback("已经停止啦。");
                return;
            }

            mStartInfo.IsStarted = false;
            Enabled = false;
            InfoCallback("停止工作！");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatWowBtn
{
    class StartingInfo {
        public bool IsStarted;
        public long UUID;
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

        public delegate void InfoCallbackDelegate(string s, bool append = false);
        public InfoCallbackDelegate InfoCallback { set; private get; }

        public delegate void OnStoppedDelegate();
        public OnStoppedDelegate OnStoppedCallback { set; private get; }

        public bool Enabled { set; get; } = false;

        private static readonly int[] DelayTimeList = { 180, 200, 220};
        public static readonly List<Keys> DefaultAutoLiveKeys = new List<Keys>() { Keys.D8, Keys.X, Keys.Space};

        private StartingInfo mStartInfo;

        private readonly FloatSwitchButtonDispatcher mFloatSwitchDispatcher = new FloatSwitchButtonDispatcher();

        public void Start() {
            if (Enabled) {
                InfoCallback("已经开始啦。");
                return;
            }

            var thisInfo = new StartingInfo
            {
                IsStarted = true,
                UUID = new Random().Next(1000)
            };
            mStartInfo = thisInfo;

            new Task(()=>{
                Enabled = true;
                RandomKeyHelp status = new RandomKeyHelp();
                long whileCount = 0;

                while (thisInfo.IsStarted) {
                    var ts = DelayTimeList[new Random().Next(DelayTimeList.Length)];
                    var k = status.Next();
                    InfoCallback(thisInfo.UUID + ":延迟" + ts + "秒，" + " 下次按键为" + k + ":");
                    if (whileCount++ == 0)
                    {
                        Thread.Sleep(ts * 200);
                    }
                    else 
                    {
                        Thread.Sleep(ts * 1000);
                    }

                    if (!thisInfo.IsStarted) { //不论如何都跳出
                        if (thisInfo == mStartInfo) { //不同才输出日志。因为已经换了线程执行了。
                            InfoCallback(thisInfo.UUID + ":延迟后发现已关闭!");
                        }
                        break;
                    }

                    InfoCallback(" OK!", true);
                    if (!mFloatSwitchDispatcher.SwitchAndActive(new KeysAndMouseEvent(k, true, false))) {
                        InfoCallback(thisInfo.UUID + "wow进程发生变化，停止！");
                        OnStoppedCallback?.Invoke();
                        break;
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            if (!Enabled)
            {
                InfoCallback(mStartInfo.UUID + ":已经停止啦。");
                return;
            }

            mStartInfo.IsStarted = false;
            InfoCallback(mStartInfo.UUID + ":停止工作！");
            mStartInfo = null;
            Enabled = false;
        }
    }
}

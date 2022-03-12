using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatWowBtn
{
    public partial class FloatWindowBtnForm : Form
    {
        private readonly List<CheckBox> CheckBoxes = new List<CheckBox>();
        private readonly FloatSwitchButtonDispatcher mActive = new FloatSwitchButtonDispatcher();
        private readonly AutoLiveManager mLiveManager = new AutoLiveManager();

        public FloatWindowBtnForm()
        {
            Context.Instance.MainForm = this;

            InitializeComponent();
            CheckBoxes.Add(Wow1Checkbox);
            CheckBoxes.Add(Wow2Checkbox);
            CheckBoxes.Add(Wow3Checkbox);
            CheckBoxes.Add(Wow4Checkbox);
            CheckBoxes.Add(Wow5Checkbox);
            CheckBoxes.Add(Wow6Checkbox);

            Context.Instance.AllWowPtrsChangedEvent += Instance_AllWowPtrsChangedEvent;

            mLiveManager.InfoCallback = (s) =>
            {
                ALog.D("callback: " + s);
                BeginInvoke(new Action(() => {
                    var text = AutoLiveTextBox.Text;
                    if (text.Length > 3000) {
                        text = text.Substring(text.Length / 2, text.Length - (text.Length/2));
                    }
                    text += "\r\n" + s;
                    AutoLiveTextBox.Text = text;
                }));
            };

            mActive.Scan();
        }

        private void Instance_AllWowPtrsChangedEvent()
        {
            
            ShowOnChecks(Context.Instance.AllWowPtrs);
            AllStateLabel.Visible = false;
        }

        private void ShowOnChecks(List<IntPtrIndex> wowStrs) {
            for (int i = 0; i < CheckBoxes.Count && i < wowStrs.Count; i++) {
                CheckBoxes[i].Visible = true;
                CheckBoxes[i].Text = wowStrs[i].Addr.ToString();
                CheckBoxes[i].Tag = wowStrs[i].Addr;
            }

            if (wowStrs.Count < CheckBoxes.Count) {
                for (int i = wowStrs.Count; i < CheckBoxes.Count; i++) {
                    CheckBoxes[i].Visible = false;
                    CheckBoxes[i].Text = "";
                    CheckBoxes[i].Tag = null;
                }
            }
        }


        #region 让窗口可以随意拖动

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Form1_MouseDown(this, e);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            WindowApi.ReleaseCapture();
            WindowApi.SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        #endregion

        private void AutoClick8Btn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Click8);
        }


        private void AutoClick2Btn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Click2);
        }

        private void AutoClick3Btn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Click3);
        }

        private void AutoClick4Btn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Click4);
        }

        private void AutoGuajiBtn_Click(object sender, EventArgs e)
        {
        }

        private void SwitchBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.NoClick);
        }

        private void SwitchAndActiveBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.ClickSpace);
        }

        public List<IntPtr> AllCheckedPtrs() {
            var ptrs = new List<IntPtr>();
            foreach (var checkbox in CheckBoxes) {
                if (checkbox.Checked && checkbox.Tag != null) {
                    ptrs.Add((IntPtr)checkbox.Tag);
                }
            }

            return ptrs;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            if (mIsBig)
            {
                Close();
            }
            else {
                SmallerBtn_Click(null, null);
            }
        }

        private void ScanAllWowListBtn_Click(object sender, EventArgs e)
        {
            mActive.Scan();
        }

        private void AutoLiveBtn_Click(object sender, EventArgs e)
        {
            if (mLiveManager.Enabled)
            {
                mLiveManager.Stop();
                AutoLiveBtn.Text = "自动保活";
            }
            else 
            {
                mLiveManager.Start();
                AutoLiveBtn.Text = "停止";
            }
        }

        private enum ClickMode {
            Click8,
            Click2,
            Click3,
            Click4,
            ClickSpace,
            ClickZ,
            NoClick,
            Scan,
        }

        private void ClickOn(ClickMode md) {
            if (!ClickNoDouble.Instance.IsAccept())
            {
                return;
            }

            bool isNoChanged = true;
            switch (md)
            {
                case ClickMode.Scan:
                    mActive.Scan();
                    break;
                case ClickMode.Click8:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.D8);
                    break;
                case ClickMode.Click2:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.D2);
                    break;
                case ClickMode.Click3:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.D3);
                    break;
                case ClickMode.Click4:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.D4);
                    break;
                case ClickMode.NoClick:
                    isNoChanged = mActive.SwitchAndActive(false, Keys.Space);
                    break;
                case ClickMode.ClickSpace:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.Space);
                    break;
                case ClickMode.ClickZ:
                    isNoChanged = mActive.SwitchAndActive(true, Keys.Z);
                    break;
                default:
                    break;
            }

            if (!isNoChanged) {
                AllStateLabel.Text = "wow进程变化，重新点击[扫描wow进程]";
                AllStateLabel.Visible = true;
            }
        }

        private bool mIsBig = true;
        private Size mBigSize = new Size(277, 320);
        private Size mSmallSize = new Size(100, 50);

        private void SmallerBtn_Click(object sender, EventArgs e)
        {
            if (mIsBig)
            {
                Size = mSmallSize;
                SpaceBtn.Visible = true;
                ZBtn.Visible = true;
                Smaller2Btn.Visible = true;
            }
            else {
                Size = mBigSize;
                SpaceBtn.Visible = false;
                ZBtn.Visible = false;
                Smaller2Btn.Visible = false;
            }

            mIsBig = !mIsBig;
        }

        private void SpaceBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.ClickSpace);
        }

        private void ZBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.ClickZ);
        }

        private void Smaller2Btn_Click(object sender, EventArgs e)
        {
            SmallerBtn_Click(null, null);
        }

        private void Current_Click(object sender, EventArgs e)
        {
            IntPtr intPtr = WindowApi.FindWindow(null, "魔兽世界");
            Current.Text = "当前的wow:" + intPtr;
        }
    }
}

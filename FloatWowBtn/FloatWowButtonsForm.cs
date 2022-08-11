using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FloatWowBtn
{
    public partial class FloatWindowBtnForm : Form
    {
        private readonly List<CheckBox> CheckBoxes = new List<CheckBox>();
        private readonly FloatSwitchButtonDispatcher mActive = new FloatSwitchButtonDispatcher();
        private readonly AutoLiveManager mLiveManager = new AutoLiveManager();

        private Point flowBoxWhenBigPoint, flowBoxWhenSmallPoint;
        private Size flowBoxBigSize, flowBoxSmallSize;

        private Size windowBigSize, windowSmallSize;

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

            mLiveManager.OnStoppedCallback = () =>
            {
                BeginInvoke(new Action(() => {
                    mLiveManager.Stop();
                }));
            };

            mLiveManager.InfoCallback = (s, append) =>
            {
                ALog.D("callback: " + s);
                BeginInvoke(new Action(() => {
                    var text = AutoLiveTextBox.Text;
                    if (text.Length > 3000) {
                        text = text.Substring(text.Length / 2, text.Length - (text.Length/2));
                    }
                    if (append)
                    {
                        text += s;
                    }
                    else {
                        text += "\r\n" + s;
                    }
                    
                    AutoLiveTextBox.Text = text;
                    AutoLiveTextBox.SelectionStart = AutoLiveTextBox.Text.Length;
                    AutoLiveTextBox.ScrollToCaret();
                }));
            };

            mActive.Scan();
            flowBoxWhenBigPoint = FlowBox.Location;
            flowBoxWhenSmallPoint = new Point(2, flowBoxWhenBigPoint.Y + 10);

            flowBoxBigSize = FlowBox.Size;
            flowBoxSmallSize = new Size(SmallerBtn.Width + 8, SmallerBtn.Height * 3 + 25);

            windowBigSize = Size;
            windowSmallSize = new Size(flowBoxSmallSize.Width + 5, flowBoxSmallSize.Height + 5);

            WindowSizeDispatcher.Instance.Register(toBig=>{
                if (!toBig)
                {
                    Size = windowSmallSize;
                    tabControl1.Visible = false;
                    SmallerBtn.Text = "放大";
                    FlowBox.Location = flowBoxWhenSmallPoint;
                    FlowBox.Size = flowBoxSmallSize;
                }
                else
                {
                    Size = windowBigSize;
                    tabControl1.Visible = true;
                    SmallerBtn.Text = "缩小";
                    FlowBox.Location = flowBoxWhenBigPoint;
                    FlowBox.Size = flowBoxBigSize;
                }
            });
        }

        private void Instance_AllWowPtrsChangedEvent()
        {
            
            ShowOnChecks(Context.Instance.AllWowPtrs);
        }

        private void ShowOnChecks(List<IntPtrIndex> wowStrs) {
            for (int i = 0; i < CheckBoxes.Count && i < wowStrs.Count; i++) {
                CheckBoxes[i].Visible = true;
                CheckBoxes[i].Text =  "0x" + wowStrs[i].Addr.ToString("X8");
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

        private void SwitchBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.NoClick, Keys.D0);
        }

        private void SwitchAndActiveBtn_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.ClickSpace, Keys.D0);
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
            Close();
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
                mActive.Scan();
                mLiveManager.Start();
                AutoLiveBtn.Text = "停止";
            }
        }

        private enum ClickMode {
            ClickSpace,
            NoClick,
            Scan,
            Key,
            Mouse
        }

        private void ClickOn(ClickMode md, Keys key, WindowApi.MouseType[] events = null) {
            if (!ClickNoDouble.Instance.IsAccept())
            {
                return;
            }

            bool isNoChanged = true;
            bool toFront = Context.Instance.IsStillGobackToFrontChecked;
            switch (md)
            {
                case ClickMode.Scan:
                    mActive.Scan();
                    break;
                case ClickMode.Mouse:
                    isNoChanged = mActive.SwitchAndActive(new KeysAndMouseEvent(events, true, toFront));
                    break;
                case ClickMode.Key:
                    isNoChanged = mActive.SwitchAndActive(new KeysAndMouseEvent(key, true, toFront));
                    break;
                case ClickMode.NoClick:
                    isNoChanged = mActive.SwitchAndActive(new KeysAndMouseEvent(Keys.Space, false, toFront));
                    break;
                case ClickMode.ClickSpace:
                    isNoChanged = mActive.SwitchAndActive(new KeysAndMouseEvent(Keys.Space, true, toFront));
                    break;
                default:
                    break;
            }

            if (!isNoChanged) {
                ALog.D("is no changed = false");
            }
        }

        private void SmallerBtn_Click(object sender, EventArgs e)
        {
            WindowSizeDispatcher.Instance.SwitchChange();
        }

        private void FloatWindowBtnForm_KeyDown(object sender, KeyEventArgs e)
        {
            ALog.D("on key FloatWindowBtnForm_KeyDown " + e.KeyCode);
        }

        private void FloatWindowBtnForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            ALog.D("on key FloatWindowBtnForm_KeyPress " + e.KeyChar);
        }

        private void FloatWindowBtnForm_KeyUp(object sender, KeyEventArgs e)
        {
            ALog.D("on key FloatWindowBtnForm_KeyUp " + e.KeyCode);
        }

        private void SingleBtn1_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Key, Keys.D1);
        }

        private void SingleBtn2_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Key, Keys.D2);
        }

        private void StillGobackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Context.Instance.IsStillGobackToFrontChecked = StillGobackCheckBox.Checked;
        }

        private void SingleBtn3_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Key, Keys.D3);
        }

        private void SingleBtn4__Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Key, Keys.D4);
        }

        private void SingleBtn4_Click(object sender, EventArgs e)
        {
            ClickOn(ClickMode.Key, Keys.D8);
        }

        //private void SpaceBtn_Click(object sender, EventArgs e)
        //{
        //    ClickOn(ClickMode.ClickSpace);
        //}

        //private void ZBtn_Click(object sender, EventArgs e)
        //{
        //    ClickOn(ClickMode.ClickZ);
        //}
    }
}

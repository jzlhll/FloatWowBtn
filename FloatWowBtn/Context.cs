using System;
using System.Collections.Generic;

namespace FloatWowBtn
{
    public class IntPtrAndBool
    {
        public IntPtr Ptr;
        public bool Check;
    }

    public sealed class Context
    {
        public static readonly Context Instance = new Context();

        public const bool NotSelectStillToFront = true;

        private bool _TopMostAlways;
        public bool TopMostAlways
        {
            set
            {
                _TopMostAlways = value;
                MyTopMostAlwaysEvent?.Invoke(value);
            }

            get
            {
                return _TopMostAlways;
            }
        }
        public delegate void TopMostAlwaysEvent(bool alwaysTop);
        public event TopMostAlwaysEvent MyTopMostAlwaysEvent;

        public FloatWindowBtnForm MainForm { set; private get; }

        private List<IntPtr> _CachedWowPtrs = new List<IntPtr>();

        private List<IntPtr> _CurrentSelectedPtrs = new List<IntPtr>();
        private List<IntPtrAndBool> _CurrentAllPtrs = new List<IntPtrAndBool>();

        public List<IntPtrAndBool> CurrentAllPtrs { get { return _CurrentAllPtrs; } }
        public List<IntPtr> CurrentSelectedPtrs { get { return _CurrentSelectedPtrs; } }

        public List<IntPtr> CachedAllWowPtrs {
            private set {
                _CachedWowPtrs = value;
                AllWowPtrsChangedEvent?.Invoke();
            }

            get
            {
                return _CachedWowPtrs;
            }
        }

        public delegate void AllWowPtrsChanged();
        public event AllWowPtrsChanged AllWowPtrsChangedEvent;

        public void SetAllWowPtrsAndNotified(List<IntPtr> allWowPtrs) {
            CachedAllWowPtrs = allWowPtrs;
        }

        public void reGetSelectedAndAll() {
            _CurrentAllPtrs = MainForm.AllPtrs();
            _CurrentSelectedPtrs = MainForm.AllCheckedPtrs();
        }
    }
}

using System;
using System.Collections.Generic;

namespace FloatWowBtn
{
    public sealed class Context
    {
        public static readonly Context Instance = new Context();

        public FloatWindowBtnForm MainForm { set; private get; }

        private List<IntPtrIndex> _AllWowPtrs = new List<IntPtrIndex>();
        public List<IntPtrIndex> AllWowPtrs {
            set {
                _AllWowPtrs = value;
                AllWowPtrsChangedEvent?.Invoke();
            }

            get
            {
                return _AllWowPtrs;
            }
        }

        public delegate void AllWowPtrsChanged();
        public event AllWowPtrsChanged AllWowPtrsChangedEvent;

        public List<IntPtr> CurrentSelectedPtrs() {
            return MainForm.AllCheckedPtrs();
        }
    }
}

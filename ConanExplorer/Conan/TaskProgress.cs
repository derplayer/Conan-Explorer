using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan
{
    public class TaskProgress
    {
        public event EventHandler<int> ProgressChanged;
        public event EventHandler TaskStarted;
        public event EventHandler TaskDone;

        private int _progress;
        private bool _started;
        private bool _done;

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnProgressChanged(value);
            }
        }

        public bool Started
        {
            get { return _started; }
            set
            {
                _started = value;
                if (value) OnTaskStarted();
            }
        }
        public bool Done
        {
            get { return _done; }
            set
            {
                _done = value;
                if (value) OnTaskDone();
            }
        }

        protected virtual void OnProgressChanged(int progress)
        {
            ProgressChanged?.Invoke(this, progress);
        }
        protected virtual void OnTaskStarted()
        {
            TaskStarted?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnTaskDone()
        {
            TaskDone?.Invoke(this, EventArgs.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan.Filetypes;

namespace ConanExplorer.Controls
{
    public partial class DynamicControl : UserControl
    {
        private TextControl textControl;
        private LZBControl lzbControl;
        private TIMControl timControl;
        private PBControl pbControl;
        private BGControl bgControl;

        public bool EnabledLZB { get; set; } = true;
        public bool EnabledTIM { get; set; } = true;
        public bool EnabledPB { get; set; } = true;
        public bool EnabledBG { get; set; } = true;

        public DynamicControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            SuspendLayout();

            textControl = new TextControl();

            textControl.Dock = DockStyle.Fill;
            textControl.Location = new Point(0, 0);
            textControl.Name = "textControl";
            textControl.Size = new Size(300, 300);
            textControl.TabIndex = 0;
            textControl.Visible = false;

            Controls.Add(textControl);


            if (EnabledLZB)
            {
                lzbControl = new LZBControl();

                lzbControl.Dock = DockStyle.Fill;
                lzbControl.Location = new Point(0, 0);
                lzbControl.Name = "lzbControl";
                lzbControl.Size = new Size(300, 300);
                lzbControl.TabIndex = 0;
                lzbControl.Visible = false;

                Controls.Add(lzbControl);
            }
            if (EnabledTIM)
            {
                timControl = new TIMControl();

                timControl.Dock = DockStyle.Fill;
                timControl.Location = new Point(0, 0);
                timControl.Name = "timControl";
                timControl.Size = new Size(300, 300);
                timControl.TabIndex = 1;
                timControl.Visible = false;

                Controls.Add(timControl);
            }
            if (EnabledPB)
            {
                pbControl = new PBControl();

                pbControl.Dock = DockStyle.Fill;
                pbControl.Location = new Point(0, 0);
                pbControl.Name = "pbControl";
                pbControl.Size = new Size(300, 300);
                pbControl.TabIndex = 1;
                pbControl.Visible = false;

                Controls.Add(pbControl);
            }
            if (EnabledBG)
            {
                bgControl = new BGControl();

                bgControl.Dock = DockStyle.Fill;
                bgControl.Location = new Point(0, 0);
                bgControl.Name = "bgControl";
                bgControl.Size = new Size(300, 300);
                bgControl.TabIndex = 1;
                bgControl.Visible = false;

                Controls.Add(bgControl);
            }
            ResumeLayout();
        }

        public void Update(BaseFile baseFile)
        {
            if (baseFile == null)
            {
                if (EnabledLZB) lzbControl.Visible = false;
                if (EnabledTIM) timControl.Visible = false;
                if (EnabledPB) pbControl.Visible = false;
                if (EnabledBG) bgControl.Visible = false;
                textControl.Visible = false;
                return;
            }
            if (baseFile.GetType() == typeof(TIMFile) && EnabledTIM)
            {
                TIMFile timFile = (TIMFile)baseFile;
                timControl.Update(timFile);
                timControl.Visible = true;
                if (EnabledLZB) lzbControl.Visible = false;
                if (EnabledPB) pbControl.Visible = false;
                if (EnabledBG) bgControl.Visible = false;
                textControl.Visible = false;
                return;
            }
            if (baseFile.GetType() == typeof(LZBFile) && EnabledLZB)
            {
                LZBFile lzbFile = (LZBFile)baseFile;
                lzbControl.Update(lzbFile);
                lzbControl.Visible = true;
                if (EnabledTIM) timControl.Visible = false;
                if (EnabledPB) pbControl.Visible = false;
                if (EnabledBG) bgControl.Visible = false;
                textControl.Visible = false;
                return;
            }
            if (baseFile.GetType() == typeof(PBFile) && EnabledPB)
            {
                PBFile pbFile = (PBFile)baseFile;
                pbControl.Update(pbFile);
                pbControl.Visible = true;
                if (EnabledTIM) timControl.Visible = false;
                if (EnabledLZB) lzbControl.Visible = false;
                if (EnabledBG) bgControl.Visible = false;
                textControl.Visible = false;
                return;
            }
            if (baseFile.GetType() == typeof(BGFile) && EnabledBG)
            {
                BGFile bgFile = (BGFile)baseFile;
                bgControl.Update(bgFile);
                bgControl.Visible = true;
                if (EnabledTIM) timControl.Visible = false;
                if (EnabledLZB) lzbControl.Visible = false;
                if (EnabledPB) pbControl.Visible = false;
                textControl.Visible = false;
                return;
            }

            textControl.Update(baseFile);
            textControl.Visible = true;
            if (EnabledLZB) lzbControl.Visible = false;
            if (EnabledTIM) timControl.Visible = false;
            if (EnabledPB) pbControl.Visible = false;
            if (EnabledBG) bgControl.Visible = false;
        }
    }
}

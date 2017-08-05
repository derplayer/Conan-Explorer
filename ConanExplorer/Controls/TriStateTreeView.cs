

namespace ConanExplorer.Controls
{

    public class TriStateTreeView : System.Windows.Forms.TreeView
    {
        public enum CheckedState : int { UnInitialised = -1, UnChecked, Checked, Mixed };
        int IgnoreClickAction = 0;

        public enum TriStateStyles : int { Standard = 0, Installer };

        private TriStateStyles TriStateStyle = TriStateStyles.Standard;

        [System.ComponentModel.Category("Tri-State Tree View")]
        [System.ComponentModel.DisplayName("Style")]
        [System.ComponentModel.Description("Style of the Tri-State Tree View")]
        public TriStateStyles TriStateStyleProperty
        {
            get { return TriStateStyle; }
            set { TriStateStyle = value; }
        }

        public TriStateTreeView() : base()
        {
            StateImageList = new System.Windows.Forms.ImageList();

            for (int i = 0; i < 3; i++)
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(16, 16);
                System.Drawing.Graphics chkGraphics = System.Drawing.Graphics.FromImage(bmp);
                switch (i)
                {
                    case 0:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                        break;
                    case 1:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
                        break;
                    case 2:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
                        break;
                }

                StateImageList.Images.Add(bmp);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            CheckBoxes = false;

            IgnoreClickAction++;
            UpdateChildState(this.Nodes, (int)CheckedState.UnChecked, false, true);
            IgnoreClickAction--;
        }

        protected override void OnAfterCheck(System.Windows.Forms.TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);

            if (IgnoreClickAction > 0)
            {
                return;
            }

            IgnoreClickAction++;

            System.Windows.Forms.TreeNode tn = e.Node;
            tn.StateImageIndex = tn.Checked ? (int)CheckedState.Checked : (int)CheckedState.UnChecked;

            UpdateChildState(e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, false);

            UpdateParentState(e.Node.Parent);

            IgnoreClickAction--;
        }

        protected override void OnAfterExpand(System.Windows.Forms.TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);

            IgnoreClickAction++;
            UpdateChildState(e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, true);
            IgnoreClickAction--;
        }

        protected void UpdateChildState(System.Windows.Forms.TreeNodeCollection Nodes, int StateImageIndex, bool Checked, bool ChangeUninitialisedNodesOnly)
        {
            foreach (System.Windows.Forms.TreeNode tnChild in Nodes)
            {
                if (!ChangeUninitialisedNodesOnly || tnChild.StateImageIndex == -1)
                {
                    tnChild.StateImageIndex = StateImageIndex;
                    tnChild.Checked = Checked;

                    if (tnChild.Nodes.Count > 0)
                    {
                        UpdateChildState(tnChild.Nodes, StateImageIndex, Checked, ChangeUninitialisedNodesOnly);
                    }
                }
            }
        }

        protected void UpdateParentState(System.Windows.Forms.TreeNode tn)
        {
            if (tn == null)
                return;

            int OrigStateImageIndex = tn.StateImageIndex;

            int UnCheckedNodes = 0, CheckedNodes = 0, MixedNodes = 0;

            foreach (System.Windows.Forms.TreeNode tnChild in tn.Nodes)
            {
                if (tnChild.StateImageIndex == (int)CheckedState.Checked)
                    CheckedNodes++;
                else if (tnChild.StateImageIndex == (int)CheckedState.Mixed)
                {
                    MixedNodes++;
                    break;
                }
                else
                    UnCheckedNodes++;
            }

            if (TriStateStyle == TriStateStyles.Installer)
            {
                if (MixedNodes == 0)
                {
                    if (UnCheckedNodes == 0)
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
            }

            if (MixedNodes > 0)
            {
                tn.StateImageIndex = (int)CheckedState.Mixed;
            }
            else if (CheckedNodes > 0 && UnCheckedNodes == 0)
            {
                if (tn.Checked)
                    tn.StateImageIndex = (int)CheckedState.Checked;
                else
                    tn.StateImageIndex = (int)CheckedState.Mixed;
            }
            else if (CheckedNodes > 0)
            {
                tn.StateImageIndex = (int)CheckedState.Mixed;
            }
            else
            {
                if (tn.Checked)
                    tn.StateImageIndex = (int)CheckedState.Mixed;
                else
                    tn.StateImageIndex = (int)CheckedState.UnChecked;
            }

            if (OrigStateImageIndex != tn.StateImageIndex && tn.Parent != null)
            {
                UpdateParentState(tn.Parent);
            }
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == System.Windows.Forms.Keys.Space)
            {
                SelectedNode.Checked = !SelectedNode.Checked;
            }
        }

        protected override void OnNodeMouseClick(System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Right) return;
            System.Windows.Forms.TreeViewHitTestInfo info = HitTest(e.X, e.Y);
            if (info == null || info.Location != System.Windows.Forms.TreeViewHitTestLocations.StateImage)
            {
                return;
            }

            System.Windows.Forms.TreeNode tn = e.Node;
            tn.Checked = !tn.Checked;
        }
    }
}

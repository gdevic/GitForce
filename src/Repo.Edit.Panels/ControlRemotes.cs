using System.Windows.Forms;

namespace GitForce.Repo.Edit.Panels
{
    public partial class ControlRemotes : UserControl, IRepoSettings
    {
        private ClassRepo currentRepo;

        public ControlRemotes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        public void Init(ClassRepo repo)
        {
            currentRepo = repo;

            currentRepo.Remotes.Refresh(currentRepo);

            listRemotes.BeginUpdate();
            listRemotes.Items.Clear();

            // Get the list of names from remotes class and iteratively add them to the listbox
            foreach (var r in currentRepo.Remotes.GetListNames())
                listRemotes.Items.Add(r);

            listRemotes.EndUpdate();
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
        }

        /// <summary>
        /// User clicked on the edit remote repos, open that dialog.
        /// </summary>
        private void BtEditClick(object sender, System.EventArgs e)
        {
            FormRemoteEdit remoteEdit = new FormRemoteEdit(currentRepo);
            if (remoteEdit.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }
    }
}

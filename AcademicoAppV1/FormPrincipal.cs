using ReaLTaiizor.Forms;

namespace AcademicoAppV1
{
    public partial class FormPrincipal : MaterialForm
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void alunosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAluno formAluno = new FormAluno();
            // Definindo FormPrincipal como pai do FormAluno
            formAluno.MdiParent = this;
            // Apresenta formulário;
            formAluno.Show();
        }
    }
}

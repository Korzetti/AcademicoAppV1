using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;

namespace AcademicoAppV1
{
    public partial class FormAluno : MaterialForm
    {
        #region Variáveis
        string alunosFileName = "alunos.txt";
        bool isEditMode = false;
        int indexSelecionado = 0;
        #endregion

        #region Métodos
        public FormAluno()
        {
            InitializeComponent();
        }

        private void LimpaCampos()
        {
            isEditMode = false;
            foreach (var control in tabPageCadastro.Controls)
            {
                if (control is MaterialMaskedTextBox textBox)
                {
                    textBox.Clear();
                }
                if (control is MaterialTextBoxEdit textBoxEdit)
                {
                    textBoxEdit.Clear();
                }
                if (control is MaterialComboBox comboBox)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }

        private void Salvar()
        {
            var line = $"{txtMatricula.Text};" +
                       $"{txtDataNascimento.Text};" +
                       $"{txtNome.Text};" +
                       $"{txtEndereco.Text};" +
                       $"{cboEstado.Text};" +
                       $"{txtBairro.Text};" +
                       $"{txtCidade.Text};" +
                       $"{txtSenha.Text}";
            if (!isEditMode)
            {
                using (StreamWriter sw = new StreamWriter(alunosFileName, true))
                {
                    sw.WriteLine(line);
                }
            }
            else
            {
                var fileLines = File.ReadAllLines(alunosFileName).ToList();
                fileLines[indexSelecionado] = line;
                File.WriteAllLines(alunosFileName, fileLines);
            }
        }

        private bool ValidaFormulario()
        {
            var erro = "";
            if (string.IsNullOrEmpty(txtMatricula.Text))
            {
                erro += "Matrícula deve ser informada!\n";
            }
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                erro += "Nome deve ser informado!\n";
            }
            if (string.IsNullOrEmpty(txtDataNascimento.Text))
            {
                erro += "Data de Nascimento deve ser informada!\n";
            }
            if (!DateTime.TryParse(txtDataNascimento.Text, out _))
            {
                erro += "Data de nascimento inválida!\n";
            }
            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                erro += "Endereço deve ser informado!\n";
            }
            if (string.IsNullOrEmpty(txtBairro.Text))
            {
                erro += "Bairro deve ser informado!\n";
            }
            if (string.IsNullOrEmpty(txtCidade.Text))
            {
                erro += "Cidade deve ser informada!\n";
            }
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                erro += "Senha deve ser informada!\n";
            }
            if (!string.IsNullOrEmpty(erro))
            {
                MessageBox.Show(erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CarregaListView()
        {
            Cursor.Current = Cursors.WaitCursor;
            listViewConsulta.Columns.Clear();
            listViewConsulta.Items.Clear();
            listViewConsulta.Columns.Add("Matricula");
            listViewConsulta.Columns.Add("Data Nasc.");
            listViewConsulta.Columns.Add("Nome");
            listViewConsulta.Columns.Add("Endereço");
            listViewConsulta.Columns.Add("Estado");
            listViewConsulta.Columns.Add("Bairro");
            listViewConsulta.Columns.Add("Cidade");
            listViewConsulta.Columns.Add("Senha");
            var fileLines = File.ReadAllLines(alunosFileName);

            foreach (var line in fileLines)
            {
                var campos = line.Split(";");
                listViewConsulta.Items.Add(new ListViewItem(campos));
            }
            listViewConsulta.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Cursor.Current = Cursors.Default;
        }

        private void Editar()
        {
            if (listViewConsulta.SelectedIndices.Count > 0)
            {
                isEditMode = true;
                indexSelecionado = listViewConsulta.SelectedItems[0].Index;
                var item = listViewConsulta.SelectedItems[0];
                txtMatricula.Text = item.SubItems[0].Text;
                txtDataNascimento.Text = item.SubItems[1].Text;
                txtNome.Text = item.SubItems[2].Text;
                txtEndereco.Text = item.SubItems[3].Text;
                cboEstado.Text = item.SubItems[4].Text;
                txtBairro.Text = item.SubItems[5].Text;
                txtCidade.Text = item.SubItems[6].Text;
                txtSenha.Text = item.SubItems[7].Text;
                tabControlCadastro.SelectedIndex = 0;
                txtMatricula.Focus();
            }
            else
            {
                MessageBox.Show("Selecione algum registro!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Deletar()
        {
            if(listViewConsulta.SelectedItems.Count > 0)
            {
                if(MessageBox.Show("Deseja realmente deletar?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    indexSelecionado = listViewConsulta.SelectedItems[0].Index;
                    var fileLines = File.ReadAllLines(alunosFileName).ToList();
                    fileLines.RemoveAt(indexSelecionado);
                    File.WriteAllLines(alunosFileName, fileLines);
                }
            }
            
        }

        #endregion

        #region Eventos

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            // Mudando para página cadastro
            tabControlCadastro.SelectedIndex = 0;
            // Campo matrícula recebe o foco do teclado
            txtMatricula.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Informações não salvas serão perdidas!\n" + "Deseja realmente cancelar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpaCampos();
                // Mudando para página consulta
                tabControlCadastro.SelectedIndex = 1;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaFormulario())
            {
                Salvar();
                LimpaCampos();
                tabControlCadastro.SelectedIndex = 1;
            }
        }

        private void tabPageConsulta_Enter(object sender, EventArgs e)
        {
            CarregaListView();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void listViewConsulta_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Editar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            Deletar();
            CarregaListView();
        }

        #endregion
    }
}

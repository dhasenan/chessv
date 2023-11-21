using System;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class ConfirmationForm : Form
  {
    public ConfirmationForm()
    {
      InitializeComponent();
    }

    private void ConfirmationForm_Load(object sender, EventArgs e)
    {
      lblConfirmationMessage.Text = ConfirmationMessage;
    }

    public string ConfirmationMessage;
  }
}

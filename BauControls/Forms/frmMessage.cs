using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.Forms
{
	/// <summary>
	///		Formulario para introducción de una cadena
	/// </summary>
	public partial class frmMessage : Form
	{
		public frmMessage()
		{	InitializeComponent();
		}
		
		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message
		{ get { return lblMessage.Text; }
			set { lblMessage.Text = value; }
		}

		/// <summary>
		///		Texto introducido por el usuario
		/// </summary>
		public string InputText
		{ get { return txtInput.Text; }
			set { txtInput.Text = value; }
		}
	}
}

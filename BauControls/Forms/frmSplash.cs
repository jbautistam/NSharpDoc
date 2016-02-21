using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.Forms
{
	/// <summary>
	///		Formulario de introducción al programa
	/// </summary>
	public partial class frmSplash : Form
	{ // Delegados
			public delegate void SetOpacityHandler(double dblOpacity);
		// Variables privadas
			private int intDelayBetweenFade = 100;
			private int intDelayShow = 3000;

		public frmSplash()
		{	// Inicializa el formulario
				InitializeComponent();
			// Limpia el mensaje
				lblMessage.Text = "";
		}

		/// <summary>
		///		Modifica la opacidad del formulario
		/// </summary>
		private void SetOpacity(double dblOpacity)
		{	// Pasa el formulario al frente
				BringToFront();
			// Cambia la opacidad
				Opacity = dblOpacity;
			 // Manejamos eventos
				Application.DoEvents();
			// Cerramos si ha llegado al final
				if (dblOpacity == 0.0F)
					Close();
		}
 
		/// <summary>
		///		Muestra el formulario realizando el proceso de fadeIn y fadeOut
		/// </summary>
		public new void Show()
		{ // Muestra el formulario
				base.Show();
			// Cambia la transparencia del formulario
				FadeIn();
			// Espera hasta que esté completamente opaco
				while (Opacity < 1.0)
					System.Threading.Thread.Sleep(DelayBetweenFade);
			// Espera un momento
				System.Threading.Thread.Sleep(DelayShow);
			// Oculta el formulario
				FadeOut();
		}
		
		/// <summary>
		///		Muestra el formulario
		/// </summary>
		public void FadeIn()
		{	Opacity = 0.0F;
			(new System.Threading.ThreadStart(DoFadeIn)).Invoke();
		}
		 
		/// <summary>
		///		Oculta el formulario
		/// </summary>
		public void FadeOut()
		{ new System.Threading.ThreadStart(DoFadeOut).Invoke();
		}
		 
		/// <summary>
		///		Realiza el proceso de mostrar el formulario cambiando la transparencia
		/// </summary>
		private void DoFadeIn()
		{ for (double dblOpacity = 0.1; dblOpacity <= 1; dblOpacity += 0.1)
				{	if (InvokeRequired)
						Invoke(new SetOpacityHandler(SetOpacity), dblOpacity);
					else
						SetOpacity(dblOpacity);
					System.Threading.Thread.Sleep(DelayBetweenFade);
				}
			 Invoke(new SetOpacityHandler(SetOpacity), 1);
		}

		/// <summary>
		///		Realiza el proceso de ocultar el formulario cambiando la transparencia
		/// </summary>
		private void DoFadeOut()
		{ for (double dblOpacity = 1; dblOpacity > 0; dblOpacity -= 0.1)
				{	if (InvokeRequired)
						Invoke(new SetOpacityHandler(SetOpacity), dblOpacity);
					else
						SetOpacity(dblOpacity);
					System.Threading.Thread.Sleep(DelayBetweenFade);
				}
			 Invoke(new SetOpacityHandler(SetOpacity), 0);
		}

		public Image BackGroundImage
		{ get { return imgImage.Image; }
			set 
				{ imgImage.Image = value; 
					Width = imgImage.Image.Width;
					Height = imgImage.Image.Height;
				}
		}		

		public string Message
		{ get { return lblMessage.Text; } 
			set { lblMessage.Text = value; }
		}
		
		public int DelayBetweenFade
		{ get { return intDelayBetweenFade; }
			set { intDelayBetweenFade = value; }
		}
		
		public int DelayShow
		{ get { return intDelayShow; }
			set { intDelayShow = value; }
		}
	}
}
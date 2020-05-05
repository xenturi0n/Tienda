using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;
using Tienda.Common.Validadores;
using Tienda.BLL;

namespace TIenda.UI.WPF
{
    

    public partial class Login : Window
    {
        IUsuariosManager manager;

        public Login()
        {
            InitializeComponent();
            manager = FabricManager.UsuariosManager(FabricManager.OrigenesDeDatos.SQLServer);
        }

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            Usuarios user = manager.Login(txtUsuario.Text, txtPassword.Password);

            if (user != null)
            {
                MessageBox.Show($"Bienvenido {user.Nombres}", "Tienda", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(manager.Error, "Tienda", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

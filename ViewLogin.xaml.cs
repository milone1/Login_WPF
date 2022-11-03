using Infomatica.Front.Almacen.Controls;
using Infomatica.Front.Almacen.Model;
using Infomatica.Front.Almacen.ViewModel;
using KeyPad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace Infomatica.Front.Almacen.View
{
    public partial class ViewLogin : Window
    {
        SqlData SqlData = new SqlData();
        private bool _altf4 = false;
        public ViewLogin()
        {
            InitializeComponent();
        }
        private void txtusuario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
            {
                txtUser.Text = txtUser.Text.ToUpper();
                txtPass.Password = "";
                txtPass.Focus();
            }
        }

        private void txtPass_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdLogin.Focus();
            }
        }
        private void SaveUsuario(string Usu)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Usuario"].Value = Usu.ToString().Trim(); ;
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch
            {

            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            login();
        }

        void login() 
        {
            try
            {
                clsUsuario _user = new clsUsuario();

                var pass = _user.Encapsula(txtPass.Password);
                // var ResultadoUser = "";
                var ResultadoUser = SqlData.ValidaUsuario(ConfigurationManager.ConnectionStrings["Almacen"].ToString(), txtUser.Text, pass);
                if (ResultadoUser != "")
                {
                    VariablesGui.Usuario = txtUser.Text;
                    this.Hide();
                    this.Close();
                }
                else
                {
                    ViewMensaje noty = new ViewMensaje("0", "Mensaje Inforest | Seguridad", "Contraseña o Usuario incorrecto!!!");
                    noty.Topmost = true;
                    noty.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ViewMensaje noty = new ViewMensaje("0", "Mensaje Inforest | Seguridad", ex.ToString());
                noty.Topmost = true;
                noty.ShowDialog();
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.Key == (int)Key.Enter)
            {
                txtPass.Focus();
            }

            if (e.SystemKey == Key.F4 && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
            {
                _altf4 = true;
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((int)e.Key == (int)Key.Enter)
                {
                    login();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (e.SystemKey == Key.F4 && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
            {
                _altf4 = true;
            }
        }

        private void btnKeyboarUser_Click(object sender, RoutedEventArgs e)
        {
            View.ViewKeyboard frmteclado = new View.ViewKeyboard();
            frmteclado.Topmost = true;
            frmteclado.ShowDialog();
            if (VariablesGui.wEnter == true)
            {
                txtUser.Text = VariablesGui.sDescrip;
            }
        }

        private void btnKeyboarPass_Click(object sender, RoutedEventArgs e)
        {
            View.ViewKeyboard frmteclado = new View.ViewKeyboard();
            frmteclado.Topmost = true;
            frmteclado.ShowDialog();
            if (VariablesGui.wEnter == true)
            {
                txtPass.Password = VariablesGui.sDescrip;
                login();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F4 && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
            {
                _altf4 = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_altf4)
                e.Cancel = true;
            _altf4 = false;
        }
    }
}

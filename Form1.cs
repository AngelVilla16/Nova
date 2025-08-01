using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Nova
{
    public partial class Form1 : Form
    {
        string ruta;
        string cadena_conexion;
        public Form1()
        {
            InitializeComponent();
             ruta = Path.Combine(Application.StartupPath, @"Data\NovaDb.mdf");
            cadena_conexion = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={ruta};Integrated Security=True;Connect Timeout=30";


        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {//Variables para traer los datos de los textbox
            string usuario;
            string contraseña;
            usuario = txtUsuario.Text;
            contraseña = txtContraseña.Text;
            //Conexion a la base de datos
            using(SqlConnection con = new SqlConnection(cadena_conexion))
            {
                //Importante usar un try catch para tolerancia a errores de conexion
                try
                {
                    con.Open();
                    string consulta = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña";

                    using(SqlCommand cmd = new SqlCommand(consulta, con))
                    {
                        //Añadir parametros por nombre
                        //el nombre del parametro debe coincidr con la consulta
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Contraseña", contraseña);
                        // ExecuteScalar devuelve un 'object'. Para COUNT(*), será
                        //un 'long'(0 o más).
                        // Es más seguro usar Convert.ToInt32() o un cast a 'long'
                        //primero.
                        object resultado = cmd.ExecuteScalar();
                        int cuenta = 0;
                        if (resultado != null && resultado != DBNull.Value)
                        {
                            cuenta = Convert.ToInt32(resultado);

                        }
                        if (cuenta > 0)
                        {
                            MessageBox.Show("Inicio de sesión exitoso " + MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Usuario y/o contraseña incorrectos " + MessageBoxButtons.OK);
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Conexion fallida a la base de datos " + ex.Message + MessageBoxIcon.Error);
                }
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtUsuario.Clear();
            txtContraseña.Clear();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}

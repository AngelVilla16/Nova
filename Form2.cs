using Microsoft.Data.SqlClient;
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

namespace Nova
{
    public partial class Form2 : Form
    {
        string ruta;
        string cadena_conexion;
        public Form2()
        {
            InitializeComponent();
            ruta = Path.Combine(Application.StartupPath, @"Data\NovaDb.mdf");
            cadena_conexion = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={ruta};Integrated Security=True;Connect Timeout=30";
        }

        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            string usuario; string contraseña;
            usuario = txtUsuario.Text;
            contraseña = txtContraseña.Text;

            if(usuario == " " || contraseña ==" ")
            {
                MessageBox.Show("Por favor llene los campos solicitados.");
                return;
            }
            using (SqlConnection con = new SqlConnection(cadena_conexion))
            {
                try
                {
                    con.Open();
                    string consulta = "SELECT COUNT (*) FROM Usuarios WHERE Usuario = @Usuario";
                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    int existe = (int)cmd.ExecuteScalar();
                    if (existe > 0)
                    {
                        MessageBox.Show("El usuario ya esta registrado. ");
                        return;
                    }
                    else
                    {
                        string insertar = "INSERT INTO Usuarios (Usuario, Contraseña) VALUES (@Usuario, @Contraseña)";
                        SqlCommand ins = new SqlCommand (insertar, con);
                        ins.Parameters.AddWithValue("@Usuario", usuario);
                        ins.Parameters.AddWithValue("@Contraseña", contraseña);
                        ins.ExecuteNonQuery();
                        MessageBox.Show("Usuario agregado con exito.");
                        this.Close();
                        return;
                    }
                  

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al acceder a la base de datos " + ex.Message);
                }
            }
            
        }
    }
}

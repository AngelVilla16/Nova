using Microsoft.Identity.Client;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nova
{
    public partial class Form4 : Form
    {
        string ruta;
        string cadena_conexion;
        public event Action AlumnoAgregado;
        public Form4()
        {
            InitializeComponent();
             ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NovaDb.mdf");
            cadena_conexion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\NovaDb.mdf;Integrated Security=True;";



        }
        public class Alumno
        {
            public string Taller { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Direccion { get; set; }   
            public string Telefono { get; set; }
            public string Estado { get; set; }
            public DateTime Fecha { get; set; }
            public string Lugar { get; set; }
            public string Seguro { get; set; }
            public string TipoSeguro { get; set; }

        }
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Alumno alumno = new Alumno();
            alumno.Taller = comboBox2.SelectedItem.ToString();
            alumno.Nombre = txtNombre.Text;
            alumno.Apellidos = txtApellidos.Text;
            alumno.Direccion = txtDireccion.Text;
            alumno.Telefono = txtTelefono.Text;
            alumno.Fecha = monthCalendar1.SelectionStart;
            alumno.Lugar=txtLugar.Text;
            alumno.Seguro = txtSeguro.Text;
            alumno.TipoSeguro = comboBox1.SelectedItem.ToString();
            //Segun el estado civil seleccionado
            if (rbCasado.Checked)
            {
                alumno.Estado = "Casado";
            }
            else if(rbSoltero.Checked)
            {
                alumno.Estado = "Soltero";
            }
            else if (rbDivorciado.Checked)
            {
                alumno.Estado = "Divorciado";
            }
            else if (rbViudo.Checked)
            {
                alumno.Estado = "Viudo";
            }

            //Usando la base de datos
            using(SqlConnection con = new SqlConnection(cadena_conexion))
            {
                try
                {
                    con.Open();
                    string insertar = "INSERT INTO Alumnos (Taller, Nombre, Apellido, Direccion, Telefono, Estado_Civil, Fecha_Nacimiento, Lugar_Nacimiento, Seguro_Medico, Tipo_Seguro) VALUES (@Taller, @Nombre, @Apellido, @Direccion, @Telefono, @Estado_Civil, @Fecha_Nacimiento, @Lugar_Nacimiento, @Seguro_Medico, @Tipo_Seguro) ";
                    SqlCommand cmd = new SqlCommand(insertar, con);
                    cmd.Parameters.AddWithValue("@Taller", alumno.Taller);
                    cmd.Parameters.AddWithValue("@Nombre", alumno.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", alumno.Apellidos);
                    cmd.Parameters.AddWithValue("@Direccion", alumno.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", alumno.Telefono);
                    cmd.Parameters.AddWithValue("@Estado_Civil", alumno.Estado);
                    cmd.Parameters.AddWithValue("@Fecha_Nacimiento", alumno.Fecha);
                    cmd.Parameters.AddWithValue("@Lugar_Nacimiento", alumno.Lugar);
                    cmd.Parameters.AddWithValue("Seguro_Medico", alumno.Seguro);
                    cmd.Parameters.AddWithValue("@Tipo_Seguro", alumno.TipoSeguro);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Alumno agregado correctamente");
                    //Disparar evento para actualizar el formulario 3
                    AlumnoAgregado?.Invoke();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible establecer conexión con la base de datos " + ex.Message);
                }

            }
            
        }  
    }
}

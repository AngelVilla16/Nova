using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Nova
{
    public partial class Form3 : Form
    {
        string ruta;
        string cadena_conexion;
        public Form3()
        {
            InitializeComponent();
            ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NovaDb.mdf");
            cadena_conexion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\NovaDb.mdf;Integrated Security=True;";


        }
        private void CargarAlumnos()
        {
            //Conectando con la base de datos para establecer una consulta
            //La consulta va a generar las columnas del datagrid a traves de un datareader
            using(SqlConnection con = new SqlConnection(cadena_conexion))
            {
                //Try catch para la tolerancia a fallos
                try
                {
                    con.Open();
                    string consulta = "SELECT * FROM Alumnos";
                    //Crear un comando para ejecutar la consulta
                    using(SqlCommand cmd = new SqlCommand(consulta, con))
                    {
                        //Ejecutamos el comando y obtenemos un lector
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable tabla = new DataTable();
                            tabla.Load(reader);
                            dgvAlumnos.DataSource  = tabla;
                        }
                    }
                }
                catch(SqlException exception)
                {
                    MessageBox.Show("Error al cargar alumnos " +  exception.Message);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error al abrir la base de datos " + ex.Message);
                }
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            CargarAlumnos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.AlumnoAgregado += () =>
            {
                CargarAlumnos();
            };
             form4.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dgvAlumnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un alumno para remover.");
                return;
            }
            DialogResult confirmacion = MessageBox.Show("¿Esta seguro que desea remover al alumno? ", "Confirmar eliminación ", MessageBoxButtons.YesNo);
            if (confirmacion == DialogResult.No)
            {
                return;
            }
            int idalumno = Convert.ToInt32(dgvAlumnos.SelectedRows[0].Cells["IdAlumno"].Value);

            using (SqlConnection con = new SqlConnection(cadena_conexion))
            {
                try
                {
                    con.Open();
                    string eliminar = "DELETE FROM Alumnos WHERE IdAlumno = @IdAlumno";
                    SqlCommand cmd = new SqlCommand(eliminar, con);
                    cmd.Parameters.AddWithValue("@IdAlumno", idalumno);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Alumno eliminado correctamente.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar al alumno " + ex.Message);
                }
            }
            CargarAlumnos();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccion = comboBox1.SelectedItem.ToString();

            switch (seleccion)
            {
                case "Todos":
                    MostrarTodosLosAlumnos();
                    break;
                default:
                    FiltrarPorTaller(seleccion);
                    break;
            }






        }
        private void FiltrarPorTaller(string nombreTaller)
        {
            if (!dgvAlumnos.Columns.Contains("Taller"))
                return;

            PrepararParaFiltrar();

            int index = dgvAlumnos.Columns["Taller"].Index;

            foreach (DataGridViewRow row in dgvAlumnos.Rows)
            {
                if (row.IsNewRow) continue;

                string valorCelda = row.Cells[index].Value?.ToString().Trim();
                row.Visible = (valorCelda == nombreTaller);
            }
        }


        private void PrepararParaFiltrar()
        {
            dgvAlumnos.EndEdit();
            dgvAlumnos.ClearSelection();
            dgvAlumnos.CurrentCell = null;
        }

        private void MostrarTodosLosAlumnos()
        {
            foreach (DataGridViewRow row in dgvAlumnos.Rows)
            {
                if (row.IsNewRow) continue;
                row.Visible = true;
            }
        }
        

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Practica1
{
    public partial class Form3 : Form
    {
        private SerialPort arduinoSerial;
        private Thread arduinoThread;
        private bool isArduinoConnected;

        public Form3()
        {
            InitializeComponent();

          
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            // Inicializar la comunicación serial con Arduino
            arduinoSerial = new SerialPort("COM4", 9600);
            try
            {
                arduinoSerial.Open();
                isArduinoConnected = true;
                // Crear y comenzar el hilo para leer datos de Arduino en segundo plano
                arduinoThread = new Thread(ArduinoLoop);
                arduinoThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el puerto COM: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ArduinoLoop()
        {
            while (true)
            {
                if (arduinoSerial.IsOpen)
                {
                    try
                    {
                        string receivedData = arduinoSerial.ReadLine();

                        // Procesar los datos recibidos desde Arduino
                        // Aquí puedes agregar la lógica para actualizar la interfaz o realizar acciones específicas
                        // en base a los datos recibidos desde Arduino

                        // Mostrar el ID leído en el TextBox1
                        textBox1.Invoke((MethodInvoker)delegate
                        {
                            textBox1.Text = receivedData.Trim();
                        });
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción que pueda ocurrir al leer datos de Arduino
                        MessageBox.Show("Error al leer datos de Arduino: " + ex.Message, "Error de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Agregar un retardo para evitar un bucle infinito rápido
                Thread.Sleep(100);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();
            DialogResult rs = fo.ShowDialog();
            if (rs == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(fo.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string idHuella = textBox1.Text;
            string matricula = textBox2.Text;
            string nombre = textBox3.Text;
            string ape1 = textBox4.Text;
            string ape2 = textBox5.Text;
            string semestre = textBox6.Text;
            string carrera = textBox7.Text;
            string fotografia = pictureBox1.Text;

            // Insertar los datos en la base de datos
            ConexionMaestra.ejecutarConsulta("CALL insertar_huella ('" +
                idHuella + "','" +
                matricula + "','" +
                nombre + "','" +
                ape1 + "','" +
                ape2 + "','" +
                semestre + "','" +
                carrera + "','" +
                fotografia + "');");
            ConexionMaestra.leer.Close();

            // Realizar la consulta para obtener la información de los campos insertados
            string consulta = "SELECT * FROM tu_tabla WHERE id = '" + idHuella + "'";
            DataTable dt = ConexionMaestra.obtenerDatos(consulta);

            if (dt.Rows.Count > 0)
            {
                // Obtener los valores de los campos consultados
                string resultadoCampo1 = dt.Rows[0]["idHuella"].ToString();
                string resultadoCampo2 = dt.Rows[0]["matricula"].ToString();
                string resultadoCampo3 = dt.Rows[0]["nombre"].ToString();
                // ...

                // Mostrar la información de los campos consultados
                MessageBox.Show("Información consultada:\nCampo1: " + resultadoCampo1 + "\nCampo2: " + resultadoCampo2 + "\nCampo3: " + resultadoCampo3, "Consulta de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Enviar los datos a Arduino a través de la comunicación serial
            string dataToSend = "Some data to send to Arduino";
            arduinoSerial.WriteLine(dataToSend);

            MessageBox.Show("USUARIO REGISTRADO CON ÉXITO", "MENSAJE #1 REGISTRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (isArduinoConnected && arduinoSerial.IsOpen)
            {
                string valor = arduinoSerial.ReadLine();
                textBox1.Text = valor.Trim(); // Eliminar los espacios en blanco alrededor del valor leído
            }
            {
                MessageBox.Show("No se puede leer desde Arduino porque no está conectado.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

            private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detener el hilo de Arduino y cerrar la conexión serial antes de cerrar la aplicación
            if (arduinoThread != null && arduinoThread.IsAlive)
            {
                arduinoThread.Abort();
            }
            if (arduinoSerial.IsOpen)
            {
                arduinoSerial.Close();
            }
        }
    }
}



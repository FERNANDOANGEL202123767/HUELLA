using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Practica1
{
    public partial class Form1 : Form
    {
        private SerialPort arduinoSerial;
        private bool isArduinoConnected;
        private Thread arduinoThread;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConexionMaestra.conectar();
            isArduinoConnected = false;
            arduinoThread = new Thread(ArduinoLoop);
            arduinoThread.Start();
        }

        private void ArduinoLoop()
        {
            arduinoSerial = new SerialPort("COM4", 9600);
            try
            {
                arduinoSerial.Open();
                isArduinoConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con Arduino: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isArduinoConnected = false;
                return;
            }

            while (true)
            {
                if (isArduinoConnected && arduinoSerial.IsOpen)
                {
                    try
                    {
                        string receivedData = arduinoSerial.ReadLine();
                        // Procesar los datos recibidos desde Arduino
                        // Aquí puedes agregar la lógica para actualizar la interfaz o realizar acciones específicas
                        // en base a los datos recibidos desde Arduino
                        // Por ejemplo:
                        // textBox1.Text = receivedData;
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción que pueda ocurrir al leer datos de Arduino
                        MessageBox.Show("Error al leer datos de Arduino: " + ex.Message, "Error de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isArduinoConnected = false;
                        arduinoSerial.Close();
                        return;
                    }
                }
                else
                {
                    // Si no está conectado a Arduino, intenta abrir la conexión
                    try
                    {
                        arduinoSerial.Open();
                        isArduinoConnected = true;
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción que pueda ocurrir al intentar abrir la conexión con Arduino
                        MessageBox.Show("Error al conectar con Arduino: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isArduinoConnected = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            ConexionMaestra.ejecutarConsulta("CALL insertar_huella ('" +
                textBox1.Text + "','" +
                textBox2.Text + "','" +
                textBox3.Text + "','" +
                textBox4.Text + "','" +
                textBox5.Text + "','" +
                textBox6.Text + "','" +
                textBox7.Text + "','" +
                pictureBox1.Text + "');");
            ConexionMaestra.leer.Close();

            // Enviar los datos a Arduino a través de la comunicación serial
            string dataToSend = "Some data to send to Arduino";
            if (isArduinoConnected && arduinoSerial.IsOpen)
            {
                arduinoSerial.WriteLine(dataToSend);
            }

            MessageBox.Show("USUARIO REGISTRADO CON EXITO", "MENSAJE #1 REGISTRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detener el hilo de Arduino y cerrar la conexión serial antes de cerrar la aplicación
            if (arduinoThread != null && arduinoThread.IsAlive)
            {
                arduinoThread.Abort();
            }
            if (isArduinoConnected && arduinoSerial.IsOpen)
            {
                arduinoSerial.Close();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (isArduinoConnected && arduinoSerial.IsOpen)
            {
                string valor = arduinoSerial.ReadLine();
                textBox1.Text = valor.Trim(); // Eliminar los espacios en blanco alrededor del valor leído
            }
            else
            {
                MessageBox.Show("No se puede leer desde Arduino porque no está conectado.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}

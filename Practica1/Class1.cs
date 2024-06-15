using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Practica1
{
    internal class Class1
    {/*
        private SerialPort arduinoSerial;

        public Class1()
        {
            // Inicializar la comunicación serial con Arduino
            arduinoSerial = new SerialPort("COM7", 9600);
            arduinoSerial.DataReceived += ArduinoSerial_DataReceived;
            arduinoSerial.Open();
        }

        private void ArduinoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Procesar los datos recibidos desde Arduino
            string receivedData = arduinoSerial.ReadLine();

            // Aquí puedes agregar la lógica para actualizar la interfaz o realizar acciones específicas
            // en base a los datos recibidos desde Arduino
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string campo2 = textBox2.Text;
            string campo3 = textBox3.Text;
            string campo4 = textBox4.Text;
            string campo5 = textBox5.Text;
            string campo6 = textBox6.Text;
            string campo7 = textBox7.Text;
            string imagen = pictureBox1.Text;

            // Insertar los datos en la base de datos
            ConexionMaestra.ejecutarConsulta("CALL insertar_huella ('" +
                id + "','" +
                campo2 + "','" +
                campo3 + "','" +
                campo4 + "','" +
                campo5 + "','" +
                campo6 + "','" +
                campo7 + "','" +
                imagen + "');");
            ConexionMaestra.leer.Close();

            // Realizar la consulta para obtener la información de los campos insertados
            string consulta = "SELECT * FROM tu_tabla WHERE id = '" + id + "'";
            DataTable dt = ConexionMaestra.obtenerDatos(consulta);

            if (dt.Rows.Count > 0)
            {
                // Obtener los valores de los campos consultados
                string resultadoCampo1 = dt.Rows[0]["campo1"].ToString();
                string resultadoCampo2 = dt.Rows[0]["campo2"].ToString();
                string resultadoCampo3 = dt.Rows[0]["campo3"].ToString();
                // ...

                // Mostrar la información de los campos consultados
                MessageBox.Show("Información consultada:\nCampo1: " + resultadoCampo1 + "\nCampo2: " + resultadoCampo2 + "\nCampo3: " + resultadoCampo3, "Consulta de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Enviar los datos a Arduino a través de la comunicación serial
            string dataToSend = "Some data to send to Arduino";
            arduinoSerial.WriteLine(dataToSend);

            MessageBox.Show("USUARIO REGISTRADO CON ÉXITO", "MENSAJE #1 REGISTRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }*/
    }
}


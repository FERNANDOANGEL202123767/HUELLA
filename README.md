# Sistema de Registro de Huellas Dactilares (HUELLA)

## 📖 Descripción

**HUELLA** es una aplicación de escritorio desarrollada en **C#** con **Windows Forms** (.NET Framework 4.7.2) diseñada para registrar y gestionar información de usuarios, incluyendo huellas dactilares, datos personales, y fotografías. 

La aplicación se comunica con un sensor de huellas dactilares conectado a un **Arduino** a través de un puerto serial (COM4) y almacena la información en una base de datos **MySQL** llamada "huella".

### 🎯 Funcionalidades Principales

- Registrar usuarios con su ID de huella, matrícula, nombre, apellidos, semestre, carrera, y fotografía
- Consultar datos almacenados en la base de datos
- Enviar y recibir datos desde un Arduino para procesar lecturas de huellas
- Visualizar imágenes asociadas a los usuarios

El proyecto está orientado a aplicaciones como **control de acceso** o **registro de asistencia estudiantil** en instituciones educativas.

## ✨ Características

- **Interfaz Gráfica**: Formularios en Windows Forms para ingreso y consulta de datos
- **Comunicación Serial**: Interacción con Arduino a través de puerto COM4 para leer huellas dactilares
- **Base de Datos**: Almacenamiento de datos en MySQL con un procedimiento almacenado (`insertar_huella`)
- **Carga de Imágenes**: Permite asociar fotografías a los registros de usuarios
- **Gestión de Conexión**: Manejo robusto de conexiones seriales y de base de datos con manejo de excepciones
- **Múltiples Formularios**: Incluye tres formularios (Form1, Form2, Form3) para diferentes funcionalidades

## 📁 Estructura del Repositorio

```
HUELLA/
├── bin/
│   ├── Debug/
│   │   ├── practica1.exe
│   │   ├── practica1.exe.config
│   │   └── practica1.pdb
│   └── Release/
├── obj/
│   ├── Debug/
│   └── Release/
├── Properties/
│   ├── AssemblyInfo.cs
│   ├── Resources.Designer.cs
│   ├── Resources.resx
│   ├── Settings.Designer.cs
│   └── Settings.settings
├── app.config
├── Class1.cs
├── ConexionMaestra.cs
├── Form1.cs
├── Form1.Designer.cs
├── Form1.resx
├── Form2.cs
├── Form2.Designer.cs
├── Form3.cs
├── Form3.Designer.cs
├── Form3.resx
├── Practica1.csproj
└── Program.cs
```

## 🛠️ Requisitos

### Hardware
- Computadora con Windows (7, 8, 10, o 11)
- Arduino (por ejemplo, Arduino Uno) con sensor de huellas dactilares (R307 o similar)
- Cable USB para conectar el Arduino a la computadora
- Sensor de Huellas Dactilares compatible (ejemplo: Adafruit Fingerprint Sensor)
- Fuente de alimentación adecuada para el Arduino y el sensor

### Software
- **Visual Studio 2019** o superior (con soporte para .NET Framework 4.7.2 y Windows Forms)
- **MySQL Server** (versión 5.1 o superior)
- **MySQL ODBC Connector** (versión 5.1 o compatible)
- **Arduino IDE** para cargar el firmware
- **.NET Framework 4.7.2** instalado en el sistema
- **Git** (opcional, para clonar el repositorio)

### Base de Datos
Una base de datos MySQL llamada `huella` con:
- Tabla para almacenar los datos de usuarios
- Procedimiento almacenado `insertar_huella`

## 🗄️ Configuración de Base de Datos

### Estructura de la Base de Datos

```sql
CREATE DATABASE huella;
USE huella;

CREATE TABLE usuarios (
    idHuella VARCHAR(50) PRIMARY KEY,
    matricula VARCHAR(50),
    nombre VARCHAR(100),
    ape1 VARCHAR(100),
    ape2 VARCHAR(100),
    semestre VARCHAR(10),
    carrera VARCHAR(100),
    fotografia VARCHAR(255)
);

DELIMITER //
CREATE PROCEDURE insertar_huella (
    IN p_idHuella VARCHAR(50),
    IN p_matricula VARCHAR(50),
    IN p_nombre VARCHAR(100),
    IN p_ape1 VARCHAR(100),
    IN p_ape2 VARCHAR(100),
    IN p_semestre VARCHAR(10),
    IN p_carrera VARCHAR(100),
    IN p_fotografia VARCHAR(255)
)
BEGIN
    INSERT INTO usuarios (idHuella, matricula, nombre, ape1, ape2, semestre, carrera, fotografia)
    VALUES (p_idHuella, p_matricula, p_nombre, p_ape1, p_ape2, p_semestre, p_carrera, p_fotografia);
END //
DELIMITER ;
```

## 🔧 Firmware de Arduino

### Sketch Básico para Arduino

```cpp
#include <Adafruit_Fingerprint.h>
#include <SoftwareSerial.h>

SoftwareSerial mySerial(2, 3); // RX, TX
Adafruit_Fingerprint finger = Adafruit_Fingerprint(&mySerial);

void setup() {
  Serial.begin(9600);
  mySerial.begin(9600);
  finger.begin(9600);
  if (finger.verifyPassword()) {
    Serial.println("Sensor de huellas encontrado!");
  } else {
    Serial.println("No se encontró el sensor de huellas :(");
    while (1);
  }
}

void loop() {
  int id = getFingerprintID();
  if (id != -1) {
    Serial.println(id); // Enviar ID de huella al puerto serial
  }
  delay(100);
}

int getFingerprintID() {
  uint8_t p = finger.getImage();
  if (p != FINGERPRINT_OK) return -1;
  p = finger.image2Tz();
  if (p != FINGERPRINT_OK) return -1;
  p = finger.fingerFastSearch();
  if (p != FINGERPRINT_OK) return -1;
  return finger.fingerID;
}
```

## 🚀 Instalación

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/FERNANDOANGEL202123767/HUELLA.git
   cd HUELLA
   ```

2. **Configura la base de datos:**
   - Instala MySQL Server y crea la base de datos `huella`
   - Ejecuta el script SQL proporcionado arriba
   - Instala el MySQL ODBC Connector
   - Configura el usuario `root` con contraseña `2002` (ajusta en `ConexionMaestra.cs` si es necesario)

3. **Configura el Arduino:**
   - Conecta el sensor de huellas al Arduino (pines RX/TX, típicamente 2 y 3)
   - Instala el Arduino IDE y la biblioteca `Adafruit_Fingerprint`
   - Carga el sketch proporcionado al Arduino
   - Conecta el Arduino al puerto **COM4**

4. **Abre el proyecto en Visual Studio:**
   - Carga el archivo `Practica1.csproj`
   - Verifica que .NET Framework 4.7.2 esté instalado
   - Compila la solución

5. **Ejecuta la aplicación:**
   - Ejecuta desde Visual Studio (F5) o desde `bin/Debug/practica1.exe`

## 📖 Uso

### Iniciar la Aplicación
- Al abrir, se conectará automáticamente con MySQL y Arduino (COM4)
- Si hay errores de conexión, se mostrará un mensaje

### Registrar un Usuario

| Campo | Descripción |
|-------|-------------|
| **Id_Huella** | Identificador único de la huella (leído desde Arduino) |
| **Matrícula** | Número de matrícula del usuario |
| **Nombre** | Nombre del usuario |
| **Apellido Paterno** | Primer apellido |
| **Apellido Materno** | Segundo apellido |
| **Semestre** | Semestre actual |
| **Carrera** | Carrera del usuario |

### Controles Principales

| Botón | Función |
|-------|---------|
| **Foto** (button2) | Cargar imagen desde computadora |
| **Huella** (button4) | Leer ID de huella desde Arduino |
| **Agregar** (button1/button4) | Guardar datos en base de datos |
| **Verificar** (button5) | Consultar datos existentes |
| **Salir** (button3) | Cerrar aplicación |

## ⚠️ Notas Importantes

- **Puerto Serial**: Configurado para **COM4**. Actualiza en `Form1.cs` y `Form3.cs` si usas otro puerto
- **Seguridad**: Las credenciales están en texto plano. Para producción, usa un usuario con permisos limitados
- **SQL Injection**: Las consultas concatenan strings directamente. Para producción, usa consultas parametrizadas
- **Form2**: Está vacío. Considera eliminarlo si no se usa
- **Class1.cs**: Contiene código comentado que puede eliminarse

## 🤝 Contribuciones

Las contribuciones son bienvenidas:

1. Haz un fork del repositorio
2. Crea una rama para tus cambios (`git checkout -b feature/nueva-funcionalidad`)
3. Realiza los cambios y haz commit (`git commit -m "Añade nueva funcionalidad"`)
4. Sube la rama (`git push origin feature/nueva-funcionalidad`)
5. Crea un Pull Request en GitHub

## 📄 Licencia

Este proyecto está bajo la licencia MIT.

## 📞 Contacto

- **Autor**: Fernando Angel
- **GitHub**: [@FERNANDOANGEL202123767](https://github.com/FERNANDOANGEL202123767)

Para preguntas, sugerencias o problemas, abre un **issue** en el repositorio o contacta al autor.

---

¡Gracias por visitar el proyecto HUELLA! 👆

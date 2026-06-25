# SIGEDAA - Sistema de Gestión de Atletismo

**SIGEDAA** es una aplicación web integral para la gestión y administración de eventos, competencias y registros de atletismo. Desarrollada con **C#**, **HTML** y **CSS**.

## 📋 Descripción

SIGEDAA es una plataforma diseñada para facilitar la organización y gestión de competencias de atletismo, permitiendo administrar atletas, eventos, resultados y rankings de manera eficiente.

## 🛠️ Tecnologías Utilizadas

- **C#** (48.1%) - Lógica de backend y procesamiento de datos
- **HTML** (45.9%) - Estructura y marcado de la interfaz web
- **CSS** (5.8%) - Estilos y diseño responsivo
- **JavaScript** (0.2%) - Interactividad en el frontend

## 🏃 Características Principales

- ✅ Gestión de atletas y participantes
- ✅ Administración de eventos y competencias
- ✅ Registro y seguimiento de resultados
- ✅ Generación de rankings y estadísticas
- ✅ Sistema de usuarios y autenticación
- ✅ Interfaz intuitiva y responsiva
- ✅ Reportes y análisis de desempeño

## 📦 Instalación

### Requisitos Previos
- .NET Framework / .NET Core
- Visual Studio o VS Code
- SQL Server (o base de datos compatible)
- Navegador web moderno

### Pasos de Instalación

1. Clona el repositorio:
```bash
git clone https://github.com/naiferrosado/SIGEDAA.git
cd SIGEDAA
```

2. Restaura las dependencias NuGet:
```bash
dotnet restore
```

3. Configura la base de datos:
- Actualiza la cadena de conexión en `appsettings.json`
- Ejecuta las migraciones (si aplica):
```bash
dotnet ef database update
```

4. Compila el proyecto:
```bash
dotnet build
```

5. Ejecuta la aplicación:
```bash
dotnet run
```

6. Abre tu navegador y accede a `http://localhost:5000`

## 📖 Uso

### Acceso a la Aplicación
- Navega a la URL local donde se ejecuta la aplicación
- Inicia sesión con tus credenciales
- Accede a los módulos disponibles según tus permisos

### Módulos Principales
- **Atletas**: Registro y gestión de participantes
- **Eventos**: Creación y administración de competencias
- **Resultados**: Registro y seguimiento de desempeños
- **Rankings**: Visualización de clasificaciones y estadísticas
- **Reportes**: Generación de informes y análisis

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Para cambios importantes:

1. Fork el repositorio
2. Crea una rama para tu característica (`git checkout -b feature/NuevaCaracteristica`)
3. Commit tus cambios (`git commit -m 'Agregar nueva característica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto no tiene una licencia especificada. Para más información, contacta al propietario del repositorio.

## 👨‍💻 Autor

**naiferrosado** - [GitHub Profile](https://github.com/naiferrosado)

## 📧 Contacto y Soporte

Para reportar problemas, sugerencias o preguntas técnicas, abre un [issue](https://github.com/naiferrosado/SIGEDAA/issues) en el repositorio.

---

**Última actualización:** 25 de junio de 2026
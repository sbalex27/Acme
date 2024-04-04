# ACME

## Requerimientos
- Servicios para Creación, Modificación y eliminación de encuestas (Requiere autenticación por medio de oauth2 o jwt).
  - Nombre de Encuesta
  - Descripción de encuesta
  - Listado de campos
    - Nombre del campo
      - Título del campo (Se mostrará en la pantalla)
      - Es Requerido (s/n)
      - Tipo de Campo (Texto, Número y fecha)
  - Al crear el formulario se generará un link único que servirá para llenar la encuesta.
- Servicio para llenado de la encuesta (No requiere autenticación)
  - Al acceder al link generado se desplegarán los campos configurados en la encuesta y se permitirá ingresar los valores correspondientes, dependiendo el tipo de campo, al guardar la encuesta se almacenarán los valores ingresados para cada campo.
- Servicio para obtener los resultados de cada encuesta (Requiere autenticación por medio de oauth2 o jwt).

## Estructura
Los controladores están ubicados en `Acme/Controllers` siendo controladores Api comúnes, en el directorio `Acme/Data` encontraremos el DbContext (Se usa EntityFramework) para la conexión a la base de datos. En `Ame/Migrations` encontraremos las migraciones que deben ser ejecutadas con `Update-Database` en la Package Manager Console. Los modelos están ubicados en `Acme/Models`. Los perfiles (Mapeos DTO) están ubicados en `Acme/Profiles` y por último los servicios están ubicados en `Acme/Services.`

## Servicios
El proyecto contiene servicios que son inyectados a través de Dependency Injection a traves de el proyecto principal, estos servicios tienen sus propias pruebas unitarias para asegurar su integridad y son reutilizables, están ubicados en `Acme/Services` y cuentan con funciones como por ejemplo la generación de Links de formularios. Estos servicios están configurados en `Program.cs`.

```c#
builder.Services.AddSingleton<Acme.Services.LinkGenerator>();
builder.Services.AddScoped<Acme.Services.SecurityTools>();
```

## Testing
La solución cuenta con un proyecto de Test para poder asegurar la integridad de los servicios principalmente, los tests están ubicados en el proyecto `Test` incluído en la solución los cuales pueden ser ejecutados por medio de el explorador de pruebas de Visual Studio

![image](https://github.com/sbalex27/Acme/assets/48226829/945f9292-7f0f-4032-be56-49fd63476806)


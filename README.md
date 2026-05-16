# BookOnWeb
Esempio correlato al mio eBook "Imparare a programmare"

---

Applicazione web sviluppata con:

* .NET 10
* ASP.NET Core MVC
* Entity Framework Core
* Microsoft SQL Server
* Bootstrap
* REST API

Il progetto nasce come esempio completo di architettura moderna per applicazioni web basate su ASP.NET Core, includendo sia componenti MVC tradizionali sia API REST utilizzate tramite chiamate AJAX dal frontend.

---

# Caratteristiche principali

## Backend

* ASP.NET Core MVC
* REST API
* Entity Framework Core
* SQL Server
* Dependency Injection
* Repository Pattern
* Service Layer
* Async/Await
* DTO
* Validazione modelli

---

## Frontend

* Bootstrap
* Razor Views
* AJAX Fetch API
* Layout responsive
* Componenti dinamici
* Modali Bootstrap

---

# Architettura del progetto

```text
BibliotecaApp
│
├── Controllers
│   ├── MVC Controllers
│   └── API Controllers
│
├── Models
│
├── DTOs
│
├── Services
│
├── Repositories
│
├── Data
│
├── Views
│
├── wwwroot
│   ├── js
│   ├── css
│   └── lib
│
└── Program.cs
```

---

# Tecnologie utilizzate

| Tecnologia            | Descrizione          |
| --------------------- | -------------------- |
| .NET 10               | Framework principale |
| ASP.NET Core MVC      | Architettura web     |
| Entity Framework Core | ORM                  |
| SQL Server            | Database relazionale |
| Bootstrap             | Frontend responsive  |
| JavaScript            | Chiamate AJAX        |
| REST API              | Comunicazione HTTP   |

---

# Funzionalità incluse

## Gestione libri

* Creazione libri
* Modifica libri
* Eliminazione libri
* Lista libri
* Ricerca libri

---

## Gestione autori

* Relazione uno-a-molti
* CRUD completo

---

## API REST

Endpoint REST di esempio:

```http
GET /api/libri
```

```http
GET /api/libri/1
```

```http
POST /api/libri
```

```http
PUT /api/libri/1
```

```http
DELETE /api/libri/1
```

---

# Esempio risposta JSON

```json
{
  "id": 1,
  "titolo": "1984",
  "annoPubblicazione": 1949
}
```

---

# AJAX nel frontend

Le API vengono utilizzate anche dal frontend tramite chiamate AJAX.

Esempio:

```javascript
async function caricaLibri() {

    const response = await fetch('/api/libri');

    const data = await response.json();

    console.log(data);
}
```

---

# Prerequisiti

Installare:

* .NET 10 SDK
* SQL Server
* SQL Server Management Studio (opzionale)

---

# Configurazione database

Modificare la connection string nel file:

```text
appsettings.json
```

Esempio:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Server=.;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

# Installazione progetto

## Clonare repository

```bash
git clone https://github.com/username/BibliotecaApp.git
```

---

## Entrare nella cartella

```bash
cd BibliotecaApp
```

---

## Ripristinare pacchetti

```bash
dotnet restore
```

---

## Creare database

```bash
dotnet ef database update
```

---

## Avviare applicazione

```bash
dotnet run
```

---

# Swagger

Il progetto include Swagger per testare le API REST.

URL:

```text
https://localhost:5001/swagger
```

---

# Entity Framework Core

Il progetto utilizza:

* Migration
* LINQ
* Relazioni
* Query async
* Fluent API

---

# Bootstrap

Frontend sviluppato con Bootstrap per garantire:

* responsive design;
* semplicità;
* compatibilità mobile;
* componenti UI moderni.

---

# Pattern utilizzati

## Repository Pattern

Separazione accesso dati.

---

## Service Layer

Separazione logica applicativa.

---

## Dependency Injection

Gestione dipendenze tramite container .NET.

---

# Obiettivo del progetto

Questo progetto è pensato come:

* esempio didattico;
* base per applicazioni reali;
* template iniziale;
* riferimento architetturale per ASP.NET Core MVC + API.

---

# Possibili estensioni future

* autenticazione JWT
* Identity
* autorizzazioni ruoli
* upload file
* caching
* Docker
* logging avanzato
* testing automatico
* pagination
* ricerca avanzata
* SignalR
* microservizi

---

# Licenza

Progetto rilasciato a scopo didattico.

---

# Autore

Sviluppato come esempio architetturale moderno con:

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* REST API
* Bootstrap


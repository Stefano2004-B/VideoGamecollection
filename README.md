# 🎮 VideoGame Collection

Applicazione web sviluppata in **ASP.NET Core 8 (Razor Pages)** che permette di gestire la propria collezione personale di videogiochi. Include sistema di autenticazione, integrazione con l'API pubblica [RAWG.io](https://rawg.io/apidocs) per la ricerca dei giochi e persistenza dei dati su database SQL Server.

---

## ✨ Funzionalità

- 🔐 **Registrazione e Login utenti** con password hashate tramite **BCrypt**
- 👤 Ogni utente ha la propria collezione privata
- 🔎 **Ricerca giochi** tramite l'API RAWG (con paginazione)
- ➕ Aggiunta giochi alla collezione direttamente dai risultati di ricerca o manualmente
- ✏️ **CRUD completo** sulla collezione (Create / Read / Update / Delete)
- ⭐ Voto personale (1-10), genere, piattaforma, note
- 📌 Stato del gioco: `Da Giocare`, `In Corso`, `Completato`, `Abbandonato`
- 🛡️ Sessione utente con timeout di 8 ore

---

## 🛠️ Tecnologie utilizzate

| Tecnologia | Utilizzo |
|---|---|
| ASP.NET Core 8.0 | Framework web |
| Razor Pages | Pattern UI |
| Entity Framework Core 8 | ORM e migrazioni |
| SQL Server | Database |
| BCrypt.Net-Next | Hashing password |
| RAWG API | Ricerca videogiochi esterna |
| HttpClientFactory | Chiamate HTTP all'API esterna |

---

## 📁 Struttura del progetto

```
VideoGameCollectionRazor/
├── Data/
│   └── AppDbContext.cs          # Contesto EF Core
├── Models/
│   ├── User.cs                  # Modello utente
│   ├── VideoGame.cs             # Modello gioco
│   └── RawgModels.cs            # DTO per l'API RAWG
├── Migrations/                  # Migrazioni EF Core
├── Pages/
│   ├── Account/                 # Login, Register, Logout, Welcome
│   ├── Collection/              # Index, Create, Edit, AddFromSearch
│   ├── Home/                    # Search
│   └── Shared/                  # Layout comune
├── wwwroot/                     # CSS, JS, asset statici
├── Program.cs                   # Configurazione app
└── appsettings.json             # Connection string e API key
```

---

## 🚀 Come eseguire il progetto

### Prerequisiti

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Un'istanza di **SQL Server** (locale, Docker o remota)
- Una **API key RAWG** gratuita (richiedibile su [rawg.io/apidocs](https://rawg.io/apidocs))

### Passaggi

1. **Clona il repository**
   ```bash
   git clone https://github.com/Stefano2004-B/VideoGamecollection.git
   cd VideoGamecollection/VideoGameCollectionRazor
   ```

2. **Configura `appsettings.json`** con la tua connection string e la tua API key:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True"
     },
     "Rawg": {
       "ApiKey": "LA_TUA_API_KEY"
     }
   }
   ```

3. **Ripristina i pacchetti**
   ```bash
   dotnet restore
   ```

4. **Applica le migrazioni** (verranno applicate anche automaticamente al primo avvio):
   ```bash
   dotnet ef database update
   ```

5. **Avvia l'applicazione**
   ```bash
   dotnet run
   ```

6. Apri il browser su `https://localhost:5001` (o la porta indicata in console). Verrai reindirizzato alla pagina di login.

---

## 🗄️ Modello dati

### `User`
| Campo | Tipo | Note |
|---|---|---|
| Id | int | PK |
| Username | string(100) | univoco |
| Email | string(200) | univoco |
| PasswordHash | string | BCrypt |
| CreatedAt | DateTime | |

### `VideoGame`
| Campo | Tipo | Note |
|---|---|---|
| Id | int | PK |
| UserId | int | FK verso User |
| Title | string(200) | obbligatorio |
| Platform | string | obbligatorio |
| PersonalScore | int | range 1-10 |
| Genre | string? | opzionale |
| ImageUrl | string? | dalla RAWG |
| AddedDate | DateTime | default `Now` |
| Notes | string? | opzionale |
| Status | string | Da Giocare / In Corso / Completato / Abbandonato |
| ExternalId | int? | ID gioco su RAWG |

---

## ⚠️ Nota sulla sicurezza

Il file `appsettings.json` presente nel repository contiene attualmente credenziali del database e una API key in chiaro. **Si raccomanda fortemente di**:

- Rimuovere `appsettings.json` dal versionamento (aggiungerlo al `.gitignore`)
- Utilizzare gli **User Secrets** in sviluppo:
  ```bash
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."
  dotnet user-secrets set "Rawg:ApiKey" "..."
  ```
- Utilizzare variabili d'ambiente o un secret manager in produzione
- Ruotare le credenziali e l'API key attualmente esposte

---

## 👤 Autore

**Stefano2004-B** — [GitHub](https://github.com/Stefano2004-B)

---

## 📄 Licenza

Progetto distribuito senza una licenza specifica. Aggiungi un file `LICENSE` (es. MIT) se vuoi rendere il codice riutilizzabile da altri.

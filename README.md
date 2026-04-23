# KvizHub – Platforma za testiranje znanja sa rang listom

## Opis projekta

KvizHub je web aplikacija za kreiranje i rešavanje kvizova iz raznih oblasti, sa sistemom bodovanja i rang listom. Projekat koristi **mikroservisnu arhitekturu** sa **Ocelot API Gateway**-om.

## Arhitektura

```
┌──────────┐     ┌─────────────────┐     ┌─────────────────┐
│  React   │────▶│  Ocelot Gateway │────▶│  UserService    │
│  :3000   │     │     :5000       │     │     :5001       │
└──────────┘     └────────┬────────┘     └─────────────────┘
                          │              ┌─────────────────┐
                          ├─────────────▶│  QuizService    │
                          │              │     :5002       │
                          │              └─────────────────┘
                          │              ┌─────────────────┐
                          └─────────────▶│  ScoreService   │
                                         │     :5003       │
                                         └─────────────────┘
```

### Mikroservisi

| Servis | Port | Baza podataka | Opis |
|--------|------|---------------|------|
| **Gateway** | 5000 | - | Ocelot API Gateway, rutira zahteve |
| **UserService** | 5001 | KvizHub_Users | Registracija, prijava, JWT, profili |
| **QuizService** | 5002 | KvizHub_Quizzes | Kategorije, kvizovi, pitanja |
| **ScoreService** | 5003 | KvizHub_Scores | Pokušaji, bodovanje, rang lista |

### Tehnologije

**Backend:**
- .NET 8 Web API
- Entity Framework Core 8 (SQL Server)
- Ocelot API Gateway
- JWT autentifikacija
- BCrypt za hešovanje lozinki
- Serilog (Gateway logovanje)

**Frontend:**
- React (JavaScript)
- React Router DOM
- Axios

## Preduslov

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18+)
- SQL Server LocalDB (dolazi sa Visual Studio)

## Pokretanje

### 1. Backend (svi servisi)

Otvorite 4 terminala i pokrenite svaki servis:

```bash
# Terminal 1 - Gateway
cd KvizHub.Gateway
dotnet run

# Terminal 2 - UserService
cd KvizHub.UserService
dotnet run

# Terminal 3 - QuizService
cd KvizHub.QuizService
dotnet run

# Terminal 4 - ScoreService
cd KvizHub.ScoreService
dotnet run
```

Baze podataka se automatski kreiraju pri prvom pokretanju (auto-migration).

### 2. Frontend

```bash
cd kvizhub-frontend
npm install
npm start
```

Aplikacija se otvara na `http://localhost:3000`.

## Podrazumevani admin nalog

- **Korisničko ime:** admin
- **Lozinka:** Admin123!

## API Rute (Gateway)

### Auth
- `POST /api/auth/register` - Registracija
- `POST /api/auth/login` - Prijava
- `POST /api/auth/refresh` - Osvežavanje tokena

### Users
- `GET /api/users/me` - Profil prijavljenog korisnika
- `PUT /api/users/me` - Ažuriranje profila
- `GET /api/users/{id}` - Profil po ID-u

### Categories
- `GET /api/categories` - Sve kategorije
- `POST /api/categories` - Kreiranje (Admin)
- `PUT /api/categories/{id}` - Ažuriranje (Admin)
- `DELETE /api/categories/{id}` - Brisanje (Admin)

### Quizzes
- `GET /api/quizzes` - Svi kvizovi
- `GET /api/quizzes/filter?categoryId=&difficulty=&search=` - Filtriranje
- `GET /api/quizzes/{id}` - Detalji kviza
- `GET /api/quizzes/{id}/play` - Pitanja za igrača (bez tačnih odgovora)
- `POST /api/quizzes` - Kreiranje (Admin)

### Questions
- `POST /api/questions/quiz/{quizId}` - Dodavanje pitanja (Admin)
- `PUT /api/questions/{id}` - Ažuriranje (Admin)
- `DELETE /api/questions/{id}` - Brisanje (Admin)

### Attempts
- `POST /api/attempts` - Slanje odgovora
- `GET /api/attempts/my` - Moji pokušaji
- `GET /api/attempts/my/stats` - Moja statistika

### Leaderboard
- `GET /api/leaderboard` - Globalna rang lista
- `GET /api/leaderboard/quiz/{quizId}` - Rang lista po kvizu

## Struktura projekta

```
Predmet_Projekat/
├── KvizHub.Gateway/          # API Gateway (Ocelot)
├── KvizHub.UserService/      # Korisnički servis
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   └── Services/
├── KvizHub.QuizService/      # Kviz servis
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   └── Services/
├── KvizHub.ScoreService/     # Servis za bodovanje
│   ├── Controllers/
│   ├── Data/
│   ├── HttpClients/
│   ├── Models/
│   ├── Repositories/
│   └── Services/
└── kvizhub-frontend/         # React frontend
    └── src/
        ├── components/
        ├── context/
        ├── pages/
        └── services/
```

## Troslojna arhitektura (svaki mikroservis)

```
Controller → Service → Repository → DbContext → SQL Server
```

## Autor

Studentski projekat

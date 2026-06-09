using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(QuizDbContext context)
        {
            // Check if data already exists
            if (context.Quizzes.Any())
                return;

            var adminUserId = 1;

            // =============================================
            // KVIZ 1: Osnove JavaScript-a
            // =============================================
            var quiz1 = new Quiz
            {
                Title = "Osnove JavaScript-a",
                Description = "Testirajte svoje znanje osnova JavaScript programskog jezika",
                CategoryId = 1,
                Difficulty = "Easy",
                TimeLimit = 10,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koja je razlika izmedju `==` i `===` u JavaScript-u?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Nema razlike", IsCorrect = false, Order = 1 },
                            new Answer { Text = "== poredi vrednost, === poredi vrednost i tip", IsCorrect = true, Order = 2 },
                            new Answer { Text = "=== poredi samo tip", IsCorrect = false, Order = 3 },
                            new Answer { Text = "== je strogo poredjenje", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je `Promise` u JavaScript-u?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Tip podatka za brojeve", IsCorrect = false, Order = 1 },
                            new Answer { Text = "Objekat koji predstavlja asinhronu operaciju", IsCorrect = true, Order = 2 },
                            new Answer { Text = "Funkcija za ispis u konzolu", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Petlja za iteraciju", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je `let` u JavaScript-u?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Funkcija", IsCorrect = false, Order = 1 },
                            new Answer { Text = "Blok-scoped promenljiva", IsCorrect = true, Order = 2 },
                            new Answer { Text = "Petlja", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Klasa", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji metod se koristi za dodavanje elementa na kraj niza?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "push()", IsCorrect = true, Order = 1 },
                            new Answer { Text = "pop()", IsCorrect = false, Order = 2 },
                            new Answer { Text = "shift()", IsCorrect = false, Order = 3 },
                            new Answer { Text = "unshift()", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je `NaN` u JavaScript-u?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Not a Number - vrednost koja nije validan broj", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Null and Null", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Name and Number", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Node as Number", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 2: C# Mastery
            // =============================================
            var quiz2 = new Quiz
            {
                Title = "C# Mastery",
                Description = "Napredni koncepti u C# programskom jeziku",
                CategoryId = 1,
                Difficulty = "Hard",
                TimeLimit = 20,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Sta je `async/await` u C#?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Hard", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Sintaksni secer za rad sa asinhronim operacijama", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Novi tip podatka", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Kontrolna struktura za petlje", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Samo za GUI aplikacije", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je Dependency Injection?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Hard", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Dizajn pattern koji omogucava labavo povezivanje klasa", IsCorrect = true, Order = 1 },
                            new Answer { Text = "SQL upit", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Tip baze podataka", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Metod za enkripciju", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta radi `LINQ` u C#?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Hard", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Omogucava upite nad kolekcijama i bazama podataka", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Kreira nove klase", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Upravlja memorijom", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Komunicira sa spoljnim API-jem", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je `IDisposable` interfejs?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Hard", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Omogucava oslobadjanje neupravljanih resursa", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Interfejs za serijalizaciju", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Oznacava da je klasa apstraktna", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Koristi se za logovanje", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja je razlika izmedju `abstract` klase i `interface`-a?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Hard", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Abstract klasa moze imati implementaciju, interface samo deklaracije (do C# 8)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Nema razlike", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Interface moze imati polja, abstract ne moze", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Abstract klasa ne moze imati metode", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 3: SQL i baze podataka
            // =============================================
            var quiz3 = new Quiz
            {
                Title = "SQL i baze podataka",
                Description = "Pitanja o SQL upitima i relacionim bazama podataka",
                CategoryId = 1,
                Difficulty = "Medium",
                TimeLimit = 15,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koja SQL komanda se koristi za dobavljanje podataka iz baze?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "SELECT", IsCorrect = true, Order = 1 },
                            new Answer { Text = "INSERT", IsCorrect = false, Order = 2 },
                            new Answer { Text = "UPDATE", IsCorrect = false, Order = 3 },
                            new Answer { Text = "DELETE", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je PRIMARY KEY?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Jedinstveni identifikator svakog reda u tabeli", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Kljuc za spajanje dve baze", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Vrsta indeksa", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Funkcija za hesiranje", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja vrsta JOIN-a vraca sve redove iz leve tabele?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "LEFT JOIN", IsCorrect = true, Order = 1 },
                            new Answer { Text = "INNER JOIN", IsCorrect = false, Order = 2 },
                            new Answer { Text = "RIGHT JOIN", IsCorrect = false, Order = 3 },
                            new Answer { Text = "FULL OUTER JOIN", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je normalizacija baze podataka?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Proces organizacije podataka radi smanjenja redundanse", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Proces kopiranja baze", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Proces brisanja podataka", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Proces enkripcije baze", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je INDEX u bazi podataka?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Struktura koja ubrzava pretragu podataka", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Spisak svih tabela", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Vrsta podatka", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Konekcija ka bazi", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 4: Svetska istorija XX veka
            // =============================================
            var quiz4 = new Quiz
            {
                Title = "Svetska istorija XX veka",
                Description = "Najvazniji dogadjaji iz 20. veka",
                CategoryId = 2,
                Difficulty = "Medium",
                TimeLimit = 15,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koje godine je pocet Prvi svetski rat?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "1914.", IsCorrect = true, Order = 1 },
                            new Answer { Text = "1918.", IsCorrect = false, Order = 2 },
                            new Answer { Text = "1939.", IsCorrect = false, Order = 3 },
                            new Answer { Text = "1905.", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Ko je bio prvi covek u svemiru?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Jurij Gagarin", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Neil Armstrong", IsCorrect = false, Order = 2 },
                            new Answer { Text = "John Glenn", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Buzz Aldrin", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koje godine je pao Berlinski zid?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "1989.", IsCorrect = true, Order = 1 },
                            new Answer { Text = "1985.", IsCorrect = false, Order = 2 },
                            new Answer { Text = "1991.", IsCorrect = false, Order = 3 },
                            new Answer { Text = "1979.", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja organizacija je osnovana 1945. godine?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Ujedinjene nacije (UN)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "NATO", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Evropska unija", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Svetska banka", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Ko je izmislio stampu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Johannes Gutenberg", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Leonardo da Vinci", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Nikola Tesla", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Isaac Newton", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 5: Stari Rim
            // =============================================
            var quiz5 = new Quiz
            {
                Title = "Stari Rim",
                Description = "Od osnivanja do pada Rimskog carstva",
                CategoryId = 2,
                Difficulty = "Easy",
                TimeLimit = 10,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koje godine je osnovan Rim (po legendi)?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "753. godine p.n.e.", IsCorrect = true, Order = 1 },
                            new Answer { Text = "476. godine p.n.e.", IsCorrect = false, Order = 2 },
                            new Answer { Text = "509. godine p.n.e.", IsCorrect = false, Order = 3 },
                            new Answer { Text = "27. godine p.n.e.", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Ko je bio prvi rimski car?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "August (Oktavijan)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Julije Cezar", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Neron", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Marko Aurelije", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji jezik su govorili stari Rimljani?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Latinski", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Grcki", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Italijanski", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Spanski", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Kako se zove poznati rimski amfiteatar?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Koloseum", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Partenon", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Panteon", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Akropolj", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koje godine je pao Zapadni Rimski imperija?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "476. godine n.e.", IsCorrect = true, Order = 1 },
                            new Answer { Text = "395. godine n.e.", IsCorrect = false, Order = 2 },
                            new Answer { Text = "500. godine n.e.", IsCorrect = false, Order = 3 },
                            new Answer { Text = "410. godine n.e.", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 6: Fizika za sve
            // =============================================
            var quiz6 = new Quiz
            {
                Title = "Fizika za sve",
                Description = "Osnovni pojmovi iz fizike",
                CategoryId = 3,
                Difficulty = "Medium",
                TimeLimit = 15,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Ko je formulisao zakon gravitacije?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Isak Njutn", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Albert Ajnstajn", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Galilej Galilej", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Nikola Tesla", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja je brzina svetlosti u vakuumu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "299.792.458 m/s", IsCorrect = true, Order = 1 },
                            new Answer { Text = "150.000.000 m/s", IsCorrect = false, Order = 2 },
                            new Answer { Text = "3.000.000 m/s", IsCorrect = false, Order = 3 },
                            new Answer { Text = "100.000.000 m/s", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je jedinica za silu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Njutn (N)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Dzul (J)", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Vat (W)", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Paskal (Pa)", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji je treci Njutnov zakon?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Akcija i reakcija - svaka akcija ima jednaku i suprotnu reakciju", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Telo u mirovanju ostaje u mirovanju", IsCorrect = false, Order = 2 },
                            new Answer { Text = "F = m * a", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Energija se ne moze stvoriti niti unistiti", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta meri voltmetar?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Napon (elektricni potencijal)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Jacinu struje", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Otpor", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Snagu", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 7: Hemijski elementi
            // =============================================
            var quiz7 = new Quiz
            {
                Title = "Hemijski elementi",
                Description = "Periodni sistem i hemijski elementi",
                CategoryId = 3,
                Difficulty = "Easy",
                TimeLimit = 10,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koji je hemijski simbol za vodu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "H2O", IsCorrect = true, Order = 1 },
                            new Answer { Text = "CO2", IsCorrect = false, Order = 2 },
                            new Answer { Text = "NaCl", IsCorrect = false, Order = 3 },
                            new Answer { Text = "O2", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji je najzastupljeniji gas u Zemljinoj atmosferi?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Azot (N2)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Kiseonik (O2)", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Ugljen-dioksid (CO2)", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Argon (Ar)", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji element ima atomski broj 1?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Vodonik (H)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Helijum (He)", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Litijum (Li)", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Ugljenik (C)", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Sta je pH vrednost?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Mera kiselosti ili baznosti rastvora", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Mera temperature", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Mera gustine", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Mera pritiska", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji metal je najbolji provodnik struje?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Srebro (Ag)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Bakar (Cu)", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Zlato (Au)", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Aluminijum (Al)", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 8: Opste znanje - Zabavni kviz
            // =============================================
            var quiz8 = new Quiz
            {
                Title = "Opste znanje - Zabavni kviz",
                Description = "Pitanja iz raznih oblasti opsteg znanja",
                CategoryId = 4,
                Difficulty = "Easy",
                TimeLimit = 10,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koji je najveci okean na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Tihi okean", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Atlantski okean", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Indijski okean", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Arkticki okean", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja je najveca pustinja na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Antarktik (polarna pustinja)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Sahara", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Gobi", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Kalahari", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koje boje je nebo na Marsu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Crvenkasto (narandzasto-smedja)", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Plavo", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Zeleno", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Ljubicasto", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji je najveci sisar na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Plavi kit", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Africki slon", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Zirafa", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Spermski kit", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koje godine je izumro dodo (ptica)?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Easy", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "1662.", IsCorrect = true, Order = 1 },
                            new Answer { Text = "1500.", IsCorrect = false, Order = 2 },
                            new Answer { Text = "1800.", IsCorrect = false, Order = 3 },
                            new Answer { Text = "1900.", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 9: Geografija sveta
            // =============================================
            var quiz9 = new Quiz
            {
                Title = "Geografija sveta",
                Description = "Drzave, gradovi i kontinenti",
                CategoryId = 4,
                Difficulty = "Medium",
                TimeLimit = 15,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Koja je najveca drzava po povrsini?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Rusija", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Kanada", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Kina", IsCorrect = false, Order = 3 },
                            new Answer { Text = "SAD", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja je najmnogoljudnija drzava na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Indija", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Kina", IsCorrect = false, Order = 2 },
                            new Answer { Text = "SAD", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Indonezija", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koja je najduza reka na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Nil", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Amazon", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Mississippi-Missouri", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Jangce", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji je najvisi vrh na svetu?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Mont Everest", IsCorrect = true, Order = 1 },
                            new Answer { Text = "K2", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Kangchenjunga", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Matterhorn", IsCorrect = false, Order = 4 }
                        }
                    },
                    new Question
                    {
                        Text = "Koji grad je glavni grad Australije?",
                        QuestionType = "SingleChoice", DifficultyLevel = "Medium", Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Kanbera", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Sidnej", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Melburn", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Brizbejn", IsCorrect = false, Order = 4 }
                        }
                    }
                }
            };

            // =============================================
            // KVIZ 10: Sve vrste pitanja
            // =============================================
            var quiz10 = new Quiz
            {
                Title = "Sve vrste pitanja",
                Description = "Kviz sa svim tipovima pitanja: SingleChoice, MultipleChoice, True/False i unos teksta",
                CategoryId = 4,
                Difficulty = "Medium",
                TimeLimit = 15,
                CreatedByUserId = adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>
                {
                    // 1. SingleChoice
                    new Question
                    {
                        Text = "Koji je glavni grad Srbije?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Easy",
                        Order = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Beograd", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Novi Sad", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Nis", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Kragujevac", IsCorrect = false, Order = 4 }
                        }
                    },
                    // 2. MultipleChoice
                    new Question
                    {
                        Text = "Koji od navedenih su programski jezici? (Izaberi SVE tacne)",
                        QuestionType = "MultipleChoice",
                        DifficultyLevel = "Easy",
                        Order = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Python", IsCorrect = true, Order = 1 },
                            new Answer { Text = "JavaScript", IsCorrect = true, Order = 2 },
                            new Answer { Text = "HTML", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Java", IsCorrect = true, Order = 4 },
                            new Answer { Text = "CSS", IsCorrect = false, Order = 5 }
                        }
                    },
                    // 3. TrueFalse
                    new Question
                    {
                        Text = "Zemlja je ravna ploca.",
                        QuestionType = "TrueFalse",
                        DifficultyLevel = "Easy",
                        Order = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Tacno", IsCorrect = false, Order = 1 },
                            new Answer { Text = "Netacno", IsCorrect = true, Order = 2 }
                        }
                    },
                    // 4. FillInBlank
                    new Question
                    {
                        Text = "Koji planet je poznat kao 'Crvena planeta'? (unesite odgovor)",
                        QuestionType = "FillInBlank",
                        DifficultyLevel = "Easy",
                        Order = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Mars", IsCorrect = true, Order = 1 },
                            new Answer { Text = "mars", IsCorrect = true, Order = 2 },
                            new Answer { Text = "MARS", IsCorrect = true, Order = 3 }
                        }
                    },
                    // 5. SingleChoice
                    new Question
                    {
                        Text = "Koja je valuta u Japanu?",
                        QuestionType = "SingleChoice",
                        DifficultyLevel = "Medium",
                        Order = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Jen", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Juan", IsCorrect = false, Order = 2 },
                            new Answer { Text = "Von", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Dolar", IsCorrect = false, Order = 4 }
                        }
                    },
                    // 6. MultipleChoice
                    new Question
                    {
                        Text = "Koji su od navedenih browser-i? (Izaberi SVE)",
                        QuestionType = "MultipleChoice",
                        DifficultyLevel = "Easy",
                        Order = 6,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Chrome", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Firefox", IsCorrect = true, Order = 2 },
                            new Answer { Text = "Windows", IsCorrect = false, Order = 3 },
                            new Answer { Text = "Edge", IsCorrect = true, Order = 4 },
                            new Answer { Text = "Safari", IsCorrect = true, Order = 5 }
                        }
                    },
                    // 7. TrueFalse
                    new Question
                    {
                        Text = "Voda kljuca na 100 stepeni Celzijusa na nivou mora.",
                        QuestionType = "TrueFalse",
                        DifficultyLevel = "Medium",
                        Order = 7,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Tacno", IsCorrect = true, Order = 1 },
                            new Answer { Text = "Netacno", IsCorrect = false, Order = 2 }
                        }
                    },
                    // 8. FillInBlank
                    new Question
                    {
                        Text = "Koji element ima hemijski simbol 'O'? (unesite odgovor)",
                        QuestionType = "FillInBlank",
                        DifficultyLevel = "Easy",
                        Order = 8,
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Kiseonik", IsCorrect = true, Order = 1 },
                            new Answer { Text = "kiseonik", IsCorrect = true, Order = 2 },
                            new Answer { Text = "Oxygen", IsCorrect = true, Order = 3 },
                            new Answer { Text = "oxygen", IsCorrect = true, Order = 4 }
                        }
                    }
                }
            };

            // Add all quizzes to context
            context.Quizzes.AddRange(quiz1, quiz2, quiz3, quiz4, quiz5, quiz6, quiz7, quiz8, quiz9, quiz10);
            await context.SaveChangesAsync();
        }
    }
}

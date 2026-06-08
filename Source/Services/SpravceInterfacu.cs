using Zakázkovna.Models;
using Zakázkovna.Utils;

namespace Zakázkovna.Services
{
    // Třída zodpovídá za veškeré zobrazování a získávání vstupu od uživatele
    // Neobsahuje žádnou business logiku – pouze komunikuje s uživatelem
    public class SpravceInterfacu
    {
        public ConsoleColor BarvaAplikace { get; set; } =  ConsoleColor.Yellow;
        // Reference na globální konfiguraci (zajišťuje konzistentní přístup k aktuálnímu jazyku a slovníkům)
        private readonly SpravceKonfigurace SK;
        
        // Konstruktor využívající Dependency Injection (předání závislosti pro lepší správu stavu a paměti)
        public SpravceInterfacu(SpravceKonfigurace konfigurace)
        {
            SK = konfigurace;
        }
        
        // Vymaže konzoli a zobrazí hlavní nabídku programu
        public void ZobrazitHlavniMenu()
        {
            Console.Clear();
            ZobrazitZahlaviSekce(ZiskatPreklad("hlavniMenu"));
            
            // Vykreslení položek hlavního menu s využitím dynamického překladu
            ZobrazitTematickouZpravu(ZiskatPreklad("uvitani"));
            ZobrazitTematickouZpravu(ZiskatPreklad("vyberteMenu"));

            Console.WriteLine(" 1. " + ZiskatPreklad("pridatZakazku"));
            Console.WriteLine(" 2. " + ZiskatPreklad("upravitZakazku"));
            Console.WriteLine(" 3. " + ZiskatPreklad("smazatZakazku"));
            Console.WriteLine(" 4. " + ZiskatPreklad("zobrazitZakazky"));
            Console.WriteLine(" 5. " + ZiskatPreklad("vyhledatZakazku"));
            Console.WriteLine(" 6. " + ZiskatPreklad("prehledAAnalyza"));
            Console.WriteLine(" 7. " + ZiskatPreklad("oAplikaci"));
            Console.WriteLine(" 8. " + ZiskatPreklad("oAutorovi"));
            Console.WriteLine(" 9. " + ZiskatPreklad("nastaveniAProfil"));
            Console.WriteLine(" 0. " + ZiskatPreklad("uzavrit"));
        }


        // Přečte volbu z hlavního menu (číslo 0–9)
        public byte ZiskatVolbuMenu()
        {
            return ZiskatVolbu(ZiskatPreklad("vaseVolba"), 0, 9);
        }

        // Zobrazí formulář pro zadání nové zakázky a vrátí vyplněný objekt
        public Zakazka ZobrazitFormularNoveZakazky(List<Zakazka> seznamZakazek)
        {
            string noveID = SpravceID.GenerovatUnikatniID(seznamZakazek);
            string nazev = ZiskatText("\n Jak se jmenuje zakázka?", 3, 50);
            string popis = ZiskatText("\n Popište zakázku", 10, 1000);
            double celkovaCena = ZiskatCislo("\n Kolik je celková cena zakázky?", 1, 1000000);
            DateTime deadline = ZiskatDatum("\n Termín odevzdání");

            Zakazka novaZakazka = new Zakazka(noveID, nazev, popis, celkovaCena, deadline);
            return novaZakazka;
        }


        // Zobrazí výsledek vyhledávání – buď chybovou zprávu, nebo tabulku s nalezenou zakázkou
        public void ZobrazitHledanouZakazku(Zakazka nalezenaZakazka)
        {
            ZobrazitZahlaviSekce("Vyhledat zakázku");

            if (nalezenaZakazka == null)
            {
                ZobrazitChybovouZpravu("\n [!] Zakázka s tímto ID nebyla nalezena");
            }
            else
            {
                ZobrazitUspesnouZpravu("\n [✓] Zakázka nalezena: \n");
                // Zabalíme jednu zakázku do seznamu, protože VykreslitTabulku bere List
                VykreslitTabulku(new List<Zakazka> { nalezenaZakazka });
            }
        }


        // Zobrazí všechny zakázky v tabulce, nebo hlášku pokud je seznam prázdný
        public void ZobrazitVsechnyZakazky(List<Zakazka> seznamZakazek)
        {
            if (seznamZakazek.Count == 0)
            {
                ZobrazitChybovouZpravu(" [!] Seznam je prázdný. Žádné zakázky k zobrazení");
            }
            else
            {
                VykreslitTabulku(seznamZakazek);
            }
        }


        // Zobrazí potvrzovací dialog před smazáním – vrátí true pokud uživatel potvrdí
        public bool ZobrazitDialogSmazani(Zakazka nalezenaZakazka)
        {
            if (nalezenaZakazka == null)
            {
                ZobrazitChybovouZpravu(" [!] Zakázka s tímto ID nebyla nalezena");
                return false;
            }

            Console.WriteLine("\n Nalezena zakázka ke smazání:");
            VykreslitTabulku(new List<Zakazka> { nalezenaZakazka });

            return ZiskatPotvrzeniProSmazani($"Opravdu chcete TRVALE SMAZAT tuto zakázku?");
        }


        // Přečte odpověď A/N od uživatele – vrátí true pouze pro "A" (ano)
        public bool ZiskatPotvrzeniProSmazani(string zprava)
        {
            Console.Write($"\n [?] {zprava} [A/N]: ");

            // "??" je null-koalescenční operátor – pokud ReadLine() vrátí null, použijeme prázdný řetězec
            string odpoved = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
            return odpoved == "A";
        }


        // Vykreslí záhlaví sekce – orámování s názvem stránky
        public void ZobrazitZahlaviSekce(string nazevSekce)
        {
            Console.Clear();
            Console.ForegroundColor = BarvaAplikace;
            Console.WriteLine("======================================");
            Console.WriteLine($"  Zakázkovna | {nazevSekce}");
            Console.WriteLine("======================================\n");
            Console.ResetColor();
        }

        public void ZobrazitOAplikaci()
        {
            ZobrazitTematickouZpravu("  Zakázkovna? Co to je? \n");
            Console.WriteLine(" Konzolová aplikace typu CRM/ERP určená pro freelancery a malé firmy.");
            Console.WriteLine(" Systém umožňuje efektivní evidenci projektů, automatizaci, \n administrativních procesů a hloubkovou analýzu obchodních výsledků.");

            ZobrazitTematickouZpravu("\n  Co Zakázkovna umí? \n");
            Console.WriteLine(" * 1. Evidence - vše se ukládá do CSV, které otevřete i v Excelu.");
            Console.WriteLine(" * 2. Pořádek - automatické číslování zakázek (ID 1001, 1002...).");
            Console.WriteLine(" * 3. Kontrola - validace vstupů nepustí chyby do vaší databáze.");
            Console.WriteLine(" * 4. Manipulace - rychlé vyhledávání a bezpečné mazání s potvrzením.");
            Console.WriteLine(" * 5. Přehled - automatický součet zisku ze všech vašich projektů.");
        }

        public void ZobrazitOAutorovi()
        {
            Console.WriteLine("  Název projektu       Zakázkovna v1.0");
            Console.WriteLine("  Autor                Ariet Muzirapov");
            Console.WriteLine("  Obor                 Informatika (PEF ČZU)");
            Console.WriteLine("  Předmět              Programování (ETE15E, LS 2025-2026)");

            Console.WriteLine("\n  © 2026 Ariet Muzirapov");
            Console.WriteLine("  Všechna práva vyhrazena. Tento software byl vytvořen jako semestrální projekt pro účely studia na ČZU.");
        }


        // Zobrazí aktuální hodnoty profilu a nastavení
        public void ZobrazitNastaveniAProfil(Profil p, Nastaveni n)
        {
            ZobrazitInformacniZpravu(" [!] POZNÁMKA: Tato nastavení jsou aktuálně ve fázi vývoje.");
            ZobrazitInformacniZpravu("     Změny se uloží, ale zatím neovlivňují některé funkce.");

            Console.WriteLine("\n [ PROFIL UŽIVATELE ] \n");
            Console.WriteLine($" 1.Jméno:       {p.Jmeno}");
            Console.WriteLine($" 2.Email:       {p.Email}");
            Console.WriteLine($" 3.Měsíční cíl: {p.MesicniCil}");

            Console.WriteLine("\n [ SYSTÉMOVÉ NASTAVENÍ ] \n");
            ZobrazitMoznosti("4.Jazyk", new[] { "Čeština", "English", "Кыргызча", "Русский" }, n.Jazyk);
            ZobrazitMoznosti("5.Téma", new[] { "Žlutá", "Zelená", "Azurová" }, n.Tema);
        }


        // Smyčka pro správu nastavení – uživatel může měnit položky, dokud nezvolí 0 (návrat)
        public void SpravovatNastaveniAProfil(Profil p, Nastaveni n)
        {
            while (true)
            {
                ZobrazitNastaveniAProfil(p, n);

                // Rozsah 0–5: číslo 0 znamená návrat, čísla 1–5 odpovídají jednotlivým položkám v menu
                byte volba = ZiskatVolbu("\n Zadejte číslo pro úpravu (nebo 0 pro návrat): ", 0, 5);

                switch (volba)
                {
                    case 1:
                        p.Jmeno = ZiskatText("\n Zadejte nové jméno", 3, 30);
                        ZobrazitUspesnouZpravu("\n [✓] Jméno aktualizováno!");
                        CekatNaNavrat(); break;
                    case 2:
                        p.Email = ZiskatText("\n Zadejte nový email", 5, 50);
                        ZobrazitUspesnouZpravu("\n [✓] Email aktualizován!");
                        CekatNaNavrat(); break;
                    case 3:
                        p.MesicniCil = ZiskatCislo("\n Zadejte nový měsíční cíl", 0, 1000000);
                        ZobrazitUspesnouZpravu("\n [✓] Měsíční cíl aktualizován!");
                        CekatNaNavrat(); break;
                    case 4:
                        n.Jazyk = VybratZMoznosti("\n Vyberte jazyk", new[] { "Čeština", "English", "Кыргызча", "Русский" });
                        ZobrazitUspesnouZpravu($"\n [✓] Jazyk byl změněn na {n.Jazyk}!");
                        CekatNaNavrat(); break;
                    case 5:
                        n.Tema = VybratZMoznosti("\n Vyberte barvu", new[] { "Žlutá", "Zelená", "Azurová" });
                        ZobrazitUspesnouZpravu($"\n [✓] Barva byla změněna na {n.Tema}!");
                        CekatNaNavrat(); break;
                    case 0:
                        return; // Návrat do hlavního menu
                }
            }
        }


        // Zobrazí očíslovaný seznam možností a nechá uživatele vybrat jednu
        private string VybratZMoznosti(string titulek, string[] moznosti)
        {
            Console.WriteLine($"\n {titulek}: \n");
            for (int i = 0; i < moznosti.Length; i++)
            {
                Console.WriteLine($" {i + 1}. {moznosti[i]}");
            }

            byte volba = ZiskatVolbu("\n Vaše volba: ", 1, (byte)moznosti.Length);
            // Odečteme 1, protože pole začíná od indexu 0, ale menu od čísla 1
            return moznosti[volba - 1];
        }


        // Zobrazí výsledky analýzy – celkovou hodnotu, procento plnění a počet zakázek
        public void ZobrazitPrehled(string jmeno, DataPrehledu d)
        {
            ZobrazitUspesnouZpravu($" Vítejte zpět, {jmeno}! \n\n");

            Console.WriteLine($" - Celková hodnota:   {d.CelkovaHodnota}");
            // Math.Round zaokrouhlí na 1 desetinné místo, aby výsledek vypadal přehledně
            Console.WriteLine($" - Plnění plánu:      {Math.Round(d.ProcentoPlneni, 1)} %");
            Console.WriteLine($" - Počet zakázek:     {d.PocetZakazek}");
        }


        // Zobrazí rozlučkovou zprávu při ukončení programu
        public void ZobrazitRozluckovouZpravu()
        {
            Console.Clear();
            Console.ForegroundColor = BarvaAplikace;
            Console.WriteLine("==============================================");
            Console.WriteLine("  Těším se na příští setkání. Na shledanou!   ");
            Console.WriteLine("==============================================");
            Console.ResetColor();
        }


        // Pozastaví program a čeká, dokud uživatel nestiskne libovolnou klávesu
        // Je statická – lze volat bez instance: SpravceInterfacu.CekatNaNavrat()
        public static void CekatNaNavrat()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\n Stiskněte libovolnou klávesu pro pokračování...");
            Console.ReadKey();
        }

        // ----- Pomocné metody pro barevný výstup -----

        public void ZobrazitChybovouZpravu(string zprava)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(zprava);
            Console.ResetColor();
        }

        public void ZobrazitUspesnouZpravu(string zprava)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(zprava);
            Console.ResetColor();
        }

        public void ZobrazitInformacniZpravu(string zprava)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(zprava);
            Console.ResetColor();
        }
        
        public void ZobrazitTematickouZpravu(string zprava)
        {
            // Použíje globální barvu, kterou jsme nastavili v Program.cs
            Console.ForegroundColor = BarvaAplikace;
            Console.WriteLine(zprava);
            Console.ResetColor();
        }


        // Zobrazí řadu možností vedle sebe a zvýrazní zeleně tu, která je aktuálně vybrána
        private void ZobrazitMoznosti(string label, string[] moznosti, string aktivni)
        {
            Console.Write($" {label}:  ");
            foreach (var moznost in moznosti)
            {
                if (moznost == aktivni)
                {
                    // Aktivní možnost je zelená a v hranatých závorkách – jasně viditelná
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" [{moznost}] ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($" {moznost} ");
                }
            }
            Console.WriteLine();
        }

        // Opakovaně čte vstup, dokud uživatel nezadá platné číslo v rozsahu min–max
        private byte ZiskatVolbu(string dotaz, byte min, byte max)
        {
            while (true)
            {
                Console.Write(dotaz);
                string vstup = Console.ReadLine()?.Trim() ?? string.Empty;

                if (Validace.JeVolbaValidni(vstup, out byte volba) && volba >= min && volba <= max)
                {
                    return volba;
                }
                ZobrazitChybovouZpravu($"\n [!] Neplatná volba, prosím zkuste znovu");
            }
        }

        // Opakovaně čte vstup, dokud uživatel nezadá text správné délky
        private string ZiskatText(string dotaz, int min, int max)
        {
            while (true)
            {
                Console.Write($"{dotaz} [{min}-{max} znaků]: ");
                string vstup = Console.ReadLine();

                if (Validace.JeTextValidni(vstup, min, max))
                {
                    return vstup?.Trim() ?? string.Empty;
                }
                ZobrazitChybovouZpravu($"\n [!] Chyba: text musí mít {min}-{max} znaků");
            }
        }

        // Opakovaně čte vstup, dokud uživatel nezadá číslo v povoleném rozsahu
        private double ZiskatCislo(string dotaz, double min, double max)
        {
            while (true)
            {
                Console.Write($"{dotaz} [{min}-{max}]: ");
                string vstup = Console.ReadLine();

                if (Validace.JeCisloValidni(vstup, min, max, out double vysledek))
                {
                    return vysledek;
                }
                ZobrazitChybovouZpravu($"\n [!] Chyba: číslo musí být v rozsahu od {min} do {max}");
            }
        }

        // Opakovaně čte vstup, dokud uživatel nezadá datum ve správném formátu a v budoucnosti
        private DateTime ZiskatDatum(string dotaz)
        {
            while (true)
            {
                Console.Write($"{dotaz} [DD.MM.YYYY]: ");
                string vstup = Console.ReadLine();

                if (Validace.JeDatumValidni(vstup, out DateTime datum))
                {
                    return datum;
                }
                ZobrazitChybovouZpravu("\n [!] Chyba: datum musí být ve formátu DD.MM.YYYY a nesmí být v minulosti");
            }
        }

        // Opakovaně čte vstup, dokud uživatel nezadá platné čtyřmístné ID
        public string ZiskatIDProHledani()
        {
            while (true)
            {
                Console.Write("\n Zadejte ID zakázky: ");
                string vstup = Console.ReadLine()?.Trim() ?? string.Empty;

                if (Validace.JeIDValidni(vstup, out string vysledek))
                {
                    return vysledek;
                }
                ZobrazitChybovouZpravu("\n [!] Chyba: ID musí být čtyřmístné číslo [1000-9999]");
            }
        }


        // Vykreslí tabulku se zakázkami – ořezává dlouhé texty, aby se vše vešlo do konzole
        private void VykreslitTabulku(List<Zakazka> seznamZakazek)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| {0,-4} | {1,-20} | {2,-12} | {3,-10} | {4,-50} |", "ID", "Název", "Celková cena", "Deadline", "Popis");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");

            foreach (var z in seznamZakazek)
            {
                Console.WriteLine("| {0,-4} | {1,-20} | {2,-12} | {3,-10} | {4,-50} |",
                    z.ID,
                    // Pokud je název delší než 20 znaků, zkrátíme ho a přidáme "..."
                    z.Nazev.Length > 20 ? z.Nazev.Substring(0, 17) + "..." : z.Nazev,
                    z.CelkovaCena,
                    z.Deadline.ToString("dd.MM.yyyy"),
                    // Stejný princip pro popis – max 50 znaků v tabulce
                    z.Popis.Length > 50 ? z.Popis.Substring(0, 47) + "..." : z.Popis);
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }
        
        // Pomocná metoda pro získání překladu textu podle aktuálně zvoleného jazyka
        public string ZiskatPreklad(string klic)
        {
            // Načtení aktuálního jazyka z konfigurace
            string jazyk = SK.Nastaveni.Jazyk; 

            // Kontrola, zda existuje slovník pro daný jazyk a zda obsahuje požadovaný klíč
            if (SK.Slovniky.ContainsKey(jazyk) && SK.Slovniky[jazyk].ContainsKey(klic))
            {
                return SK.Slovniky[jazyk][klic]; // Vrátí přeložený text z JSONu
            }

            // Pokud překlad nebo jazyk chybí, vrátí se samotný klíč jako fallback
            return $"[{klic}]"; 
        }
    }
}

using System;
using System.Text.Json;
using Zakázkovna.Models;

namespace Zakázkovna.Services
{
    public class SpravceKonfigurace
    {
        // V debug módu hledáme JSON soubory v adresáři projektu, v release verzi přímo v adresáři s .exe
        #if DEBUG
            private readonly string cestaProfil = "../../../Database/profil.json";
            private readonly string cestaNastaveni = "../../../Database/nastaveni.json";
        #else
        private readonly string cestaProfil = "Database/profil.json";
            private readonly string cestaNastaveni = "Database/nastaveni.json";
        #endif


        // Veřejné vlastnosti – SpravceInterfacu a SpravceAnalyzy je mohou číst, ale ne přepisovat
        public Profil Profil { get; private set; }
        public Nastaveni Nastaveni { get; private set; }


        // Konstruktor – při vytvoření objektu hned načteme data ze souborů
        public SpravceKonfigurace()
        {
            Profil = Nacist<Profil>(cestaProfil);
            Nastaveni = Nacist<Nastaveni>(cestaNastaveni);
        }


        // Uloží aktuální stav profilu i nastavení do jejich JSON souborů
        public void UlozitVsechnyZmeny()
        {
            Ulozit(Profil, cestaProfil);
            Ulozit(Nastaveni, cestaNastaveni);
        }


        // Generická metoda pro načtení libovolného objektu z JSON souboru
        // "where T : new()" znamená, že T musí mít bezparametrický konstruktor
        // – potřebujeme ho pro vytvoření výchozích hodnot, pokud soubor neexistuje
        private T Nacist<T>(string nazevSouboru) where T : new()
        {
            // Kontrolujeme, jestli soubor existuje – pokud ne, vrátíme nový objekt
            if (!File.Exists(nazevSouboru)) return new T();

            // Kontrolujeme, jestli soubor není prázdný – pokud ano, vrátíme nový objekt
            string json = File.ReadAllText(nazevSouboru);
            if (string.IsNullOrWhiteSpace(json)) return new T();

            try
            {
                return JsonSerializer.Deserialize<T>(json) ?? new T();
            }
            catch (JsonException)
            {
                // Pokud JSON není validní, vrátíme nový objekt – tím se vyhneme pádům aplikace kvůli poškozeným souborům
                return new T();
            }
        }


        // Generická metoda pro uložení libovolného objektu do JSON souboru
        private void Ulozit<T>(T data, string nazevSouboru)
        {
            string adresar = Path.GetDirectoryName(nazevSouboru) ?? string.Empty;

            // Pokud adresář neexistuje, vytvoříme ho – tím zajistíme, že ukládání nebude selhávat kvůli chybějícím složkám
            if (!string.IsNullOrEmpty(adresar) && !Directory.Exists(adresar))
            {
                Directory.CreateDirectory(adresar);
            }

            // WriteIndented = true znamená, že JSON bude čitelně naformátovaný (s odsazením)
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(nazevSouboru, json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Zakázkovna.Models;

namespace Zakázkovna.Services
{
    public class SpravceZakazek
    {
        // Interní seznam zakázek – je soukromý, přístup zvenku je jen přes vlastnost níže
        private List<Zakazka> seznamZakazek = new List<Zakazka>();

        // Veřejná vlastnost pro čtení seznamu – ostatní třídy mohou seznam vidět, ale ne přímo měnit
        public List<Zakazka> SeznamZakazek { get { return seznamZakazek; } }


        // Cesta k CSV souboru se liší podle toho, zda spouštíme program z Visual Studia (DEBUG)
        // nebo jako hotovou aplikaci (RELEASE) – aby soubor byl vždy na správném místě
        #if DEBUG
            private readonly string cestaCsvSouboru = "../../../Database/seznamZakazek.csv";
        #else
            private readonly string cestaCsvSouboru = "Database/seznamZakazek.csv";
        #endif


        // Uloží celý seznam zakázek do CSV – přepíše soubor od začátku (append: false)
        private void UlozitDoCsv()
        {
            // Pokud složka Database ještě neexistuje, vytvoříme ji (důležité při prvním spuštění)
            string? adresar = Path.GetDirectoryName(cestaCsvSouboru);
            if (!string.IsNullOrEmpty(adresar) && !Directory.Exists(adresar))
            {
                Directory.CreateDirectory(adresar);
            }

            // "using" zajistí, že se soubor správně zavře i v případě chyby
            using (StreamWriter sw = new StreamWriter(cestaCsvSouboru, false, Encoding.UTF8))
            {
                foreach (var z in seznamZakazek)
                {
                    // Sloupce jsou odděleny středníkem – formát: ID;Název;Popis;Cena;Deadline
                    string radek = $"{z.ID};{z.Nazev};{z.Popis};{z.CelkovaCena};{z.Deadline:dd.MM.yyyy}";
                    sw.WriteLine(radek);
                }
            }
        }

        // Přidá zakázku do seznamu a okamžitě uloží změny do souboru
        public void PridatZakazku(Zakazka novaZakazka)
        {
            seznamZakazek.Add(novaZakazka);
            UlozitDoCsv();
        }

        // Načte všechny zakázky z CSV souboru do paměti (volá se při startu programu)
        public void NacistZCsv()
        {
            // Pokud soubor ještě neexistuje, není co načítat – klidně skončíme
            if (!File.Exists(cestaCsvSouboru)) return;

            seznamZakazek.Clear();
            string[] radky = File.ReadAllLines(cestaCsvSouboru, Encoding.UTF8);

            foreach (string radek in radky)
            {
                string[] casti = radek.Split(';');

                // Přeskočíme řádky, které nemají přesně 5 sloupců (poškozená nebo prázdná data)
                if (casti.Length == 5)
                {
                    string ID = casti[0];
                    string nazev = casti[1];
                    string popis = casti[2];

                    // TryParse místo Parse – pokud číslo nelze převést, použijeme 0 (program nespadne)
                    double celkovaHodnota = double.TryParse(casti[3], out double result) ? result : 0;

                    // InvariantCulture zajistí, že datum se správně přečte na všech počítačích
                    // (různé systémy mohou mít různé formáty data)
                    DateTime deadline = DateTime.ParseExact(casti[4], "dd.MM.yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None);

                    seznamZakazek.Add(new Zakazka(ID, nazev, popis, celkovaHodnota, deadline));
                }
            }
        }

        // Odstraní zakázku ze seznamu a uloží změny – vrátí true pokud se povedlo
        public bool SmazatZakazku(Zakazka zakazkaKeZmazani)
        {
            if (zakazkaKeZmazani != null)
            {
                seznamZakazek.Remove(zakazkaKeZmazani);
                UlozitDoCsv();
                return true;
            }
            return false;
        }

        // Najde zakázku podle ID pomocí LINQ – vrátí null, pokud zakázka neexistuje
        public Zakazka VyhledatZakazkuPodleID(string id)
        {
            return seznamZakazek.FirstOrDefault(z => z.ID == id);
        }
    }
}

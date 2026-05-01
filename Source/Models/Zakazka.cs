using System;

namespace Zakázkovna.Models
{
    // Datový model jedné zakázky – uchovává všechny informace o projektu
    public class Zakazka
    {
        // Vlastnosti jsou jen pro čtení (get bez set) – po vytvoření je nelze změnit
        // To zajišťuje, že data zakázky zůstanou vždy konzistentní
        public string ID { get; }
        public string Nazev { get; }
        public string Popis { get; }
        public double CelkovaCena { get; }
        public DateTime Deadline { get; }

        // Konstruktor – jediný způsob, jak vytvořit zakázku (všechna pole jsou povinná)
        public Zakazka(string id, string nazev, string popis, double celkovaCena, DateTime deadline)
        {
            ID = id;
            Nazev = nazev;
            Popis = popis;
            CelkovaCena = celkovaCena;
            Deadline = deadline;
        }
    }
}

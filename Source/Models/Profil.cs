namespace Zakázkovna.Models
{
    // Profil uživatele – ukládá se do souboru profil.json
    // Výchozí hodnoty se použijí při prvním spuštění, kdy soubor ještě neexistuje
    public class Profil
    {
        public string Jmeno { get; set; } = "[NENÍ NASTAVENO]";
        public string Email { get; set; } = "[NENÍ NASTAVENO]";
        public double MesicniCil { get; set; } = 0;  // cíl v Kč, používá se pro výpočet % plnění
    }
}

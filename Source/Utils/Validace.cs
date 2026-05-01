using System;
using System.Globalization;

namespace Zakázkovna.Utils
{
    // Statická třída pro ověřování vstupů od uživatele
    // Všechny metody používají vzor "TryParse" – vracejí bool a výsledek předávají přes "out" parametr
    public static class Validace
    {
        // Ověří, zda vstup lze převést na číslo typu byte (0–255) – pro volbu v menu
        public static bool JeVolbaValidni(string textDotazu, out byte vysledek)
        {
            return byte.TryParse(textDotazu, out vysledek);
        }


        // Ověří, zda text není prázdný a jeho délka je v povoleném rozsahu
        public static bool JeTextValidni(string vstup, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(vstup))
            {
                return false;
            }
            int delka = vstup.Trim().Length;
            return delka >= min && delka <= max;
        }


        // Ověří, zda vstup lze převést na desetinné číslo a zda je v povoleném rozsahu
        public static bool JeCisloValidni(string vstup, double min, double max, out double vysledek)
        {
            return double.TryParse(vstup, out vysledek) && vysledek >= min && vysledek <= max;
        }


        // Ověří formát data a zároveň zkontroluje, že deadline není v minulosti
        public static bool JeDatumValidni(string vstup, out DateTime datum)
        {
            // TryParseExact kontroluje přesný formát "DD.MM.YYYY"
            // InvariantCulture zajistí stejné chování na počítačích s různým nastavením jazyka
            bool validniFormat = DateTime.TryParseExact(
                vstup, "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out datum);

            // Datum musí mít správný formát A nesmí být v minulosti
            return validniFormat && datum >= DateTime.Today;
        }


        // Ověří, zda vstup je čtyřmístné číslo v rozsahu 1000–9999 (formát ID zakázek)
        public static bool JeIDValidni(string vstup, out string vysledek)
        {
            vysledek = null;
            if (!string.IsNullOrWhiteSpace(vstup)
                && int.TryParse(vstup, out int cislo)
                && cislo >= 1000 && cislo <= 9999)
            {
                vysledek = vstup;
                return true;
            }
            return false;
        }
    }
}

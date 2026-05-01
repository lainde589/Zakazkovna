using System;

namespace Zakázkovna.Models
{
    // Systémová nastavení aplikace – ukládají se do souboru nastaveni.json
    public class Nastaveni
    {
        public string Jazyk { get; set; } = "Čeština"; // výchozí jazyk rozhraní
        public string Mena { get; set; } = "CZK"; // výchozí měna pro zobrazení cen
    }
}

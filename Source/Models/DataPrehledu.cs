namespace Zakázkovna.Models
{
    // Přenosový objekt pro výsledky analýzy – používá se pouze pro předání dat do UI
    // SpravceAnalyzy ho naplní hodnotami, SpravceInterfacu ho zobrazí
    public class DataPrehledu
    {
        public double CelkovaHodnota { get; set; }   // součet cen všech zakázek
        public double ProcentoPlneni { get; set; }   // kolik % měsíčního cíle je splněno
        public int PocetZakazek { get; set; }        // celkový počet zakázek v seznamu
    }
}

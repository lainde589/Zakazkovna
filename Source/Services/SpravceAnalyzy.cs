using System.Collections.Generic;
using System.Linq;
using Zakázkovna.Models;

namespace Zakázkovna.Services
{
    // Třída zodpovídá pouze za výpočty – žádné zobrazování ani práce se soubory zde nejsou
    public class SpravceAnalyzy
    {
        // Sečte ceny všech zakázek pomocí LINQ metody Sum
        private double SpocitatCelkovouHodnotu(List<Zakazka> zakazky)
        {
            return zakazky.Sum(z => z.CelkovaCena);
        }

        // Vypočítá, kolik procent měsíčního cíle bylo dosaženo
        private double SpocitatProcentoPlneni(double celkovaHodnota, double mesicniCil)
        {
            // Ochrana před dělením nulou – pokud cíl není nastaven, vrátíme 0 %
            if (mesicniCil <= 0) return 0;
            return (celkovaHodnota / mesicniCil) * 100;
        }

        // Hlavní metoda – vypočítá všechny ukazatele a vrátí je zabalené v objektu DataPrehledu
        public DataPrehledu SpocitatUkazatele(List<Zakazka> zakazky, Profil p)
        {
            double celkovaHodnota = SpocitatCelkovouHodnotu(zakazky);
            double procentoPlneni = SpocitatProcentoPlneni(celkovaHodnota, p.MesicniCil);

            return new DataPrehledu
            {
                CelkovaHodnota = celkovaHodnota,
                PocetZakazek = zakazky.Count,
                ProcentoPlneni = procentoPlneni
            };
        }
    }
}

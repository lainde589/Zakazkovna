using Zakázkovna.Models;

namespace Zakázkovna.Utils
{
    // Pomocná třída pro generování ID zakázek
    // Metoda je statická – není potřeba vytvářet instanci třídy, stačí zavolat SpravceID.GenerovatUnikatniID(...)
    public class SpravceID
    {
        public static string GenerovatUnikatniID(List<Zakazka> seznam)
        {
            // Pokud ještě žádné zakázky nejsou, začínáme od 1001
            if (seznam == null || seznam.Count == 0)
            {
                return "1001";
            }

            // Najdeme nejvyšší existující ID a přičteme 1
            // LINQ metoda Max() projde celý seznam a vrátí největší hodnotu
            int maxId = seznam.Max(z => int.Parse(z.ID));

            return (maxId + 1).ToString();
        }
    }
}

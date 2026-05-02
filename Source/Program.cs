using Zakázkovna.Models;
using Zakázkovna.Services;

namespace Zakázkovna
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Vytvoříme instance všech správců – každý má svou odpovědnost
            SpravceZakazek SZ = new SpravceZakazek();       // správa zakázek (CRUD + CSV)
            SpravceInterfacu SI = new SpravceInterfacu();   // zobrazení a vstup od uživatele
            SpravceKonfigurace SK = new SpravceKonfigurace(); // načtení profilu a nastavení z JSON
            SpravceAnalyzy SA = new SpravceAnalyzy();       // výpočet KPI ukazatelů

            // Načteme zakázky z CSV souboru hned při startu programu
            SZ.NacistZCsv();

            bool beziProgram = true;

            // Hlavní smyčka programu – běží, dokud uživatel nezvolí "Uzavřít" (0)
            while (beziProgram)
            {
                SI.ZobrazitHlavniMenu();
                byte volba = SI.ZiskatVolbuMenu();

                switch (volba)
                {
                    case 1:
                        SI.ZobrazitZahlaviSekce("Přidání nové zakázky");
                        // Vygenerujeme unikátní ID pro novou zakázku
                        Zakazka novaZakazka = SI.ZobrazitFormularNoveZakazky(SZ.SeznamZakazek);

                        // Formulář vrátí null pouze tehdy, když uživatel zadá neplatné hodnoty
                        if (novaZakazka != null)
                        {
                            SZ.PridatZakazku(novaZakazka);
                            SI.ZobrazitUspesnouZpravu($"\n Zakázka uložena pod ID: {novaZakazka.ID}");
                        }
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 2:
                        SI.ZobrazitZahlaviSekce("Úprava zakázky");
                        SI.ZobrazitInformacniZpravu(" [i] Sekce 'Úprava zakázky' je momentálně ve vývoji (plánováno pro verzi 2.0).");
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 3:
                        SI.ZobrazitZahlaviSekce("Smazání zakázky");
                        string IDZakazkyKeZmazani = SI.ZiskatIDProHledani();
                        Zakazka ZakazkaKeZmazani = SZ.VyhledatZakazkuPodleID(IDZakazkyKeZmazani);

                        if (ZakazkaKeZmazani == null)
                        {
                            SI.ZobrazitChybovouZpravu($"\n [!] Zakázka s ID {IDZakazkyKeZmazani} nebyla nalezena.");
                        }
                        else
                        {
                            // Zobrazíme potvrzovací dialog, než zakázku trvale smažeme
                            if (SI.ZobrazitDialogSmazani(ZakazkaKeZmazani))
                            {
                                SZ.SmazatZakazku(ZakazkaKeZmazani);
                                SI.ZobrazitUspesnouZpravu($"\n [✓] Zakázka s ID {IDZakazkyKeZmazani} byla úspěšně smazána.");
                            }
                            else
                            {
                                SI.ZobrazitChybovouZpravu($"\n [!] Smazání zakázky s ID {IDZakazkyKeZmazani} bylo zrušeno.");
                            }
                        }
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 4:
                        SI.ZobrazitZahlaviSekce("Seznam všech zakázek");
                        SI.ZobrazitVsechnyZakazky(SZ.SeznamZakazek);
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 5:
                        SI.ZobrazitZahlaviSekce("Vyhledávání zakázky");
                        string hledaneID = SI.ZiskatIDProHledani();
                        Zakazka nalezenaZakazka = SZ.VyhledatZakazkuPodleID(hledaneID);
                        SI.ZobrazitHledanouZakazku(nalezenaZakazka);
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 6:
                        // Nejdřív spočítáme data, pak je předáme do UI k zobrazení
                        var data = SA.SpocitatUkazatele(SZ.SeznamZakazek, SK.Profil);
                        SI.ZobrazitPrehled(SK.Profil.Jmeno, data, SK.Nastaveni.Mena);
                        SpravceInterfacu.CekatNaNavrat(); break;

                    case 7:
                        SI.ZobrazitZahlaviSekce("O aplikaci");
                        SI.ZobrazitOAplikaci();
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 8:
                        SI.ZobrazitZahlaviSekce("O autorovi");
                        SI.ZobrazitOAutorovi();
                        SpravceInterfacu.CekatNaNavrat(); break;


                    case 9:
                        // Po návratu z nastavení hned uložíme případné změny do JSON
                        SI.SpravovatNastaveniAProfil(SK.Profil, SK.Nastaveni);
                        SK.UlozitVsechnyZmeny();
                        break;


                    case 0:
                        beziProgram = false;
                        SI.ZobrazitRozluckovouZpravu();
                        SpravceInterfacu.CekatNaNavrat(); break;


                    default:
                        SI.ZobrazitChybovouZpravu("\n [!]Neplatný vstup! Prosím zkuste znovu.");
                        SI.ZobrazitHlavniMenu();
                        SpravceInterfacu.CekatNaNavrat(); break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;

class Program
{

    public static List<Databaze> HledaniZanru(List<Databaze> list, string vstup)  //funkce hledaniZanru má funkci aby mohl uživatel hledat v csv souboru filmy/serialy kde obsahuje daný žárn co uživatel chce vidět
    {
        Console.Clear();

        int i = 1;

        var vysledek = from str in list                                             //řádek 18 až 20 se ptáme souboru jestli obsahuje žarn, pokud ano uloží se nám to do "vysledek"
                       where str.Genre.Contains(vstup)
                       select str;
        if (vysledek.Any())
        {
            foreach (var v in vysledek)                                                 //řádek 22 až 27 nám napíše do konzole všechny nalezený filmy/serialy které obsahují ten žanr, který chce uživatel
            {
                string text = i++ + ". " + v.Title.PadRight(50) + "    " + (v.IMBD_Rating + @"*").PadRight(20) + "     " + v.Genre.PadRight(20);
                Console.WriteLine(text);
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Nenalezeno");
            

        }
        Console.ReadKey();

        return vysledek.ToList<Databaze>();                                         
        

    }

    public static List<Databaze> PodleAbecedy(List<Databaze> list, string vstup)
    {

        int i = 1;

        var vysledek = from str in list                                         //řádek 32 až 42 se ptáme souboru jsou tam jsou film/serialy začínající na písmenko, pokud ano uloží se nám to do "vysledek"
                       where str.Title.StartsWith(vstup)
                       select str;
        if (vysledek.Any())
        {
            foreach (var v in vysledek)                                             //řádek 44 až 48 nám napíše do konzole všechny nalezený filmy/serialy které začínají na písmenko, který chce uživatel
            {

                string text = i++ + ". " + v.Title.PadRight(50) + "    " + (v.IMBD_Rating + @"*").PadRight(20) + "     " + v.Genre.PadRight(20);
                Console.WriteLine(text);

            }
        }
        else
        {
            Console.WriteLine("Nenalezeno");
        }
        Console.ReadKey();
        
        return vysledek.ToList<Databaze>();
                
                  
    }


    public static List<Databaze> HledaniNazev(List<Databaze> list, string vstup)
    {
        int i = 1;

        var vysledek = from str in list                         //řádek 58 až 60 se ptáme souboru jestli obsahuje název, který zadal uživatel, pokud ano uloží se nám to do "vysledek"
                       where str.Title.Contains(vstup)
                       select str;
        if (vysledek.Any())
        {
            foreach (var v in vysledek)                            //řádek 62 až 66 nám napíše do konzole všechny nalezený filmy/serialy které mají ten nazev, který chce uživatel
            {
                string text = i++ + ". " + v.Title.PadRight(50) + "    " + (v.IMBD_Rating + @"*").PadRight(20) + "     " + v.Genre.PadRight(20);
                Console.WriteLine(text);
            }
        }
        else
        {
            Console.WriteLine("Nenalezeno");
        }
        Console.ReadKey();
        return vysledek.ToList<Databaze>();
    }

    public static string Kontroluj(String nazev)                //funkce kontroluj slouží k tomu, aby kontoloval, jestliže uživatel nic nezadal do konzole tak má dát "" a  pokud uživatel něco zadal tak má ze stringu udělat char aby mohl každý písmenko dát do polích a změnít první písmenko na větší písmenko a ostatní na malý a dát to zpatky do stingu a pokud tam bude mezera tak se použije trim() kde odtraňuje zbytečné mezery
    {
        if (!(string.IsNullOrEmpty(nazev))) 
        {
            char[] ch = new char[nazev.Length];
            for (int i = 0; i < nazev.Length; i++)
            {
                ch[i] = nazev[i];
            }

            ch[0] = char.ToUpper(ch[0]);

            for (int x = 1; x < nazev.Length; x++)
            {
                ch[x] = char.ToLower(ch[x]);
            }
            string nazev2 = new string(ch);
            nazev = nazev2.ToString().Trim(' ');
        }
        else
        {
            nazev = "";
        }

        return nazev;
    }

    static void Ulozit(string path,string vstup1, int vstup2, string vstup3, int vstup4, string vstup5, int vstup6, string vstup7, int pocet)  //funkce uložit použije veškere informace od uživatele co zadal do konzole a poté půjde do souboru kde užitevatel chce přidat nový film/serial a pote se to uloži do souboru a pak nám konzola napíše že se změna uložila
    {
        List<string> lists = File.ReadAllLines(path).ToList();          

        lists.Add((pocet + 1) + "." + ";"   //rank                      
            + Kontroluj(vstup1) + ";"      //nazev
            + vstup2 + ";"      //rok
            + vstup3 + ";"      //Certifikat
            + vstup4 + " min" + ";" //cas
            + Kontroluj(vstup5).Replace(' ',',') //zanr
            + ";" + vstup6  //hodnocení
            + ";" + Kontroluj(vstup7));//popis

        File.WriteAllLines(path, lists);                                

        Console.WriteLine("Uloženo");
        Console.ReadKey();
    }

    static void smazat(string path, string vstup)           //funkce smazat nám nejprve vyhledá konkretní film/serial, který chce smazat a to pomocí, že vyhleda index toho danýho filmu/serialu, a poté smaže ze souboru daný index poté přesune pořadí v souboru a nakonec se uloží změný v souboru
    {
        List<string> lines = File.ReadAllLines(path).ToList();

        var vysledek = lines.FindIndex(p => p.Contains(vstup));

        lines.RemoveAt(vysledek);

        for (int i = 0; i < lines.Count; i++)
        {
            string[] parts = lines[i].Split('.');
            parts[0] = (i + 1).ToString();
            lines[i] = string.Join(".", parts);
        }

        File.WriteAllLines(path, lines);

        Console.WriteLine("smazáno a uloženo");
        Console.ReadKey();

    }
   public static List<Databaze> seznam(List<Databaze> list)
    {
        Console.Clear();

        
        foreach (var v in list)
        {
            string text = v.Rank + ". " + v.Title.PadRight(50) + "    " + (v.IMBD_Rating + @"*").PadRight(20) + "     " + v.Genre.PadRight(20);
            Console.WriteLine(text);
            
        }
        Console.ReadKey();
        return list.ToList<Databaze>();
    }




    static void Main(string[] args)
    {                                                                                       //tahle část programu vidíme v konzoly 

        string? title = null;
        string cesta = @"";
        char klavesa;
        bool control = false;
        int cislo;

        bool menu = true;
        bool menu2 = false;
        bool menu3 = false;
        bool dal = true;

        string? addNazev = null;
        int addRok;
        string? addCer = null;
        int addCas;
        string? addZarn = null;
        int addhodnoceni;
        string? addObsah = null;


        while (dal){                                                            //použil jsem metodu přepínání mezi menu a to že mám tři meníčka tak mám tři podmínky a podle boolienové funkci tak se mi přemínají meníčka pokud se to nemíčko pravdivé a ostaní jsou vypnuté pokud už ukončit program všechny meníčka a cyklus while budou přepnutý na nepravda 
            if (menu) { 
        Console.Clear();
        Console.WriteLine("--------------------------");
        Console.WriteLine("Vítejte!");
        Console.WriteLine("--------------------------");
        Console.WriteLine();
      
        Console.WriteLine("Film [F]");
        Console.WriteLine("Serial [S]");
        Console.WriteLine("Konec [K]");

                klavesa = char.ToUpper(Console.ReadKey().KeyChar);

            switch (klavesa)                                            //rádek 190 až 206 nám přepínají cestu souboru záleží na volbu uživatel
            {
                case 'F':
                    cesta = @"imdb_top_1000.csv";
                        menu = false;                                  //přeínání mezimenu
                        menu2 = true;

                        break;
                case 'S':
                    cesta = @"series_data.csv";
                        menu = false;                                   //přeínání mezimenu
                        menu2 = true;             
                        break;
                    case 'K':
                        Console.WriteLine("Naschledanou");

                        dal = false;
                        menu = false;                                   //přeínání mezimenu
                        menu2 = false;
                        menu3 = false;

                        Console.ReadKey();
                        break;
                    default:
                        continue;

                }
        }
        var listResult = new List<Databaze>();

            IEnumerable<string> lines = File.ReadAllLines(cesta); // přečte soubor a uloží pořádcích do stringu

            List<Databaze> list = (from line in lines               //vezme každý text a poté odstraní oddělovač a dá je do pole poté se nadefinuje každou proměnou v poli a dá se to listu

                                   let values = line.Split(';')
                                   select new Databaze
                                   {
                                       Rank = values[0],
                                       Title = values[1],
                                       Year = values[2],
                                       Certificate = values[3],
                                       Runtime = values[4],
                                       Genre = values[5],
                                       IMBD_Rating = values[6],
                                       Overview = values[7],
                                   }).ToList();

            if (menu2)
            {
                Console.Clear();
                Console.WriteLine("Hledat podle:");

                Console.WriteLine();
                Console.WriteLine("Žárn [Z]");
                Console.WriteLine("Název [N]");
                Console.WriteLine("Podle písméná [P]");
                Console.WriteLine("Přidat [A]");
                Console.WriteLine("Seznam [S]");
                Console.WriteLine("Konec [K]");
                Console.WriteLine("Zpět [E]");


                klavesa = char.ToUpper(Console.ReadKey().KeyChar);

                switch (klavesa)
                {
                    case 'Z':
                        do                                              //do-while se bude ptát o žánr dokud neco nezadá 
                        {
                            Console.Clear();
                            Console.WriteLine("zarn");
                            title = Console.ReadLine();

                        } while (string.IsNullOrEmpty(title));

                        listResult = HledaniZanru(list, Kontroluj(title)); //zavolání funkce

                        menu2 = false;                                  //přeínání mezimenu
                        menu3 = true;                                   //přeínání mezimenu

                        break;
                    case 'N':
                        do                                               //do-while se bude ptát o žánr dokud neco nezadá 
                        {
                            Console.Clear();
                            Console.WriteLine("Název");
                            title = Console.ReadLine();

                        } while (string.IsNullOrEmpty(title));

                        listResult = HledaniNazev(list, Kontroluj(title));  //zavolání funkce

                        menu2 = false;                                        //přeínání mezimenu
                        menu3 = true;                                         //přeínání mezimenu

                        break;
                    case 'P':
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Zatejte písmenko");
                            title = Console.ReadLine();

                        } while (string.IsNullOrEmpty(title));

                        listResult = PodleAbecedy(list, Kontroluj(title).ToUpper());

                        menu2 = false;
                        menu3 = true;

                        break;
                    case 'A':
                        Console.Clear();                                    // [A] znamená pro akci přidat kde uživatel může přidat film/serial do souboru
                        Console.WriteLine("Napište název");
                        while (addNazev == null)
                        {
                            addNazev = Console.ReadLine();
                        }
                        Kontroluj(addNazev);
                        Console.WriteLine();

                        Console.WriteLine("Napište rok");
                        do
                        {
                            control = int.TryParse(Console.ReadLine(), out addRok);
                        } while (addRok == 0);
                        Console.WriteLine();

                        Console.WriteLine("Napište Certifikát");
                        while (addCer == null)
                        {
                            addCer = Console.ReadLine();
                        }
                        Console.WriteLine();

                        Console.WriteLine("Napište doba přehrání");
                        do
                        {
                            control = int.TryParse(Console.ReadLine(), out addCas);
                        } while (addCas == 0);
                        Console.WriteLine();

                        Console.WriteLine("Napište zarn");
                        while (addZarn == null)
                        {
                            addZarn = Console.ReadLine();
                        }
                            Kontroluj(addZarn);
                        
                        Console.WriteLine();
                        Console.WriteLine("Napište hodnocení");

                        do
                        {
                            control = int.TryParse(Console.ReadLine(), out addhodnoceni);
                        } while (addhodnoceni == 0);
                        Console.WriteLine();

                        Console.WriteLine("Napište obsah");
                        while (addObsah == null)
                        {
                            addObsah = Console.ReadLine();
                        }
                        Kontroluj(addObsah);

                        int pocet = list.Count;

                        Console.WriteLine();

                        Ulozit(cesta, addNazev, addRok, addCer, addCas, addZarn, addhodnoceni, addObsah, pocet);  //zavolání funkce

                        menu2 = false;                            //přeínání mezimenu
                        menu3 = true;                                 //přeínání mezimenu

                        Console.ReadKey();
                        break;
                    case 'S':
                        listResult = seznam(list);
                        Console.ReadKey();

                        menu2 = false;                                  //přeínání mezimenu
                        menu3 = true;
                        break;
                    case 'E':                                   //[E] je akce zpět přepíná menu aby jsme se mohli vrátit zpět do prvního menu
                        menu2 = false;
                        menu = true;
                        break;
                    case 'K':
                        Console.Clear();
                        Console.WriteLine("Naschledanou");

                        dal = false;
                        menu = false;                                   //přeínání mezimenu
                        menu2 = false;
                        menu3 = false;

                        Console.ReadKey();
                        break;
                    default:
                        continue;
                }
            }
            if(menu3)
            {  
                  var poleResult = listResult.ToArray();                  //změní listResult, které nám vrácí z fukce, tak se nám convertuje na pole 



                    Console.WriteLine("Pokud chcete vidět detaily konkretního filmu/seriálu stiknete na klavese [D]");
                    Console.WriteLine("Chcete-li film/serial smazat stisknete [R] ");
                    Console.WriteLine("Konec [K]");
                    Console.WriteLine("Zpět [Z]");
                    klavesa = char.ToUpper(Console.ReadKey().KeyChar);

                    Console.Clear();

                    switch (klavesa)
                    {
                        case 'D':                               // D - detail díky poleResult můžeme pracovat s datami které jsou v poli a proto nám vypisuje detaily filmu a serialu
                            Console.Clear();

                        do {
                            Console.WriteLine("Zadejte prosím číslo filmu");
                            control = int.TryParse(Console.ReadLine(), out cislo);
                        }while(cislo == 0);

                            Console.WriteLine("Název:");
                            Console.WriteLine(poleResult[(cislo) - 1].Title);
                            Console.WriteLine();

                            Console.WriteLine("Rok:");
                            Console.WriteLine(poleResult[(cislo) - 1].Year);
                            Console.WriteLine();

                            Console.WriteLine("Certifikát:");
                            Console.WriteLine(poleResult[(cislo) - 1].Certificate);
                            Console.WriteLine();

                            Console.WriteLine("Čas v minutach:");
                            Console.WriteLine(poleResult[(cislo) - 1].Runtime);
                            Console.WriteLine();

                            Console.WriteLine("Žárn:");
                            Console.WriteLine(poleResult[(cislo) - 1].Genre);
                            Console.WriteLine();

                            Console.WriteLine("IMBD Hodnocení:");
                            Console.WriteLine(poleResult[(cislo) - 1].IMBD_Rating + @"*");
                            Console.WriteLine();

                            Console.WriteLine("Obsah:");
                            Console.WriteLine(poleResult[(cislo) - 1].Overview);
                            Console.WriteLine();

                            Console.ReadKey();

                            break;
                        case 'R':                               // R - je pro smazaní, kde uživatel napíše číslo pořádí filmu/serialu, který chce smazat
                            Console.Clear();
                        do
                        {
                            Console.WriteLine("Zadejte prosím číslo filmu");
                            control = int.TryParse(Console.ReadLine(), out cislo);
                        } while (cislo == 0);

                            smazat(cesta, poleResult[Convert.ToInt32(cislo) - 1].Title);
                            break;

                        case 'K':                           //K - ukončí program
                            Console.Clear();
                            Console.WriteLine("Naschledanou!");
                            dal = false;
                            menu = false;
                            menu2 = false;
                            menu3 = false;

                            
                            Console.ReadKey();
                            break;
                        case 'Z':                           //Z - je to pro zpět do menu 2
                            menu3 = false;
                            menu2 = true;
                            break;
                        default:
                            continue;
                    }
                
            }
        }      
    }
}

class Databaze                      //podle jak jsem si nadefinovali v "list", kde jsme pojmenovali každý sloupec tak má získavat a nastavit 
{
    public string? Rank { get; set; }
    public string? Title { get; set; }
    public string? Year { get; set; }
    public string? Certificate { get; set; }
    public string? Runtime { get; set; }
    public string? Genre { get; set; }
    public string? IMBD_Rating { get; set; }
    public string? Overview { get; set; }
   

}

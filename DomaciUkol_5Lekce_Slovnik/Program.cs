// Zadání cesty pro ukládání souboru

using DomaciUkol_5Lekce_Slovnik;
using System.IO;


string cesta = "";
Console.WriteLine("Dobrý den,\nchcete zadat cestu k souboru ručně? (Odpovězte zadáním čísla) \n1 - ano\n2 - ne");

string answPath = Console.ReadLine();
bool tryAgainPath = true;

while (tryAgainPath)
{
    try
    {
        switch (Int32.Parse(answPath))
        {
            case (int)TrueFalse.yes:
                Console.WriteLine("Zadejte cestu:");
                cesta = Console.ReadLine();
                break;

            case (int)TrueFalse.no:
                cesta = @"C:\text.txt";
                break;
            default:
                Console.WriteLine("Zřejmě jste jako odpověď nezadali 1 nebo 2, zkuste to znovu:");
                answPath = Console.ReadLine();
                break;
        }
        tryAgainPath = false;
    }
    catch (FormatException e)
    {
        Console.WriteLine("Zřejmě jste jako odpověď nezadali 1 nebo 2, zkuste to znovu:");
        answPath = Console.ReadLine();
    }
}

// Práce se slovníkem

//var slovnik = new Dictionary<string, string>();

Dictionary<string, string> slovnik = File.ReadAllLines(cesta)
                                     .Select(x => x.Split('\t'))
                                     .ToDictionary(x => x[0], x => x[1]);

bool jeKonec = false;

Console.WriteLine("Vítejte v menu. Pro další akci zvolte číselné označení akce:");

while (!jeKonec)
{
    Console.WriteLine("\n1 - Pridat slovo");
    Console.WriteLine("2 - Vyhledat slovo");
    Console.WriteLine("3 - Vypsat celý slovník");
    Console.WriteLine("0 - Konec");

    string answ = Console.ReadLine();
    bool tryAgainFile = true;

    while (tryAgainFile)
    {
        try
        {
            switch (Int32.Parse(answ))
            {
                case (int)Operation.end:
                    jeKonec = true;
                    break;

                case (int)Operation.addWord:
                    Word word = new Word();
                    Console.Write("Zadej slovo: ");
                    word.Slovo = (Console.ReadLine()).ToUpper();
                    Console.Write("Zadej preklad: ");
                    word.Preklad = (Console.ReadLine()).ToUpper();

                    // funguje jenom v rámci slovníku, ale nezapíše se do souboru
                    try
                    {
                        slovnik.Add(word.Slovo, word.Preklad);
                        File.AppendAllText(cesta, $"{word.Slovo}\t{word.Preklad}\n");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("\nDo slovníku se snažíš dostat slovo, které už je přeložené. K tomuto slovu bude přidán nový překlad");
                        string translation = word.Preklad;
                        slovnik[word.Slovo] = slovnik[word.Slovo] + "," +  translation;
                        Console.WriteLine($"Nový překlad pro slovo {word.Slovo} je {slovnik[word.Slovo]}");
                    }

                    break;

                case (int)Operation.findWord:
                    Console.Write("Zadejte hledane slovo: ");
                    string find = (Console.ReadLine()).ToUpper();

                    Dictionary<string, string> findW = File.ReadAllLines(cesta)
                                                           .Select(x => x.Split('\t'))
                                                           .ToDictionary(x => x[0], x => x[1]);
                    if (findW.ContainsKey(find))
                    {
                            Console.WriteLine($"Předkla pro slovo {find} je: {findW[find]}");
                    }
                    else
                    {
                        Console.WriteLine("Zadané slovo není ve slovníku");
                    }

                    break;

                case (int)Operation.readFile:

                    Dictionary<string, string> readDict = File.ReadAllLines(cesta)
                                                           .Select(x => x.Split('\t'))
                                                           .ToDictionary(x => x[0], x => x[1]);

                    foreach (var r in readDict)
                    {
                        Console.WriteLine($"{r.Key}\t{r.Value}");
                    }

                    break;

                default:
                    Console.WriteLine("Zřejmě jste jako odpověď nedazali číslo ve škále od 0 do 3. Zkuste to znovu.");
                    answ = Console.ReadLine();
                    break;
            }

            tryAgainFile = false;
            
        }
        catch (FormatException e)
        {
            Console.WriteLine("Jako odpověď jste nezadali číslo. Zkuste to znovu.");
            answ = Console.ReadLine();
        }
    }

}










enum TrueFalse
{
    yes = 1,
    no = 2,
}

enum Operation
{
    end,
    addWord,
    findWord,
    readFile
}
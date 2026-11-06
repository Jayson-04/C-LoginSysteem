
using BCrypt.Net;

namespace LoginSysteem;

class Program
{
    static readonly Dictionary<string, string> Gebruikers = new();

    static void Main()
    {
        Console.WriteLine("=== Login- en registratiesysteem ===");

        bool doorgaan = true;
        while (doorgaan)
        {
            Console.WriteLine();
            Console.WriteLine("1. Registreren");
            Console.WriteLine("2. Inloggen");
            Console.WriteLine("3. Demo: gebruikersfouten");
            Console.WriteLine("4. Afsluiten");
            Console.Write("Maak een keuze: ");

            string? keuze = Console.ReadLine();

            switch (keuze?.Trim())
            {
                case "1": RegistreerMenu(); break;
                case "2": LoginMenu(); break;
                case "3": DemoGebruikersfouten(); break;
                case "4": doorgaan = false; break;
                default:
                    Console.WriteLine("Ongeldige keuze, kies 1, 2, 3 of 4.");
                    break;
            }
        }

        Console.WriteLine("Tot ziens!");
    }

    static void RegistreerMenu()
    {
        Console.WriteLine("\n--- Registreren ---");
        Console.Write("E-mailadres: ");
        string? email = Console.ReadLine();
        Console.Write("Wachtwoord: ");
        string? wachtwoord = Console.ReadLine();
        Console.Write("Herhaal wachtwoord: ");
        string? herhaling = Console.ReadLine();

        var (gelukt, bericht) = Registreer(email, wachtwoord, herhaling);
        Console.WriteLine((gelukt ? "OK: " : "FOUT: ") + bericht);
    }

    static void LoginMenu()
    {
        Console.WriteLine("\n--- Inloggen ---");
        Console.Write("E-mailadres: ");
        string? email = Console.ReadLine();
        Console.Write("Wachtwoord: ");
        string? wachtwoord = Console.ReadLine();

        var (gelukt, bericht) = Login(email, wachtwoord);
        Console.WriteLine((gelukt ? "OK: " : "FOUT: ") + bericht);
    }

    static (bool gelukt, string bericht) Registreer(string? email, string? wachtwoord, string? herhaling)
    {
        if (string.IsNullOrWhiteSpace(email))
            return (false, "E-mailadres mag niet leeg zijn.");

        email = email.Trim().ToLower();

        if (!email.Contains('@') || !email.Contains('.'))
            return (false, "Dit is geen geldig e-mailadres.");

        if (string.IsNullOrWhiteSpace(wachtwoord))
            return (false, "Wachtwoord mag niet leeg zijn.");

        if (wachtwoord.Length < 8)
            return (false, "Wachtwoord moet minimaal 8 tekens hebben.");

        if (wachtwoord != herhaling)
            return (false, "De twee wachtwoorden komen niet overeen.");

        if (Gebruikers.ContainsKey(email))
            return (false, "Dit e-mailadres is al geregistreerd.");

        try
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(wachtwoord);
            Gebruikers[email] = hash;
            return (true, "Registratie gelukt!");
        }
        catch (Exception ex)
        {
            return (false, "Er ging iets mis bij het hashen: " + ex.Message);
        }
    }

    static (bool gelukt, string bericht) Login(string? email, string? wachtwoord)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(wachtwoord))
            return (false, "Vul zowel e-mailadres als wachtwoord in.");

        email = email.Trim().ToLower();

        if (!Gebruikers.TryGetValue(email, out string? opgeslagenHash))
            return (false, "E-mailadres of wachtwoord is onjuist.");

        try
        {
            bool klopt = BCrypt.Net.BCrypt.Verify(wachtwoord, opgeslagenHash);
            return klopt
                ? (true, "Welkom! Je bent ingelogd.")
                : (false, "E-mailadres of wachtwoord is onjuist.");
        }
        catch (Exception ex)
        {
            return (false, "Er ging iets mis bij het controleren: " + ex.Message);
        }
    }

    static void DemoGebruikersfouten()
    {
        Console.WriteLine("\n--- Demo: gebruikersfouten ---");

        Toon("Registreren met leeg e-mailadres",
            Registreer("", "geheim1234", "geheim1234"));

        Toon("Registreren met e-mail zonder @",
            Registreer("piet.example.nl", "geheim1234", "geheim1234"));

        Toon("Registreren met te kort wachtwoord",
            Registreer("piet@example.nl", "abc", "abc"));

        Toon("Registreren met twee verschillende wachtwoorden",
            Registreer("piet@example.nl", "geheim1234", "geheim9999"));

        Toon("Registreren met null als invoer",
            Registreer(null, null, null));

        Toon("Correct registreren (demo@example.nl)",
            Registreer("demo@example.nl", "geheim1234", "geheim1234"));

        Toon("Zelfde e-mailadres nog een keer registreren",
            Registreer("demo@example.nl", "geheim1234", "geheim1234"));

        Toon("Inloggen met fout wachtwoord",
            Login("demo@example.nl", "verkeerd-wachtwoord"));

        Toon("Inloggen met onbekend e-mailadres",
            Login("onbekend@example.nl", "geheim1234"));

        Toon("Inloggen met lege invoer",
            Login("", ""));

        Toon("Inloggen met juist wachtwoord",
            Login("demo@example.nl", "geheim1234"));

        Console.WriteLine("\nSalt-demo: twee keer hetzelfde wachtwoord hashen");
        Console.WriteLine("Hash 1: " + BCrypt.Net.BCrypt.HashPassword("geheim1234"));
        Console.WriteLine("Hash 2: " + BCrypt.Net.BCrypt.HashPassword("geheim1234"));
        Console.WriteLine("De hashes verschillen, maar Verify() werkt voor allebei.");

        Console.WriteLine("\nDemo klaar: het programma draait nog gewoon door.");
    }

    static void Toon(string omschrijving, (bool gelukt, string bericht) resultaat)
    {
        Console.WriteLine($"\n> {omschrijving}");
        Console.WriteLine("  " + (resultaat.gelukt ? "OK: " : "FOUT: ") + resultaat.bericht);
    }
}

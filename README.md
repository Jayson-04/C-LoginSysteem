# C-LoginSysteem
Hash je wachtwoord opdracht

## Wat doet het programma?
Dit programma simuleert een login systeem gemaakt in C# met behulp van Claude. Het hoofddoel is het hashen van je wachtwoord.

## Installatie
Ik heb een nieuw project aangemaakt in Visual Studio (2026) in C#, en heb de Bcrypt.Net-Next add-on ervoor gebruikt.

## Antwoorden op de vragen
Niet rechtstreeks, maar met geluk wel. Bcrypt zorgt ervoor dat het lang duurt, dus gaat het veel tijd kosten om wachtwoorden te achterhalen.
Zowel de Hash en Bcrypt gebruiken Salt.

## Foutafhandeling
Het programma geeft duidelijke foutmeldingen bij lege of ongeldige invoer, verkeerde wachtwoorden en ongeldige keuzes. Met try-catch worden fouten bij het beveiligen en controleren van wachtwoorden opgevangen, zodat het programma niet vastloopt.

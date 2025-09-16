using System;

class Program
{
    // A. Data & Konstanter
    // Konstanter för konfiguration
    const double TAX_RATE = 0.06;
    const double STUDENT_DISCOUNT = 0.15;
    const string CURRENCY = "SEK";

    // Programdata i arrayer
    static string[] movies = { "Matrix", "Inception", "Interstellar" };
    static string[] showTimes = { "18:00", "20:30", "22:00" };
    static double[] basePrices = { 120.0, 140.0, 160.0 };

    // Användarens valda variabler
    static int selectedMovieIndex = -1;
    static int selectedTimeIndex = -1;
    static int numberOfTickets = 0;
    static bool hasStudentDiscount = false;

    static void Main(string[] args)
    {
        bool isRunning = true;
        while (isRunning)
        {
            ShowMenu();
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ListMovies(movies, showTimes, basePrices);
                    break;
                case "2":
                    ChooseMovieAndTickets();
                    break;
                case "3":
                    ToggleStudentDiscount();
                    break;
                case "4":
                    PrintReceipt(selectedMovieIndex, selectedTimeIndex, numberOfTickets, hasStudentDiscount);
                    break;
                case "5":
                    isRunning = false;
                    Console.WriteLine("Tack för ditt besök! Välkommen åter.");
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Vänligen välj ett nummer mellan 1-5.");
                    break;
            }
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
            Console.Clear();
        }
    }

  
    // ShowMenu()
    static void ShowMenu()
    {
        Console.WriteLine("============================");
        Console.WriteLine("    Bio-biljettsystem");
        Console.WriteLine("============================");
        Console.WriteLine("1. Se filmer & visningstider");
        Console.WriteLine("2. Välj film, tid & antal biljetter");
        Console.WriteLine("3. Lägg på/ta bort studentrabatt");
        Console.WriteLine("4. Skriv ut kvitto");
        Console.WriteLine("5. Avsluta");
        Console.WriteLine("----------------------------");
        Console.WriteLine($"Valda biljetter: {numberOfTickets}");
        if (selectedMovieIndex != -1)
        {
            Console.WriteLine($"Vald film: {movies[selectedMovieIndex]} ({showTimes[selectedTimeIndex]})");
        }
        Console.WriteLine($"Studentrabatt: {(hasStudentDiscount ? "Ja" : "Nej")}");
        Console.WriteLine("----------------------------");
        Console.Write("Välj ett alternativ: ");
    }

    // ListMovies()
    static void ListMovies(string[] movies, string[] times, double[] basePrices)
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Aktuella filmer och priser:");
        for (int i = 0; i < movies.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {movies[i]} kl. {times[i]} - {basePrices[i]:F2} {CURRENCY}");
        }
        Console.WriteLine("----------------------------");
    }

    static void ChooseMovieAndTickets()
    {
        ListMovies(movies, showTimes, basePrices);

        // Val av film
        int movieChoice;
        Console.Write("Välj film (ange nummer): ");
        while (!int.TryParse(Console.ReadLine(), out movieChoice) || movieChoice < 1 || movieChoice > movies.Length)
        {
            Console.Write("Ogiltigt val. Vänligen ange ett nummer från listan: ");
        }
        selectedMovieIndex = movieChoice - 1;

        // Val av antal biljetter
        Console.Write("Hur många biljetter vill du köpa? ");
        while (!int.TryParse(Console.ReadLine(), out numberOfTickets) || numberOfTickets <= 0)
        {
            Console.Write("Ogiltigt antal. Vänligen ange ett heltal större än 0: ");
        }
    }

    static void ToggleStudentDiscount()
    {
        hasStudentDiscount = !hasStudentDiscount;
        Console.WriteLine($"Studentrabatt är nu {(hasStudentDiscount ? "aktiverad" : "avaktiverad")}.");
    }

    // Överlagrade metoder som returnerar ett värde
    static double CalculatePrice(int tickets, double basePrice)
    {
        return tickets * basePrice;
    }

    static double CalculatePrice(int tickets, double basePrice, double discountPercent)
    {
        double priceWithoutDiscount = CalculatePrice(tickets, basePrice);
        return priceWithoutDiscount * (1 - discountPercent);
    }

    // Metod för kvittoutskrift med namngivna argument
    // PrintReceipt()
    static void PrintReceipt(int movieIndex, int timeIndex, int tickets, bool isStudent)
    {
        if (tickets <= 0)
        {
            Console.WriteLine("Du måste välja film och biljetter först.");
            return;
        }

        double basePrice = basePrices[movieIndex];
        double finalPrice;

        if (isStudent)
        {
            finalPrice = CalculatePrice(tickets, basePrice, STUDENT_DISCOUNT);
        }
        else
        {
            finalPrice = CalculatePrice(tickets, basePrice);
        }

        double tax = finalPrice * TAX_RATE;
        double totalIncludingTax = finalPrice + tax;

        // Anrop med namngivna argument för tydlighet
        Console.WriteLine($"\n--- Kvitto ---");
        Console.WriteLine($"Film: {movies[movieIndex]}");
        Console.WriteLine($"Tid: {showTimes[timeIndex]}");
        Console.WriteLine($"Antal biljetter: {tickets}");
        Console.WriteLine($"Studentrabatt: {(isStudent ? "Ja" : "Nej")}");
        Console.WriteLine("----------------");
        Console.WriteLine($"Pris (exkl. moms): {finalPrice:F2} {CURRENCY}");
        Console.WriteLine($"Moms ({TAX_RATE * 100}%): {tax:F2} {CURRENCY}");
        Console.WriteLine($"Totalpris: {totalIncludingTax:F2} {CURRENCY}");
        Console.WriteLine("----------------");
    }
}
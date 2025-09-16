using System;

namespace BioBokaren
{
    internal class Program
    {
        //Konstanter
        const double TAX_RATE = 0.08;
        const double STUDENT_DISCOUNT = 0.20;
        const string CURRENCY = "SEK";

        //Programdata of films
        static string[] movies = { "The Green Mile", "Inception", "Interstellar", "Forrest Gump", "Terminator 2" };
        static string[] showTimes = { "14:30", "16:00", "18:00", "20:30", "22:00" };
        static double[] basePrices = { 110.0, 120.0, 130.0, 140.0, 155.0 };

        
        static int selectedMovieIndex = -1;
        static int selectedTimeIndex = -1;
        static int ticketCount = 0;
        static bool isStudent = false;

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                ShowMenu();
                Console.Write("Välj ett alternativ: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListMovies(movies, showTimes, basePrices);
                        break;
                    case "2":
                        SelectMovieAndTickets();
                        break;
                    case "3":
                        ToggleStudentDiscount();
                        break;
                    case "4":
                        if (selectedMovieIndex == -1 || selectedTimeIndex == -1 || ticketCount == 0)
                        {
                            Console.WriteLine("Du måste göra en bokning innan du kan skriva ut kvitto.");
                        }
                        else
                        {
                            PrintReceipt(
                                movie: movies[selectedMovieIndex],
                                time: showTimes[selectedTimeIndex],
                                tickets: ticketCount,
                                total: CalculateTotalPrice(),
                                isStudent: isStudent
                            );
                        }
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Tack för att du använde BioApp. Ha en trevlig dag!");
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        //Meny
        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nBIOBOKNING");
            Console.ResetColor();
            Console.WriteLine("1. Lista filmer");
            Console.WriteLine("2. Välj film, tid och antal biljetter");
            Console.WriteLine("3. Lägg på/ ta bort studentrabatt");
            Console.WriteLine("4. Skriv ut kvitto");
            Console.WriteLine("5. Avsluta"); //Tackar användaren
        }

        //Lista filmer
        static void ListMovies(string[] movies, string[] times, double[] basePrices)
        {
            if (movies.Length != times.Length || movies.Length != basePrices.Length)
            {
                Console.WriteLine("Fel i programdata: Arrayerna har olika längd.");
                return;
            }
           
            Console.WriteLine("\nTillgängliga filmer:");
            for (int i = 0; i < movies.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {movies[i]} - Tid: {times[i]} - Pris: {basePrices[i]} {CURRENCY}");
            }
        }

        //Välj film och biljetter
        static void SelectMovieAndTickets()
        {
            ListMovies(movies, showTimes, basePrices);

            Console.Write($"Ange filmnummer (1-{movies.Length}): ");
            if (int.TryParse(Console.ReadLine(), out int movieIndex) && movieIndex >= 1 && movieIndex <= movies.Length)
            {
                selectedMovieIndex = movieIndex - 1;
            }
            else
            {
                Console.WriteLine("Ogiltigt filmval.");
                return;
            }

            Console.Write($"Ange visningstid (1-{showTimes.Length}): ");
            if (int.TryParse(Console.ReadLine(), out int timeIndex) && timeIndex >= 1 && timeIndex <= showTimes.Length)
            {
                selectedTimeIndex = timeIndex - 1;
            }
            else
            {
                Console.WriteLine("Ogiltig tid.");
                return;
            }

            Console.Write("Ange antal biljetter: ");
            if (int.TryParse(Console.ReadLine(), out int tickets) && tickets > 0)
            {
                ticketCount = tickets;
            }
            else
            {
                Console.WriteLine("Ogiltigt antal.");
            }
        }

        //Studentrabatt
        static void ToggleStudentDiscount()
        {
            isStudent = !isStudent;
            Console.WriteLine(isStudent ? "Studentrabatt aktiverad." : "Studentrabatt avaktiverad.");
        }

        //Prisberäkning
        static double CalculatePrice(int tickets, double basePrice)
        {
            return tickets * basePrice;
        }

        static double CalculatePrice(int tickets, double basePrice, double discountPercent)
        {
            double discountedPrice = basePrice * (1 - discountPercent);
            return tickets * discountedPrice;
        }

        //Totalpris med moms
        static double CalculateTotalPrice()
        {
            double basePrice = basePrices[selectedMovieIndex];
            double subtotal = isStudent
                ? CalculatePrice(ticketCount, basePrice, STUDENT_DISCOUNT)
                : CalculatePrice(ticketCount, basePrice);

            return subtotal * (1 + TAX_RATE);
        }

        //Kvitto
        static void PrintReceipt(string movie, string time, int tickets, double total, bool isStudent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nKVITTO");
            Console.ResetColor();
            Console.WriteLine($"Film: {movie}");
            Console.WriteLine($"Tid: {time}");
            Console.WriteLine($"Antal biljetter: {tickets}");
            Console.WriteLine($"Studentrabatt: {(isStudent ? "Ja" : "Nej")}");
            Console.WriteLine($"Totalpris (inkl. moms): {total:F2} {CURRENCY}");
        }
    }
}

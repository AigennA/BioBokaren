using System;

namespace BioBokaren
{
    internal class Program
    {
        //constans
        const double TAX_RATE = 0.08;
        const double STUDENT_DISCOUNT = 0.20;
        const string CURRENCY = "SEK";

        //Programdata - films
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
                Console.Write("Choose an alternative: ");
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
                            Console.WriteLine("You should have a booing before you can print out receipt.");
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
                        Console.WriteLine("Thank you for choosing BioApp. Welcome back!");
                        break;
                    default:
                        Console.WriteLine("Invalid choose, try again.");
                        break;
                }
            }
        }

        //Menu
        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nBIOBOKNING");
            Console.ResetColor();
            Console.WriteLine("1. List films");
            Console.WriteLine("2. Select films, time and number of tickets");
            Console.WriteLine("3. Add/ remove student discount");
            Console.WriteLine("4. Print receipt");
            Console.WriteLine("5. Exit");//User can se this message"Thank you for choosing BioApp. Welcome back!"
        }

        //List films
        static void ListMovies(string[] movies, string[] times, double[] basePrices)
        {
            if (movies.Length != times.Length || movies.Length != basePrices.Length)
            {
                Console.WriteLine("Error in program data: The arrays have different lengths.");
                return;
            }
           
            Console.WriteLine("\nAvailable movies:");
            for (int i = 0; i < movies.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {movies[i]} - Tid: {times[i]} - Pris: {basePrices[i]} {CURRENCY}");
            }
        }

        //Select film and tickets
        static void SelectMovieAndTickets()
        {
            ListMovies(movies, showTimes, basePrices);

            Console.Write($"Enter movie number (1-{movies.Length}): ");
            if (int.TryParse(Console.ReadLine(), out int movieIndex) && movieIndex >= 1 && movieIndex <= movies.Length)
            {
                selectedMovieIndex = movieIndex - 1;
            }
            else
            {
                Console.WriteLine("Invalid movie selection.");
                return;
            }

            Console.Write($"Set viewing time(1-{showTimes.Length}): ");
            if (int.TryParse(Console.ReadLine(), out int timeIndex) && timeIndex >= 1 && timeIndex <= showTimes.Length)
            {
                selectedTimeIndex = timeIndex - 1;
            }
            else
            {
                Console.WriteLine("Invalid time.");
                return;
            }

            Console.Write("Number of tickets: ");
            if (int.TryParse(Console.ReadLine(), out int tickets) && tickets > 0)
            {
                ticketCount = tickets;
            }
            else
            {
                Console.WriteLine("Invalid number.");
            }
        }

        //Student discount
        static void ToggleStudentDiscount()
        {
            isStudent = !isStudent;
            Console.WriteLine(isStudent ? "Student discount activated." : "Student discount deactivated.");
        }

        //Pricecounting
        static double CalculatePrice(int tickets, double basePrice)
        {
            return tickets * basePrice;
        }

        static double CalculatePrice(int tickets, double basePrice, double discountPercent)
        {
            double discountedPrice = basePrice * (1 - discountPercent);
            return tickets * discountedPrice;
        }

        //Totalprice med moms
        static double CalculateTotalPrice()
        {
            double basePrice = basePrices[selectedMovieIndex];
            double subtotal = isStudent
                ? CalculatePrice(ticketCount, basePrice, STUDENT_DISCOUNT)
                : CalculatePrice(ticketCount, basePrice);

            return subtotal * (1 + TAX_RATE);
        }

        //Receipt
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

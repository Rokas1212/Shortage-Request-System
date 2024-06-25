using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortageRequests.Util
{
    public static class Operations
    {
        private const string FilePath = "./shortages.json"; 

        private static List<Shortage> ReadShortages()
        {
            var shortages = new List<Shortage>();
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        shortages = JsonSerializer.Deserialize<List<Shortage>>(json, options) ?? new List<Shortage>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }

            return shortages;
        }

        private static void WriteShortages(List<Shortage> requests)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            try
            {
                string updatedJson = JsonSerializer.Serialize(requests, options);
                File.WriteAllText(FilePath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }

        private static Shortage RegisterShortage(string name)
        {
            Console.Write("\nEnter Title: ");
            var title = Console.ReadLine() ?? "";
            
            if(title == "")
            {
                Console.WriteLine("Invalid Title!");
                return RegisterShortage(name);
            }


            Console.Write("Enter category (1. Electronics, 2. Food, 3. Other): ");
            Category category;
            while (true)
            {
                try
                {
                    var categoryInput = Convert.ToInt32(Console.ReadLine());
                    if (Enum.IsDefined(typeof(Category), categoryInput - 1))
                    {
                        category = (Category)(categoryInput - 1);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Choose a valid input (1-3)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                }
            }

            Room room;
            while (true)
            {
                Console.Write("Enter room (1. MeetingRoom, 2. Kitchen, 3. Bathroom): ");
                try
                {
                    var roomInput = Convert.ToInt32(Console.ReadLine());
                    if (Enum.IsDefined(typeof(Room), roomInput - 1))
                    {
                        room = (Room)(roomInput - 1);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Choose a valid input (1-3)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                }
            }

            var priority = 0;
            while (true)
            {
                Console.WriteLine("Specify the priority of your request 1-10 (1-LOW, 10-HIGH)");
                try
                {
                    priority = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                }
            }

            return new Shortage(title, name, room, category, priority);        
        }

        public static void DeleteShortage(List<Shortage> shortages, List<Shortage> displayedList, int id)
        {
            var title = displayedList[id - 1].Title;
            var room = displayedList[id - 1].Room;
            displayedList.RemoveAt(id - 1);

            Shortage toDelete = shortages.Find(x => x.Title == title && x.Room == room)!;
            shortages.Remove(toDelete);

            WriteShortages(shortages);

            Console.Clear();
            Console.WriteLine("Request deleted successfully");
        }

        private static void DisplayTable(List<Shortage> displayedList)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($" {"Id", -3} | {"Title", -15} | {"Name", -15} | {"Room", -11} | {"Category", -12} | {"CreatedOn", -10} | {"Priority", -10}");
            Console.ResetColor();
            Console.WriteLine(new string('-', 95));

            var i = 1;
            foreach (var row in displayedList)
            {
                Console.WriteLine($" {i++, -3} | {row}");
                Console.WriteLine(new string('-', 95));
            }
        }


        public static List<Shortage> FilterByTitle(List<Shortage> displayedList)
        {
            Console.Write("Enter title to filter by: ");
            var title = Console.ReadLine() ?? "";
            var filteredList = displayedList.Where(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

            return filteredList;
        }

        public static List<Shortage> FilterByCreatedOn(List<Shortage> displayedList)
        {
            Console.Write("Enter two dates to filter by (yyyy-mm-dd): ");
            
            string? date1 = Console.ReadLine();
            string? date2 = Console.ReadLine();

            if (!DateTime.TryParse(date1, out DateTime from) || !DateTime.TryParse(date2, out DateTime to))
                throw new ArgumentException("Invalid date format");
            
            var filteredList = displayedList.Where(x => x.Date >= from && x.Date <= to).ToList();
            return filteredList;
        }

        public static List<Shortage> FilterByCategory(List<Shortage> displayedList)
        {
            Console.Write("Enter category to filter by (1. Electronics, 2. Food, 3. Other): ");
            while (true)
            {
                try
                {
                    var categoryInput = Convert.ToInt32(Console.ReadLine());
                    if (Enum.IsDefined(typeof(Category), categoryInput - 1))
                    {
                        var category = (Category)(categoryInput - 1);
                        var filteredList = displayedList.Where(x => x.Category == category).ToList();
                        return filteredList;
                    }
                    else
                    {
                        Console.WriteLine("Choose a valid input (1-3)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Choose a valid input (1-3)");
                }
            }
        }

        public static List<Shortage> FilterByRoom(List<Shortage> displayedList)
        {
            Console.Write("Enter room to filter by (1. MeetingRoom, 2. Kitchen, 3. Bathroom): ");
            while (true)
            {
                try
                {
                    var roomInput = Convert.ToInt32(Console.ReadLine());
                    if (Enum.IsDefined(typeof(Room), roomInput - 1))
                    {
                        var room = (Room)(roomInput - 1);
                        var filteredList = displayedList.Where(x => x.Room == room).ToList();
                        return filteredList;
                    }
                    else
                    {
                        Console.WriteLine("Choose a valid input (1-3)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Choose a valid input (1-3)");
                }
            }
        }
        

        private static void Display(string name, List<Shortage> shortages)
        {
            var displayedList = shortages.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || name.Equals("admin", StringComparison.OrdinalIgnoreCase))
                                          .OrderByDescending(x => x.Priority)
                                          .ToList();

            DisplayTable(displayedList);

            while (true)
            {
                Console.WriteLine("1. Delete a Shortage Request");
                Console.WriteLine("2. Filter by title");
                Console.WriteLine("3. Filter by date");
                Console.WriteLine("4. Filter by category");
                Console.WriteLine("5. Filter by room");
                Console.WriteLine("6. Back");

                int option = 0;
                try
                {
                    option = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Choose a valid input (1-6)");
                    continue;
                }

                switch (option)
                {
                    //Delete a request
                    case 1:
                        Console.Write("Enter ID of the request you want to delete: ");
                        try
                        {
                            int id = Convert.ToInt32(Console.ReadLine());
                            if (id < 1 || id > displayedList.Count)
                            {
                                Console.WriteLine("This ID does not exist");
                                continue;
                            }
                            DeleteShortage(shortages, displayedList, id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Invalid input: {ex.Message}");
                        }
                        return;

                    //Filtering by Title
                    case 2:
                        var filteredList = FilterByTitle(displayedList);
                        Console.Clear();
                        DisplayTable(filteredList);
                        continue;

                    //Filtering by CreatedOn
                    case 3:
                        try
                        {
                            filteredList = FilterByCreatedOn(displayedList);
                            DisplayTable(filteredList);
                        }
                        catch (Exception ex)
                        {
                            DisplayTable(displayedList);
                            Console.WriteLine($"Invalid input: {ex.Message}");
                        }
                        continue;

                    //Filtering by Category
                    case 4:
                        filteredList = FilterByCategory(displayedList);

                        Console.Clear();
                        DisplayTable(filteredList);
                        continue;

                    //Filtering by Room
                    case 5:
                        filteredList = FilterByRoom(displayedList);

                        Console.Clear();
                        DisplayTable(filteredList);
                        continue;

                    //Back
                    case 6:
                        Console.Clear();
                        return;

                    //Invalid input
                    default:
                        Console.WriteLine("Enter a valid option");
                        continue;
                }
            }
        }


        public static void Start(string name)
        {
            var shortages = ReadShortages();
            var flag = true;

            while(flag)
            {
                Console.WriteLine("Choose an operation: ");
                Console.WriteLine("1. Register a shortage");
                Console.WriteLine("2. Active Shortage Requests");
                Console.WriteLine("3. Exit");

                var option = 0;
                try
                {
                    option = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                    continue;
                }

                switch (option)
                {
                    //Register a shortage
                    case 1:
                        var shortage = RegisterShortage(name);

                        var existingShortage = shortages.Find(x => x.Title == shortage.Title && x.Room == shortage.Room);

                        if(existingShortage != null)
                        {
                            if(shortage.Priority > existingShortage.Priority)
                            {
                                shortage.Name = existingShortage.Name;
                                shortage.Date = existingShortage.Date;
                                shortages.Remove(existingShortage);
                                shortages.Add(shortage);
                                Console.WriteLine("\nShortage already registered, updated priority\n\n");
                                continue;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\nShortage already registered with a higher or equal priority\n\n");
                                continue;
                            }
                        }
    
                        shortages.Add(shortage);
                        
                        WriteShortages(shortages);

                        Console.Clear();
                        Console.WriteLine("\nShortage registered\n\n");
                        break;

                    //Display active shortage requests
                    case 2:
                        Display(name, shortages);
                        break;

                    //Exit
                    case 3:
                        flag = false;
                        break;

                    //Invalid input
                    default:
                        Console.Clear();
                        Console.WriteLine("Enter a valid option!\n");
                        continue;
                }
            }
        }
    }
}


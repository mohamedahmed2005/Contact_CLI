using Contact_CLI.Application_Layer.Services;
using Contact_CLI.Entity;
using Contact_CLI.Json_Infrastructure;

namespace Contact_CLI.Presentation_Layer
{
    public class Program
    {
        public static void Contact_menu()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine(" Contact Management System");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contact");
            Console.WriteLine("5. List Contacts");
            Console.WriteLine("6. Search");
            Console.WriteLine("7. Filter");
            Console.WriteLine("8. Save");
            Console.WriteLine("9. Exit");
            Console.WriteLine("==============================");
        }

        public static string Contact_input()
        {
            Contact_menu();
            Console.Write("Enter your choice: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public static string Contact_Validate_input()
        {
            string input = Contact_input();
            while (!int.TryParse(input, out int choice) || choice < 1 || choice > 9)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                input = Contact_input();
            }
            return input;
        }
        public static bool Contact_Validate_Phone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?\d{7,15}$");
        }
        public static bool Contact_Validate_Email(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static void PrintContactRow(Contact c)
        {
            Console.WriteLine($"{c.Id,-5} {c.Name,-20} {c.Phone,-15} {c.Email,-25} {c.CreationDate:dd/MM/yyyy HH:mm}");
        }

        private static void PrintTableHeader()
        {
            Console.WriteLine($"\n{"ID",-5} {"Name",-20} {"Phone",-15} {"Email",-25} {"Created",-17}");
            Console.WriteLine(new string('-', 82));
        }

        private static void HandleAdd(ContactService service)
        {
            Console.Write("Name  : ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Phone : ");
            string phone = Console.ReadLine() ?? "";

            while (!Contact_Validate_Phone(phone))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
                Console.Write("Phone : ");
                phone = Console.ReadLine() ?? "";
            }

            Console.Write("Email : ");
            string email = Console.ReadLine() ?? "";

            while (!Contact_Validate_Email(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
                Console.Write("Email : ");
                email = Console.ReadLine() ?? "";
            }

            service.AddContact(0, name, phone, email);
            Console.WriteLine("Contact added successfully");
        }

        private static void HandleEdit(ContactService service)
        {
            Console.Write("Enter contact ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { Console.WriteLine("Invalid ID."); return; }

            Contact current = service.GetById(id);
            if (current == null) { Console.WriteLine("Contact not found."); return; }

            Console.WriteLine("\n--- Current details ---");
            Console.WriteLine($"  Name  : {current.Name}");
            Console.WriteLine($"  Phone : {current.Phone}");
            Console.WriteLine($"  Email : {current.Email}");
            Console.WriteLine("(Press Enter to keep the current value)\n");

            Console.Write($"New Name  [{current.Name}]: ");
            string newName  = Console.ReadLine() ?? "";

            Console.Write($"New Phone [{current.Phone}]: ");
            string newPhone = Console.ReadLine() ?? "";

            Console.Write($"New Email [{current.Email}]: ");
            string newEmail = Console.ReadLine() ?? "";

            service.UpdateContact(id,
                string.IsNullOrWhiteSpace(newName)  ? current.Name  : newName,
                string.IsNullOrWhiteSpace(newPhone) ? current.Phone : newPhone,
                string.IsNullOrWhiteSpace(newEmail) ? current.Email : newEmail);

            Console.WriteLine("Contact updated Successfully.");
        }

        private static void HandleDelete(ContactService service)
        {
            Console.Write("Enter contact ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { Console.WriteLine("Invalid ID."); return; }

            Contact existing = service.GetById(id);
            if (existing == null) { Console.WriteLine("Contact not found."); return; }

            service.Delete(id);
            Console.WriteLine($"Contact '{existing.Name}' deleted Successfully.");
        }

        private static void HandleView(ContactService service)
        {
            Console.Write("Enter contact ID to view: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { Console.WriteLine("Invalid ID."); return; }

            Contact c = service.GetById(id);
            if (c == null) { Console.WriteLine("Contact not found."); return; }

            Console.WriteLine($"\n  ID      : {c.Id}");
            Console.WriteLine($"  Name    : {c.Name}");
            Console.WriteLine($"  Phone   : {c.Phone}");
            Console.WriteLine($"  Email   : {c.Email}");
            Console.WriteLine($"  Created : {c.CreationDate:dd/MM/yyyy HH:mm}");
        }

        private static void HandleList(ContactService service)
        {
            var all = service.GetAll();
            if (!all.Any()) { Console.WriteLine("No contacts found."); return; }

            PrintTableHeader();
            foreach (var c in all)
                PrintContactRow(c);
        }

        private static void HandleSearch(ContactService service)
        {
            Console.Write("Search by name or phone: ");
            string query = Console.ReadLine() ?? "";

            var results = service.GetAll()
                .Where(c => c.Name.Contains(query,  StringComparison.OrdinalIgnoreCase)
                         || c.Phone.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!results.Any()) { Console.WriteLine("No matches found."); return; }

            PrintTableHeader();
            foreach (var c in results)
                PrintContactRow(c);
        }

        private static void HandleFilter(ContactService service)
        {
            Console.WriteLine("Filter by:");
            Console.WriteLine("  1. Email domain");
            Console.WriteLine("  2. Creation date (dd/MM/yyyy)");
            Console.Write("Choice: ");
            string filterChoice = Console.ReadLine() ?? "";

            List<Contact> filtered;

            if (filterChoice == "1")
            {
                Console.Write("Enter email domain (e.g. gmail.com): ");
                string domain = Console.ReadLine() ?? "";
                filtered = service.GetAll()
                    .Where(c => c.Email.EndsWith(domain, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else if (filterChoice == "2")
            {
                Console.Write("Enter date (dd/MM/yyyy): ");
                string dateStr = Console.ReadLine() ?? "";
                if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime filterDate))
                { Console.WriteLine("Invalid date format."); return; }

                filtered = service.GetAll()
                    .Where(c => c.CreationDate.Date == filterDate.Date)
                    .ToList();
            }
            else { Console.WriteLine("Invalid filter choice."); return; }

            if (!filtered.Any()) { Console.WriteLine("No matches found."); return; }

            PrintTableHeader();
            foreach (var c in filtered)
                PrintContactRow(c);
        }

        private static void HandleSave(ContactService service, string filePath)
        {
            service.Save();
            Console.WriteLine($"Contacts saved to {Path.GetFileName(filePath)}");
        }

        private static bool HandleExit(ContactService service)
        {
            Console.Write("Save before exiting? (y/n): ");
            string answer = Console.ReadLine() ?? "n";
            if (answer.Trim().ToLower() == "y")
            {
                service.Save();
                Console.WriteLine("Contacts saved successfully.");
            }
            Console.WriteLine("Goodbye!");
            return false;
        }

        private static string PromptForFilePath()
        {
            string fileName;
            do
            {
                Console.Write("Enter the name of the contacts file (e.g. contacts.json): ");
                fileName = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("File name cannot be empty. Please try again.");
                    continue;
                }
            } while (string.IsNullOrWhiteSpace(fileName));
            if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                fileName += ".json";
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "[]");
                Console.WriteLine($"File '{fileName}' created.");
            }
            else
            {
                Console.WriteLine($"File '{fileName}' found. Loading contacts...");
            }


            return fileName;
        }

        static void Main(string[] args)
        {
            string filePath = PromptForFilePath();
            var service = new ContactService(new JsonRepository(filePath));

            bool running = true;
            while (running)
            {
                switch (Contact_Validate_input())
                {
                    case "1": HandleAdd(service);              break;
                    case "2": HandleEdit(service);             break;
                    case "3": HandleDelete(service);           break;
                    case "4": HandleView(service);             break;
                    case "5": HandleList(service);             break;
                    case "6": HandleSearch(service);           break;
                    case "7": HandleFilter(service);           break;
                    case "8": HandleSave(service, filePath);   break;
                    case "9": running = HandleExit(service);   break;
                }
            }
        }
    }
}

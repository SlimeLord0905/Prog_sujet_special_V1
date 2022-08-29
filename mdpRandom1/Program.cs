// See https://aka.ms/new-console-template for more information


using System.Text.Json;

using Crypto;

namespace mdpRandom1
{
    internal class Program
    {
        static string letters = "abcdefghijklmnopqrstuvwxyz";
        static string numbers = "1234567890";
        static string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static  string symbole = "!/$%?&*()";
        
        static string answer;
        private static List<Mdp> ActivePasswords = new List<Mdp>();
        
        //get all active password from json file
        
        static void Main(string[] args)
        {
            AesCrypter aes = new AesCrypter("hiddenkey");
            string path = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "";
            path = Path.Combine(path, "passwords.dat");
            string json = aes.DecryptFromFile(path);
            
            ActivePasswords= JsonSerializer.Deserialize<List<Mdp>>(json) ?? new List<Mdp>();
            do
            {
                
               
                Console.WriteLine("Choose an option 1: create password 2: check passwords 3:turn the program off");
                answer = Console.ReadLine();
                if (answer == "1")
                {

                    string password;
                    string username = "unknown";
                    int length = 16;
                    bool symb = false;
                    bool up = false;
                    bool num = false;
                    string stop = "y";
                    while (stop == "y")
                    {
                        Console.WriteLine("Enter length:");
                        length = Convert.ToInt32(Console.ReadLine());

                        while (length < 8 || length > 64)
                        {
                            Console.WriteLine("Invalide length must be between 8 an 64 characters defult is 16");
                            Console.WriteLine("Enter length:");
                            length = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.WriteLine("Use Numbers y or n");
                        string answer = Console.ReadLine();
                        if (answer == "y")
                        {

                            num = true;
                        }

                        Console.WriteLine("Use uppercase y or n");
                        answer = Console.ReadLine();
                        if (answer == "y")
                        {

                            up = true;
                        }

                        Console.WriteLine("Use symbole y or n");
                        answer = Console.ReadLine();
                        if (answer == "y")
                        {

                            symb = true;
                        }

                        password = PasswordGenerator.GeneratePassword(length, symb, num, up, true);
                        Console.WriteLine(password);
                        Console.WriteLine("Enter the username going with this password");
                        username = Console.ReadLine() ?? "unknown";
                        Mdp newmdp = new Mdp(username, password);
                        ActivePasswords.Add(newmdp);
                        
                        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                        json = JsonSerializer.Serialize(ActivePasswords, options);
                        Console.WriteLine(json);
                        
                       
                       
                        aes.EncryptToFile(json, path);
                        
                        
                        Console.WriteLine("generate an other password y or n");
                        stop = Console.ReadLine();
                    }
                }
                else if (answer == "2")
                {
                    Console.WriteLine("type the username of the password(1), see the list(2) or quit(3)");
                    string Answer2 = Console.ReadLine();
                    do
                    {
                        if (Answer2 == "1")
                        {
                            Console.WriteLine("type the username associated with your password");
                            string recherche = Console.ReadLine();

                            foreach (Mdp mdp in ActivePasswords)
                            {
                                if (mdp.Username.Equals(recherche))
                                {
                                    Console.WriteLine("Username: "+mdp.Username+" Password: "+ mdp.Password);
                                }
                            }
                        }
                        else if (Answer2 == "2")
                        {
                            foreach (Mdp mdp in ActivePasswords)
                            {
                                Console.WriteLine("Username: "+mdp.Username+" Password: "+ mdp.Password);
                            }
                        }
                        
                        
                        
                        Console.WriteLine("type the username of the password(1), see the list(2) or quit(3)");
                        Answer2 = Console.ReadLine();
                    } while (Answer2 != "3");
                }
            } while (answer != "3");
        }
    }
}
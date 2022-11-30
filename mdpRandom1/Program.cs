// See https://aka.ms/new-console-template for more information
//test

using System.ComponentModel;
using System.Text.Json;

using CryptoLib;

namespace mdpRandom1
{
    internal class Program
    {
        static string letters = "abcdefghijklmnopqrstuvwxyz";
        static string numbers = "1234567890";
        static string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static  string symbole = "!/$%?&*()";
        
        static string answer;
        
        //get all active password from json file
        
        static void Main(string[] args)
        {
            bool Online = false;
            user CurrentUser = new user(-1);
            AesCrypter aes = new AesCrypter("hiddenkey");
            string path = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "";
            path = Path.Combine(path, "passwords.dat");
            string json = aes.DecryptFromFile(path);
            DBcontroller Db = new DBcontroller();
            DBcontrollerOffline DbOf = new DBcontrollerOffline();
            List<password> currentpwd;
            
            
            Console.WriteLine("Do you want to be on 1)Online mode 2)Offline Mode");
            string ans = Console.ReadLine();
            if (ans == "1")
            {
                Online = true;
            }
            //String pwd = BCrypt.Net.BCrypt.HashPassword("12345678");
           // Console.WriteLine(pwd);

               while (CurrentUser.id == -1)
               {
                   Console.WriteLine("Enter your Username");
                   string usr = Console.ReadLine();

                   Console.WriteLine("Enter your password");
                   string usr_pwd = Console.ReadLine();


                   if (Online)
                   {
                       CurrentUser = Db.ConnecteUser(usr, usr_pwd);
                   }
                   else
                   {
                       CurrentUser = DbOf.ConnecteUser(usr, usr_pwd);
                   }
                   if (CurrentUser.id != -1)
                   {
                       Console.WriteLine("connection ok"+ CurrentUser.id);
                   }
               }

               SyncManager syncManager = new SyncManager();
               syncManager.SyncPasswords(Db,DbOf, CurrentUser.id);
               currentpwd = Db.GetUserPasswords(CurrentUser.id);
               if (!Online)
               {
                   currentpwd = DbOf.GetUserPasswords(1);
               }



               //Db.AddPassword(1, "youtube", "Lord0905", "unpasswordtest");
            //Db.UpdatePassword(3,1, "youtube", "Lord0905", "unpassword");
            //Db.DeletePassword(3);
            //Db.AddPasswordToDelete(1);
           // Db.RemovePasswordToDelete(1);

            /*if (Db.adduser("lord", "Mathieu Mercier", "mercier@gmail.com", "unpassword0905"))
            {
                Console.WriteLine("adduser ok");
            }*/
            
            //ActivePasswords= JsonSerializer.Deserialize<List<Mdp>>(json) ?? new List<Mdp>();
            do
            {
                
               
                Console.WriteLine("Choose an option 1: create password 2: check passwords 3:turn the program off");
                answer = Console.ReadLine();
                if (answer == "1")
                {

                    string login;
                    string pwd;
                    string username = "unknown";
                    string site = "unknown";
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

                        pwd = PasswordGenerator.GeneratePassword(length, symb, num, up, true);
                        Console.WriteLine(pwd);
                        Console.WriteLine("Enter the Login going with this password");
                        login = Console.ReadLine() ?? "unknown";
                        Console.WriteLine("Enter the site going with this password");
                        site = Console.ReadLine() ?? "unknown";

                        pwd = aes.Encrypt(pwd);
                        if (Online)
                        {
                            Db.AddPassword(CurrentUser.id, site, login, pwd);
                        }
                        DbOf.AddPassword(CurrentUser.id, site, login, pwd);

                        
                        /*JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                        json = JsonSerializer.Serialize(ActivePasswords, options);
                        Console.WriteLine(json);*/
                        /*aes.EncryptToFile(json, path);*/
                        
                        
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
                            Console.WriteLine("type the website associated with your password");
                            string recherche = Console.ReadLine();
                            foreach (password pwd in currentpwd)
                            {
                                if (pwd.site.Equals(recherche))
                                {
                                    Console.WriteLine(pwd.id);
                                    Console.WriteLine(pwd.user_id);
                                    Console.WriteLine(pwd.site);
                                    Console.WriteLine(pwd.login);
                                    Console.WriteLine(pwd.Password);
                                    
                                    string Answer3;
                                    bool hidden = false;
                                    do
                                    {

                                        Console.WriteLine("voulez vous décripter(1), updater(2), supprimer(3), cacher le mdp déchiffrer(4) ou quitter(5)");
                                        Answer3 = Console.ReadLine();
                                        if (Answer3 == "1")
                                        {
                                            Console.WriteLine(pwd.id);
                                            Console.WriteLine(pwd.user_id);
                                            Console.WriteLine(pwd.site);
                                            Console.WriteLine(pwd.login);
                                            String decrypted = aes.Decrypt(pwd.Password);
                                            Console.WriteLine(decrypted);
                                        }

                                        if (Answer3 == "2")
                                        {
                                            Console.WriteLine("voules vous updater le username(1) ou le mdp(2)");
                                            string Answer4 = Console.ReadLine();
                                            if (Answer4 == "1")
                                            {
                                                Console.WriteLine("entrez votre nouveau login");
                                                pwd.login = Console.ReadLine();
                                                if (Online)
                                                {
                                                    Db.UpdatePassword(pwd.id, pwd.user_id,pwd.site, pwd.login, pwd.Password);
                                                }
                                                DbOf.UpdatePassword(pwd.id, pwd.user_id,pwd.site, pwd.login, pwd.Password);

                                            }
                                            else
                                            {
                                                Console.WriteLine("Vous allez générer un nouveau mot de passe ");
                                                pwd.Password = Console.ReadLine();
                                                pwd.Password = aes.Encrypt(pwd.Password);
                                                
                                                if (Online)
                                                {
                                                    Db.UpdatePassword(pwd.id, pwd.user_id,pwd.site, pwd.login, pwd.Password);
                                                }
                                                DbOf.UpdatePassword(pwd.id, pwd.user_id,pwd.site, pwd.login, pwd.Password);
                                            }
                                        }

                                        if (Answer3 == "4")
                                        {
                                            Console.Clear();
                                            if (!hidden)
                                            {
                                                Console.WriteLine(pwd.id);
                                                Console.WriteLine(pwd.user_id);
                                                Console.WriteLine(pwd.site);
                                                Console.WriteLine(pwd.login);
                                                Console.WriteLine("********");

                                                hidden = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine(pwd.id);
                                                Console.WriteLine(pwd.user_id);
                                                Console.WriteLine(pwd.site);
                                                Console.WriteLine(pwd.login);
                                                aes.Decrypt(pwd.Password);
                                                Console.WriteLine(pwd.Password);
                                                
                                                hidden = false;
                                            }
                                        }

                                        if (Answer3 == "3")
                                        {
                                            if (Online)
                                            {
                                                Db.DeletePassword(pwd.id);
                                            }
                                            else
                                            {
                                                DbOf.addToDeleteList(pwd.id);
                                            }
                                            DbOf.DeletePassword(pwd.id);
                                        }

                                        /*JsonSerializerOptions options = new JsonSerializerOptions
                                            { WriteIndented = true };
                                        json = JsonSerializer.Serialize(ActivePasswords, options);

                                        aes.EncryptToFile(json, path);*/
                                        if(Online)
                                        {
                                            currentpwd = Db.GetUserPasswords(CurrentUser.id);
                                        }
                                        else
                                        { 
                                            currentpwd = DbOf.GetUserPasswords(1);
                                        }

                                    } while (Answer3 != "5");
                                }
                            }
                        }
                        else if (Answer2 == "2")
                        {
                            foreach (password pwd in currentpwd)
                            {
                                Console.WriteLine(pwd.id);
                                Console.WriteLine(pwd.user_id);
                                Console.WriteLine(pwd.site);
                                Console.WriteLine(pwd.login);
                                Console.WriteLine(pwd.Password);
                            }
                        }
                        
                        
                        
                        Console.WriteLine("type the login of the password(1), see the list(2) or quit(3)");
                        Answer2 = Console.ReadLine();
                    } while (Answer2 != "3");
                }
            } while (answer != "3");

            currentpwd.Clear();
        }
    }
}
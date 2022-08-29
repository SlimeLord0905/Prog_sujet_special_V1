namespace crypto;

using System.Security.Cryptography;


public class MdpGenerator
{
    static string letters = "abcdefghijklmnopqrstuvwxyz";
    static string numbers = "1234567890";
    static string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    static string symbole = "!/$%?&*()";



    public static  string make_password(int length, bool num, bool symb, bool up)
    {
        string used_char = "";
        string password = "";

        used_char += letters;

        if (num)
        {
            used_char += numbers;
        }

        if (up)
        {
            used_char += upper;
        }

        if (symb)
        {
            used_char += symbole;

        }

        do
        {
            for (int j = 0; j < length; j++)
            {
                int i = (RandomNumberGenerator.GetInt32(0, used_char.Length));
                password += used_char[i];
            }
        } while (check_password(symb, num, up, password));

        return password;
    }

    static bool check_password(bool b1, bool b2, bool b3, string psw)
    {
        bool ok1 = false;
        bool ok2 = false;
        bool ok3 = false;
        bool ok4 = false;

        if (b1)
        {
            for (int i = 0; i < symbole.Length; i++)
            {
                if (psw.Contains(symbole[i]))
                {
                    ok1 = true;
                }
            }
        }

        if (!b1)
        {
            ok1 = true;
        }

        if (b2)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (psw.Contains(numbers[i]))
                {
                    ok2 = true;
                }
            }
        }

        if (!b2)
        {
            ok2 = true;
        }

        if (b3)
        {
            for (int i = 0; i < upper.Length; i++)
            {
                if (psw.Contains(upper[i]))
                {
                    ok3 = true;
                }
            }
        }

        if (!b3)
        {
            ok3 = true;
        }

        for (int i = 0; i < letters.Length; i++)
        {
            if (psw.Contains(letters[i]))
            {
                ok4 = true;
            }
        }


        if (ok1 && ok2 && ok3)
        {
            return false;
        }

        return true;
    }

}
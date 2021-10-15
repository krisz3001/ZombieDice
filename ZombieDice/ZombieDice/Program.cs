using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDice
{
    class DobozKocka
    {
        char[] pirosKocka = new char[] { 'a', 'l', 'l', 's', 's', 's', 'R' };
        char[] sargaKocka = new char[] { 'a', 'a', 'l', 'l', 's', 's', 'Y' };
        char[] zoldKocka = new char[] { 'a', 'a', 'a', 'l', 'l', 's', 'G' };
        List<char[]> l = new List<char[]>();
        List<char[]> agyKocka = new List<char[]>();
        char[][] dice = new char[3][];
        char[] oldal = new char[3];
        int player1_pont = 0;
        int player2_pont = 0;
        public int currentPlayer = 1;
        int pont = 0;
        int shotgun = 0;
        public DobozKocka()
        {
            for (int i = 0; i < 3; i++)
            {
                l.Add(pirosKocka);
            }
            for (int i = 0; i < 4; i++)
            {
                l.Add(sargaKocka);
            }
            for (int i = 0; i < 6; i++)
            {
                l.Add(zoldKocka);
            }
        }
        public void Dob()
        {
            Kiir();
            Random r = new Random();
            Console.WriteLine("A dobás eredménye:");
            for (int i = 0; i < 3; i++)
            {
                if (l.Count < 3)
                {
                    for (int j = 0; j < agyKocka.Count; j++)
                    {
                        l.Add(agyKocka[j]);
                    }
                }
                int randomKocka = r.Next(0,l.Count());
                int randomOldal = r.Next(0, l[randomKocka].Length-1);
                if (oldal[i] == 'l')
                {
                    oldal[i] = dice[i][randomOldal];
                    Console.WriteLine("{0} kocka, {1}", Szin(dice[i].Last()), Oldal(oldal[i]));
                    if (oldal[i] == 'a')
                    {
                        agyKocka.Add(dice[i]);
                        pont++;
                    }
                    if (oldal[i] == 's') shotgun++;
                    continue;
                }
                dice[i] = (l[randomKocka]);
                oldal[i] = l[randomKocka][randomOldal];
                Console.WriteLine("{0} kocka, {1}", Szin(l[randomKocka].Last()), Oldal(l[randomKocka][randomOldal]));
                if (l[randomKocka][randomOldal] == 'a')
                {
                    agyKocka.Add(l[randomKocka]);
                    pont++;
                }
                if (l[randomKocka][randomOldal] == 's') shotgun++;
                l.Remove(l[randomKocka]);
            }
            Stats();
        }
        public void Kiir()
        {
            Console.Clear();
            Console.WriteLine("----------------------ZombieDice----------------------");
            Console.WriteLine("Első játékos pontjai: {0}", player1_pont);
            Console.WriteLine("Második játékos pontjai: {0}", player2_pont);
            Console.WriteLine(currentPlayer == 1 ? "\nJelenleg az első játékos játszik.\n" : "\nJelenleg a második játékos játszik.\n");
        }
        public string Oldal(char c)
        {
            switch (c)
            {
                case 'a': return "Agy";
                case 'l': return "Lábnyomok";
                case 's': return "Lövés";
                default: return "";
            }
        }
        public string Szin(char c)
        {
            switch (c)
            {
                case 'R': return "Piros";
                case 'Y': return "Sárga";
                case 'G': return "Zöld";
                default: return "";
            }
        }
        public void Turn()
        {
            dice = new char[3][];
            oldal = new char[3];
            l.Clear();
            for (int i = 0; i < 3; i++)
            {
                l.Add(pirosKocka);
            }
            for (int i = 0; i < 4; i++)
            {
                l.Add(sargaKocka);
            }
            for (int i = 0; i < 6; i++)
            {
                l.Add(zoldKocka);
            }
            agyKocka.Clear();
            if (currentPlayer == 1)
            {
                player1_pont += pont;
                currentPlayer = 2;
            }
            else
            {
                player2_pont += pont;
                currentPlayer = 1;
            }
            pont = 0; shotgun = 0;
        }
        public void Stats()
        {
            Console.WriteLine("\nAgyak száma: " + pont);
            Console.WriteLine("Lövések száma: " + shotgun);
        }
        public bool Dead()
        {
            if (shotgun >= 3)
            {
                pont = 0;
                return true;
            }
            return false;
        }
        public bool Win()
        {
            if (player1_pont >= 13 || player2_pont >= 13)
            {
                Console.WriteLine(player1_pont >= 13 ? "Az első játékos megnyerte a játékot." : "A második játékos megnyerte a játékot.");
                return true;
            }
            return false;
        }
        public void Tartalom()
        {
            Console.WriteLine("A doboz tartalma:");
            l.GroupBy(x => x).ToList().ForEach(x=>Console.WriteLine(Szin(x.Key.Last()) + ": " + x.Count() + " db"));
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            DobozKocka d = new DobozKocka();
            Console.Title = "Zombie Dice";
            Console.SetWindowSize(54,25);
            Console.SetBufferSize(54,25);
            Console.CursorVisible = false;
            Console.WriteLine("----------------------ZombieDice----------------------");
            Console.WriteLine("         \"Egyél agyat! Ne lövesd le magad!\"");
            Console.WriteLine("\n   A játék kezdéséhez nyomd meg az <ENTER> gombot.");
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true);
            }
            bool game = true;
            d.Kiir();
            while (game)
            {
                d.Tartalom();
                Console.WriteLine("\nKockák húzása és dobás <SPACE>");
                key = Console.ReadKey(true);
                while (key.Key != ConsoleKey.Spacebar)
                {
                    key = Console.ReadKey(true);
                }
                d.Dob();
                if (d.Dead())
                {
                    Console.WriteLine("Lelőttek, pontjaid elvesztek.");
                    d.Turn();
                    Console.WriteLine(d.currentPlayer == 1 ? "Az első játékos következik. <SPACE>" : "A második játékos következik. <SPACE>");
                    key = Console.ReadKey(true);
                    while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.Spacebar)
                    {
                        key = Console.ReadKey(true);
                    }
                    d.Kiir();
                }
                else
                {
                    Console.WriteLine("\nTovábbmész? <Y/N>");
                    key = Console.ReadKey(true);
                    while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N)
                    {
                        key = Console.ReadKey(true);
                    }
                    if (key.Key == ConsoleKey.N)
                    {
                        d.Turn();
                        if (d.Win()) break;
                        Console.WriteLine(d.currentPlayer == 1 ? "Az első játékos következik. <SPACE>" : "A második játékos következik. <SPACE>");
                        key = Console.ReadKey(true);
                        while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.Spacebar)
                        {
                            key = Console.ReadKey(true);
                        }
                    }
                    if (d.Win()) break;
                    d.Kiir();
                }
            }
            
            Console.ReadKey();
        }
    }
}
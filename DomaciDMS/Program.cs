using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DomaciDMS
{
    class Program
    {
        /*DMS domaci automatizovan*/
        static void DomaciDMS()
        {
            StreamReader sr = new StreamReader("tablica.txt"); //Otvara se fajl u kom se nalazi tablica
            /*
              Kada kopiram tablicu u tablica.txt
              relacije su odvojene sa tab znakom             
             */
            int brojLinija = sr.ReadToEnd().Split(new char[] { '\n' }).Length; //broj linija koje fajl ima
            sr = new StreamReader("tablica.txt");
            int[] a = new int[brojLinija]; //pravi se lista 
            bool krajFajla = false;
            string[] relacije = new string[brojLinija];
            int brojac = 0;
            
            while (!krajFajla)
            {
                string linija = sr.ReadLine();
                string[] delovi = linija.Split('\t');
                a[brojac] = int.Parse(delovi[0]);
                relacije[brojac++] = string.Join("\t", delovi.Skip(1));
                krajFajla = sr.EndOfStream;
            }//uzima sve vrednosti

            Dictionary<int, int> dict = new Dictionary<int, int>(); //za mapu

            for (int i = 0; i < a.Length; i++)
            {
                dict[a[i]] = i;
            }//svaki broj je mapiran

            bool[,] matrica = new bool[a.Length, a.Length]; //matrica za relacije

            for (int i = 0; i < matrica.GetLength(0); i++)
            {
                for (int j = 0; j < matrica.GetLength(1); j++)
                {
                    matrica[i, j] = false;
                }
            }//popuni matricu sa false svuda
            for (int i = 0; i < matrica.GetLength(0); i++)
            {
                string[] delovi = relacije[i].Split('\t');
                for (int j = 0; j < matrica.GetLength(1); j++)
                {
                    matrica[i, j] = delovi[j] == "1";
                }
            }
            Console.WriteLine("Prvi zadatak: refleksivnost");
            foreach (var item in a)
            {
                if (!matrica[dict[item], dict[item]])
                {
                    Console.Write($"({item},{item}), ");
                    matrica[dict[item], dict[item]] = true;
                }
            }
            Console.WriteLine("\n\nDrugi zadatak: simetricnost");
            foreach (var item1 in a)
            {
                foreach (var item2 in a)
                {
                    if (matrica[dict[item1], dict[item2]] != matrica[dict[item2], dict[item1]])
                    {
                        Console.Write(!matrica[dict[item1], dict[item2]] ? $"({item1},{item2}), " : $"({item2},{item1}), ");
                        if (!matrica[dict[item1], dict[item2]]) matrica[dict[item1], dict[item2]] = true;
                        else matrica[dict[item2], dict[item1]] = true;
                    }
                }
            }
            Console.WriteLine($"\n\nTreci zadatak: ekvivalencija");

            bool changed;

            do
            {
                changed = false;
                foreach (var x in a)
                {
                    foreach (var y in a)
                    {
                        foreach (var z in a)
                        {
                            if (matrica[dict[x], dict[y]] && matrica[dict[y], dict[z]] && !matrica[dict[x], dict[z]])
                            {
                                Console.Write($"({x},{z}), ");
                                matrica[dict[x], dict[z]] = true;
                                changed = true;
                            }
                        }
                    }
                }
            } while (changed);
            Console.WriteLine("\n\nZadatak 4: Ukupan broj grana u R_rst:");

            int brojGrana = 0;

            foreach (var i in a)
            {
                foreach (var j in a)
                {
                    if (matrica[dict[i], dict[j]]) brojGrana++;
                }
            }
            Console.WriteLine($"Ukupno grana: {brojGrana}");
            Console.WriteLine("\nZadatak 5: Klase ekvivalencije");

            HashSet<int> obradjeni = new HashSet<int>();

            foreach (var x in a)
            {
                if (!obradjeni.Contains(x))
                {
                    var klasa = new List<int>();
                    foreach (var y in a)
                    {
                        if (matrica[dict[x], dict[y]])
                        {
                            klasa.Add(y);
                            obradjeni.Add(y);
                        }
                    }
                    Console.Write("{");
                    for (int i = 0; i < klasa.Count; i++)
                    {
                        Console.Write(klasa[i]);
                        if (i < klasa.Count - 1) Console.Write(",");
                    }
                    Console.Write("}, ");
                }
            }
            Print2DBoolSpecific(matrica, a, dict);
        }//domacidms
        static void Print2DBoolSpecific(bool[,] matrica, int[] a, Dictionary<int,int> dict)
        {
            Console.WriteLine("\n\n\n");
            foreach (var item in a)
            {
                Console.Write(item + ",");
            }
            Console.WriteLine("\n\n");
            foreach (var item in a)
            {
                foreach (var item2 in a)
                {
                    if (matrica[dict[item], dict[item2]]) Console.Write($"{item},{item2} ");
                }
            }
        }//print2dboolspecific
        static void Printuj2DBool(bool[,] matrica, int[] a, Dictionary<int, int> dict)
        {
            Console.WriteLine("\n\n");
            Console.Write("   ");
            foreach (var item in a)
            {
                Console.Write(item + " ");
            }
            foreach (var item in a)
            {
                Console.Write($"\n{item} ");
                foreach (var item2 in a)
                {
                    Console.Write(matrica[dict[item], dict[item2]] ? "1 " : "0 ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }//printuj2dbool
        static void Main(string[] args)
        {
            DomaciDMS();
        }
    }
}

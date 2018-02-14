using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cpts321;

namespace Walsh_HW6
{
    class Program
    {
        static private ExpTree expTree;
        static private string exp;
        static void Main(string[] args)
        {
            expTree = new ExpTree("");
            menu();
        }
        static void menu()
        {
            bool end = false;
            while (end == false)
            {
                Console.WriteLine("Menu (Current Expression = \"" + exp + "\")");
                Console.WriteLine("1 = Enter a new expression");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate tree");
                Console.WriteLine("4 = Quit");
                string userInput = Console.ReadLine();


                if (int.TryParse(userInput, out var data))
                {
                    switch (data)
                    {
                        case 1:
                            Console.Write("Enter new expression: ");
                            expTree.Clear();
                            exp = Console.ReadLine();
                            expTree = new ExpTree(exp);
                            break;

                        case 2:
                            Console.Write("Enter variable name: ");
                            string name = Console.ReadLine();
                            bool isInt = false;
                            while (!isInt)
                            {
                                Console.Write("Enter variable value: ");
                                string value = Console.ReadLine();
                                if (double.TryParse(value, out var v))
                                {
                                    expTree.SetVar(name, v);
                                    isInt = true;
                                }
                                else
                                    Console.WriteLine("Invalid Input");
                            }
                            break;
                        case 3:
                            double result = expTree.Eval();
                            Console.WriteLine("{0}", result);
                            break;

                        case 4:
                            end = true;
                            break;
                    }
                }
            }
        }
    }
}

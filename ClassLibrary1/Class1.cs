using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cpts321
{
    public class ExpTree
    {
        private static Dictionary<string, double> ExpDict = new Dictionary<string, double>();
        private Node treeRoots;
        public readonly static char[] operators = { '+', '-', '*', '/' };


        private static Node MakeTree(string s, char op)
        {
            int pcounter = 0; // parenthesis counter

            for (int i = s.Length - 1; i >= 0; i--) // 0 based indexing, start from right of the string
            {
                // keep count of total parentesis open and closed
                if (')' == s[i]) // check if closing parenthesis
                    pcounter++;
                else if ('(' == s[i]) // check for open parenthesis
                    pcounter--;

                // no mismatching parenthesis set op to a valid op, do not want to set parenthesis to opnode
                if (0 == pcounter && op == s[i])
                {
                    opNode on = new opNode(s[i], MakeTree(s.Substring(0, i)), MakeTree(s.Substring(i + 1))); // make a new opnode
                    return on;
                }
            }

            if (pcounter != 0) // no matching parenthesis
            {
                if (pcounter < 0)
                    throw new Exception("Missing 1 or more closing parenthesis");
                else
                    throw new Exception("missing 1 or more opening parenthesis");
            }

            return null; // no node created
        }

        // make tree based off of the current string
        private static Node MakeTree(string s)
        {
            if (string.IsNullOrEmpty(s)) // no string, return no node
                return null;

            // get rid of parenthesis
            if ('(' == s[0])
            {
                int counter = 0;
                for (int i = 1; i < s.Length; i++) // check for other parenthesis
                {
                    if ('(' == s[i])
                    {
                        counter++;
                    }
                    else if (')' == s[i])
                    {
                        counter--;
                        if (0 == counter) // send items inside parenthesis for current string
                        {
                            // not last char
                            if (i != s.Length - 1)
                            {
                                break;
                            }
                            else if (s[0] == '(')
                            {
                                //make tree again ( based off the stringwithout outside parenthesis)
                                return MakeTree(s.Substring(1, s.Length - 2));
                            }
                            else
                                break; // otherwise does not need to create a substring again, go operator loop
                        }
                    }
                    // make tree again (based off the string without outside parenthesis)
                    if (')' == s[s.Length - 1] && s[0] == '(')
                    {
                        s = s.Substring(1, s.Length - 2);
                        //counter = 0;  // reset counter to 0 since parenthesis removed;
                    }
                }
            }
            // valid op characters in reverse PEMDAS order
            char[] ops = { '+', '-', '*', '/', '^' };

            //send the string to search for each valid operator allowed
            foreach (char op in ops)
            {
                Node n = MakeTree(s, op); // make the tree based off the current operator
                if (n != null) return n; // if a node was returned, return that node to the calling function
            }

            double num; // try to make the string as a value
            if (double.TryParse(s, out num)) // if it works
            {
                return new numNode(num); // return a cNode (constant node)
            }
            else
            {
                return new varNode(s); // otherwise it has letters so make a variable node
            }
        }


        public abstract class Node
        {
            public abstract double Eval();
        }

        public class varNode : Node
        {
            private string Names;
            public varNode(string name) //creats varNode
            {
                this.Names = name;
                ExpDict[Names] = 0;
            }

            public override double Eval()
            {
                return ExpDict[Names];
            }
        }

        public class opNode : Node
        {
            private char Nodes;
            private Node Lefts;
            private Node Rights; //node values

            public opNode(char op, Node left, Node right)
            {
                this.Lefts = left; //create opNode
                this.Rights = right;
                this.Nodes = op;
            }

            public override double Eval()
            {
                switch (Nodes) //Evaluates the expression tree
                {
                    case '/':
                        return this.Lefts.Eval() / this.Rights.Eval();
                    case '*':
                        return this.Lefts.Eval() * this.Rights.Eval();
                    case '-':
                        return this.Lefts.Eval() - this.Rights.Eval();
                    case '+':
                        return this.Lefts.Eval() + this.Rights.Eval();
                }
                return 0;
            }
        }

        public class numNode : Node
        {
            private double valueing;
            public numNode(double value)
            {
                this.valueing = value; //makes numNode with value
            }

            public override double Eval()
            {
                return valueing;
            }
        }

        public void SetVar(string varName, double varValue)
        {
            if (ExpDict.ContainsKey(varName)) //Sets the specified variable variable within the ExpTree variables dictionary
                ExpDict[varName] = varValue;
            else
                ExpDict.Add(varName, varValue);
        }

        public ExpTree(string Exp)
        {
            treeRoots = MakeTree(Exp); //calls to make the tree with the input expression
        }

        public double Eval()
        {
            if (treeRoots != null) //evaluates expression to a double value
                return treeRoots.Eval();

            else
                return 0.0;
        }

        public void Clear()
        {
            ExpDict.Clear(); //clears the dictionary
        }
    }
}
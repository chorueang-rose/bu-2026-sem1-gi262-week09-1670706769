using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment
{
    public class StudentSolution
    {
        class Action
        {
            public string Name;
            public int Value;
        }

        #region Lecture

        public (int Count, int Popped, int Peek, int CountAfterPeek) LCT01_StackSyntax()
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            int count = stack.Count;
            Debug.Log($"count: {count}");
            int popped = stack.Pop();
            Debug.Log($"pop: {popped}");
            int peek = stack.Peek();
            int countAfterPeek = stack.Count;

            return ( count, popped, peek, countAfterPeek);
        }

        public (int Count, int Dequeued, int Peek, int CountAfterPeek) LCT02_QueueSyntax()
        {
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            int count = queue.Count;
            int dequeued = queue.Dequeue();
            int peek = queue.Peek();
            int countAfterPeek = queue.Count;

            return (count, dequeued, peek, countAfterPeek);
        }

        public string[] LCT03_ActionStack()
        {
            Action action1 = new Action { Name = "Action 1" };
            Action action2 = new Action { Name = "Action 2" };
            Action action3 = new Action { Name = "Action 3" };
            
            Stack<Action> undoStack = new Stack<Action>();
            undoStack.Push(action1);
            undoStack.Push(action2);
            undoStack.Push(action3);

            int c = undoStack.Count;
            string[] history = new string[c];
            for (int i = 0; i < c; i++)
            {
                var popped = undoStack.Pop();
                history[i] = popped.Name;
            }
            return history;
        }

        public string[] LCT04_ActionQueue()
        {
            Action action1 = new Action { Name = "Action 1" };
            Action action2 = new Action { Name = "Action 2" };
            Action action3 = new Action { Name = "Action 3" };
            
            Queue<Action> queue = new Queue<Action>();
            queue.Enqueue(action1);
            queue.Enqueue(action2);
            queue.Enqueue(action3);

            int c = queue.Count;
            string[] actionList = new string[c];
            for (int i = 0; i < c; i++)
            {
                var dequeued = queue.Dequeue();
                actionList[i] = dequeued.Name;
            }
            return actionList;
        }

        #endregion

        #region Assignment

        public string ASN01_ReverseString(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            Stack<char> stack = new Stack<char>();
            foreach (char c in str)
            {
                stack.Push(c);
            }

            char[] reversedChars = new char[stack.Count];
            int index = 0;
            while (stack.Count > 0)
            {
                reversedChars[index++] = stack.Pop();
            }

            return new string(reversedChars);
        }

        public bool ASN02_StackPalindrome(string str)
        {
            string reversed = ASN01_ReverseString(str);
            return str == reversed;
        }

        #endregion

        #region Extra

        public bool EX01_ParenthesesChecker(string str)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in str)
            {
                if (c == '(' || c == '[' || c == '{')
                {
                    stack.Push(c);
                }
                else if (c == ')' || c == ']' || c == '}')
                {
                    if (stack.Count == 0) return false;

                    char top = stack.Pop();
                    if ((c == ')' && top != '(') ||
                        (c == ']' && top != '[') ||
                        (c == '}' && top != '{'))
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }
    }

        #endregion
    }

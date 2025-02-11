namespace Kfz
{
    public class ConsoleHelper : IConsoleHelper
    {
        private readonly string CANCEL_STRING = "q";
        private readonly string CONFIRM_STRING = "Ja";

        public void InvalidUserInputMessage()
        {
            Console.WriteLine("Sie haben eine invalide Eingabe getätigt. Bitte versuchen Sie es erneut.");
        }

        public bool Confirm(string? userHint = null)
        {
            if (userHint == null)
            {
                userHint = "Möchten Sie die Operation bestätigen? (Ja/q)";
            }

            return InputString(userHint, s => CONFIRM_STRING.Equals(s, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public string? InputString(string userHint, Func<string, bool>? validationFunction = null)
        {
            Console.WriteLine(userHint);

            string? userInput = null;
            while (string.IsNullOrEmpty(userInput))
            {
                userInput = Console.ReadLine();

                if (CANCEL_STRING.Equals(userInput))
                {
                    return null;
                }

                if (validationFunction != null && userInput != null && !validationFunction(userInput))
                {
                    userInput = null;
                    InvalidUserInputMessage();
                }
            }
            return userInput;
        }

        public DateOnly? InputDate(string userHint)
        {
            Console.WriteLine(userHint);

            bool dateSuccessfullyParsed = false;
            while (!dateSuccessfullyParsed)
            {
                string? userInput = Console.ReadLine();

                if (CANCEL_STRING.Equals(userInput))
                {
                    return null;
                }

                dateSuccessfullyParsed = DateOnly.TryParseExact(userInput, "dd.MM.yyyy", out DateOnly date);

                if (!dateSuccessfullyParsed)
                {
                    InvalidUserInputMessage();
                } else
                {
                    return date;
                }
            }

            return null;
        }

        public T? PickSingleItemFromList<T>(IList<T> items, Func<T, string> outputFunc)
        {
            for (int i = 0; i < items.Count; i++)
            {
                T item = items[i];
                Console.WriteLine($"{i + 1}) {outputFunc(item)}");
            }

            int index = -1;
            while (index == -1)
            {
                string? userInput = Console.ReadLine();

                if (CANCEL_STRING.Equals(userInput))
                {
                    return default;
                }

                if(int.TryParse(userInput, out index))
                {
                    index--;
                    if (index < 0 || index >= items.Count())
                    {
                        index = -1;
                        InvalidUserInputMessage();
                    }
                } else
                {
                    index = -1;
                    InvalidUserInputMessage();
                }
            }
            return items[index];
        }

        public IList<T>? PickMultipleItemsFromList<T>(IList<T> items, Func<T, string> outputFunc) where T : class
        {
            IList<T> returns = [];

            bool canceled = false;
            while (!canceled)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    T item = items[i];
                    Console.WriteLine($"{i + 1}) {outputFunc(item)}");
                }
                Console.WriteLine($"0) -- Auswahl beenden --");

                int index = -2;
                while (index == -2)
                {
                    string? userInput = Console.ReadLine();

                    if (CANCEL_STRING.Equals(userInput))
                    {
                        return null;
                    }

                    if(int.TryParse(userInput, out index))
                    {
                        index--;
                        if (index < -1 || index >= items.Count())
                        {
                            index = -2;
                            InvalidUserInputMessage();
                        }
                    } else
                    {
                        index = -2;
                        InvalidUserInputMessage();
                    }

                }
                if (index == -1)
                {
                    canceled = true;
                }
                else
                {
                    returns.Add(items[index]);
                    items.RemoveAt(index);
                }
            }
            return returns;
        }

        public int PickOption(IList<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i+1}) {items[i]}");
            }

            int index = -1;
            while (index == -1)
            {
                string? userInput = Console.ReadLine();

                if (CANCEL_STRING.Equals(userInput))
                {
                    return -1;
                }

                if(int.TryParse(userInput, out index))
                {

                    index--;
                    if (index < 0 || index >= items.Count())
                    {
                        index = -1;
                        InvalidUserInputMessage();
                    }
                } else
                {
                    index = -1;
                    InvalidUserInputMessage();
                }
            }
            return index;
        }
    }
}

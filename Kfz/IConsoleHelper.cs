namespace Kfz
{
    public interface IConsoleHelper
    {
        public void InvalidUserInputMessage();

        public bool Confirm(string? userHint);

        public string? InputString(string userHint, Func<string, bool>? validationFunction = null);

        public DateOnly? InputDate(string userHint);

        public T? PickSingleItemFromList<T>(IList<T> items, Func<T, string> outputFunc);

        public IList<T>? PickMultipleItemsFromList<T>(IList<T> items, Func<T, string> outputFunc) where T : class;
        public int PickOption(IList<string> items);
    }
}

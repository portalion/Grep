namespace codecrafters_grep.src
{
    public static class Extensions
    {
        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            var array = new T[stack.Count];
            stack.CopyTo(array, 0);
            Array.Reverse(array);
            return new Stack<T>(array);
        }

        public static Stack<T> Merge<T>(this Stack<T> stack, Stack<T> other)
        {
            var secondStack = other.Reverse();
            foreach (var item in secondStack)
            {
                stack.Push(item);
            }
            return stack;
        }
    }
}

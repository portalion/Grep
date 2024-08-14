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
    }
}

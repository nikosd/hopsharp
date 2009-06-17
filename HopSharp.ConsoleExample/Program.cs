namespace HopSharp.ConsoleExample
{
    using System;

    /// <summary>
    /// A super program that throws an exception.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                new Exceptioner().Throw();
            }
            catch (Exception ex)
            {
                ex.SendToHoptoad();
            }
        }
    }

    /// <summary>
    /// A super class that has a super method which throws an exception.
    /// </summary>
    public class Exceptioner
    {
        /// <summary>
        /// Throw an exception!
        /// </summary>
        /// <exception cref="SomeCustomException">
        /// <c>SomeCustomException</c>.
        /// </exception>
        public void Throw()
        {
            throw new SomeCustomException();
        }
    }

    /// <summary>
    /// A very special custom exception...
    /// </summary>
    internal class SomeCustomException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public SomeCustomException() : base("And a custom exception (arrriiibaaaaaa!!!!)")
        {
        }
    }
}
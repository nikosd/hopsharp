using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HopSharp.ConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Exceptioner().Throw();
            }
            catch(Exception ex)
            {
                ex.SendToHoptoad();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Exceptioner
    {
        /// <exception cref="SomeCustomException"><c>SomeCustomException</c>.</exception>
        public void Throw()
        {
            throw new SomeCustomException();
        }
    }

    internal class SomeCustomException : ApplicationException
    {
        public SomeCustomException() : base("And a custom exception (arrriiibaaaaaa!!!!)"){}
    }
}
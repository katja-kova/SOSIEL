/// Name: InputParameterException.cs
/// Description:
/// Authors: Multiple.
/// Last updated: July 10th, 2020.
/// Copyright: Garry Sotnik

using System;

namespace SOSIEL.Exceptions
{
    public class InputParameterException: Exception
    {
        private string parameter;
        private string message;


        public InputParameterException(string parameter, string message)
        {
            this.parameter = parameter;
            this.message = message;
        }

        public override string ToString()
        {
            return string.Format("Value of {0} is wrong. {1}", parameter, message);
        }
    }
}

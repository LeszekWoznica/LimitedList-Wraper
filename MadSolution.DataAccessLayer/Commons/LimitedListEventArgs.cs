using System;

namespace MadSolution.DataAccessLayer.Commons
{
    public class LimitedListEventArgs : EventArgs       
    {
        public string Message { get; }
        public LimitedListEventArgs(string message) => Message = message;
    }
}

using System;

namespace PracticumEmailer.Business
{
    public class Clearances
    {
        public string TbExpiration { get; set; }
        public string FcsrExpiration { get; set; }
        public string FbiExpiration { get; set; }
        public string PliExpiration { get; set; }

        public bool IsFcsrValid(DateTime cutOff)
        {
            return false;
        }

        public bool IsFbiValid(DateTime cutOff)
        {
            return false;
        }

        public bool IsTbValid(DateTime cutOff)
        {
            return false;
        }

        public bool IsPliValid(DateTime cutOff)
        {
            return false;
        }
    }
}

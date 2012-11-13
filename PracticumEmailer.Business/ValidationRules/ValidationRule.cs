using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PracticumEmailer.Business.ValidationRules
{
    public abstract class ValidationRule
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public abstract bool Validate(BusinessObject businessObject);
    }
}

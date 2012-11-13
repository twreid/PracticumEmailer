using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PracticumEmailer.Business.ValidationRules;

namespace PracticumEmailer.Business
{
    public abstract class BusinessObject
    {
        private readonly IList<ValidationRule> _validationRules = new List<ValidationRule>();
        private readonly IList<string> _errors = new List<string>(); 

        public IList<string> ValidationErrors
        {
            get { return _errors; }
        }

        protected void AddRule(ValidationRule rule)
        {
            _validationRules.Add(rule);
        }

        public bool Validate()
        {
            bool isValid = true;

            foreach (var validationRule in _validationRules.Where(v => !v.Validate(this)))
            {
                isValid = false;
                _errors.Add(validationRule.ErrorMessage);
            }

            return isValid;
        }
    }
}

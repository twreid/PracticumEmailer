using System.ComponentModel.Composition;

namespace PracticumEmailer.Data
{
    public class DataFactory
    {
        [Import]
        public IStudentDataAccess StudentDataAccess { get; set; }
    }
}

using System.ComponentModel;

namespace PracticumEmailer
{
    public class Course : INotifyPropertyChanged
    {

        private string _course;
        private bool _fbi;
        private bool _tb;
        private bool _fcsr;
        private bool _liab;
        private bool _isPracticum;

        public event PropertyChangedEventHandler PropertyChanged;

        public Course()
        {
            _course = "";
            _fbi = false;
            _tb = false;
            _liab = false;
            _fcsr = false;
            _isPracticum = false;
        }

        public string CourseId
        {
            get { return _course; }
            set 
            { 
                _course = value;
                this.NotifyPropertyChanged("CourseId");
            }
            
        }

        public bool Fbi
        {
            get { return _fbi; }
            set 
            { 
                _fbi = value;
                this.NotifyPropertyChanged("Fbi");
            }
        }

        public bool Tb
        {
            get { return _tb; }
            set 
            { 
                _tb = value;
                this.NotifyPropertyChanged("Tb");
            }
        }

        public bool Fcsr
        {
            get { return _fcsr; }
            set 
            { 
                _fcsr = value;
                this.NotifyPropertyChanged("Fcsr");
            }
        }

        public bool Liab
        {
            get { return _liab; }
            set 
            { 
                _liab = value;
                this.NotifyPropertyChanged("Liab");
            }
        }

        public bool IsPracticum
        {
            get { return _isPracticum; }
            set 
            { 
                _isPracticum = value;
                this.NotifyPropertyChanged("IsPracticum");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        


        
    }
}

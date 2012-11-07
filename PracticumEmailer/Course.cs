using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PracticumEmailer
{
    public class Course : INotifyPropertyChanged
    {

        private string course;
        private bool fbi;
        private bool tb;
        private bool fcsr;
        private bool liab;
        private bool isPracticum;

        public event PropertyChangedEventHandler PropertyChanged;

        public Course()
        {
            course = "";
            fbi = false;
            tb = false;
            liab = false;
            fcsr = false;
            isPracticum = false;
        }

        public string CourseId
        {
            get { return course; }
            set 
            { 
                course = value;
                this.NotifyPropertyChanged("CourseId");
            }
            
        }

        public bool FBI
        {
            get { return fbi; }
            set 
            { 
                fbi = value;
                this.NotifyPropertyChanged("FBI");
            }
        }

        public bool TB
        {
            get { return tb; }
            set 
            { 
                tb = value;
                this.NotifyPropertyChanged("TB");
            }
        }

        public bool FCSR
        {
            get { return fcsr; }
            set 
            { 
                fcsr = value;
                this.NotifyPropertyChanged("FCSR");
            }
        }

        public bool LIAB
        {
            get { return liab; }
            set 
            { 
                liab = value;
                this.NotifyPropertyChanged("LIAB");
            }
        }

        public bool IsPracticum
        {
            get { return isPracticum; }
            set 
            { 
                isPracticum = value;
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

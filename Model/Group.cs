using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FinalBPM.Model
{
    public class Group<T> : ObservableCollection<T>
    {
        public Group(string name, string shortName = "")
        {
            this.Name = name;
            this.ShortName = shortName;
        }


        public string Name { get; }
        public string ShortName { get; }
    }

}

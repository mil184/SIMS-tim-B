using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class CheckpointDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsChecked { get; set; }
        public bool ButtonIsEnabled { get; set; }

        public CheckpointDTO() 
        {

        }

        public CheckpointDTO(int id, string name, int order)
        {
            Id = id;
            Name = name;
            Order = order;
            IsEnabled = false;
            IsChecked = false;
            ButtonIsEnabled = false;
        }  
        
    }
}

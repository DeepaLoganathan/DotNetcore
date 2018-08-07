using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample.Model
{
    public class DropdownModel
    {
        public string Value;
        public string Text;
        public List<DropdownModel> List() {
            List<DropdownModel> list_items = new List<DropdownModel>();
            list_items.Add(new DropdownModel { Text = "1.0", Value = "netcoreapp1.0" });
            list_items.Add(new DropdownModel { Text = "2.0", Value = "netcoreapp2.0" });
            return list_items;
        }
    }

    public class DropdownModel_Sync
    {
        public string Value;
        public string Text;
        public List<DropdownModel_Sync> List()
        {
            List<DropdownModel_Sync> list_items = new List<DropdownModel_Sync>();
            list_items.Add(new DropdownModel_Sync { Text = "16.1.0.37", Value = "16.1.0.37" });
            list_items.Add(new DropdownModel_Sync { Text = "16.2.0.41", Value = "16.2.0.41" });
            return list_items;
        }
    }

}

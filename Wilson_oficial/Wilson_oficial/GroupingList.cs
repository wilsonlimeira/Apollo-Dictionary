using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wilson_oficial
{
    public class GroupingList<TKey, TItem> : ObservableCollection<TItem>
    {
        public TKey Key { get; set; }

        public GroupingList(TKey key, IEnumerable<TItem> items)
        {
            this.Key = key;
            foreach (var item in items)
            {
                this.Items.Add(item);
            }
        }
    }
}

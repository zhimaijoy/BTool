﻿using System;
using System.Collections;
using System.Windows.Forms;

public class ListViewSort : IComparer
{
    private int columnToSort = 0;
    private CaseInsensitiveComparer objectCompare = new CaseInsensitiveComparer();
    private SortOrder orderOfSort = SortOrder.None;

    public int Compare(object x, object y)
    {
        ListViewItem item = (ListViewItem) x;
        ListViewItem item2 = (ListViewItem) y;
        if (((item.SubItems.Count - 1) >= this.columnToSort) && ((item2.SubItems.Count - 1) >= this.columnToSort))
        {
            int num = this.objectCompare.Compare(item.SubItems[this.columnToSort].Text, item2.SubItems[this.columnToSort].Text);
            if (this.orderOfSort == SortOrder.Ascending)
            {
                return num;
            }
            if (this.orderOfSort == SortOrder.Descending)
            {
                return -num;
            }
        }
        return 0;
    }

    public SortOrder Order
    {
        get
        {
            return this.orderOfSort;
        }
        set
        {
            this.orderOfSort = value;
        }
    }

    public int SortColumn
    {
        get
        {
            return this.columnToSort;
        }
        set
        {
            this.columnToSort = value;
        }
    }
}


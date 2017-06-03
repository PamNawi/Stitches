using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Report
{
    public string text;
    public int showNumbers;

    public void showTime()
    {
        showNumbers--;
    }
    public bool isOld()
    {
        if (showNumbers == 0)
            return true;
        return false;
    }


}
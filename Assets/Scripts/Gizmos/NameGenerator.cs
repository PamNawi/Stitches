using System.Collections.Generic;
using UnityEngine;

public class NameGenerator {
    public List<string> firstNameSyllables  = new List<string>();
    public List<string> lastNameSyllables   = new List<string>();
    public NameGenerator()
    {
        firstNameSyllables.Add("mon");
        firstNameSyllables.Add("fay");
        firstNameSyllables.Add("shi");
        firstNameSyllables.Add("zag");
        firstNameSyllables.Add("blarg");
        firstNameSyllables.Add("rash");
        firstNameSyllables.Add("izen");
        firstNameSyllables.Add("tum");

        lastNameSyllables.Add("malo");
        lastNameSyllables.Add("zak");
        lastNameSyllables.Add("abo");
        lastNameSyllables.Add("wonk");
        lastNameSyllables.Add("bo");
    }

    public string createNewName()
    {
        string firstName = "";
        string lastName  = "";
        int numFirstSyllables = firstNameSyllables.Count;
        int numLastSyllabels = lastNameSyllables.Count;
        for (int i = 0; i< 3; i++)
        {
            firstName += firstNameSyllables[(int) (Random.value *  numFirstSyllables)];
            lastName += lastNameSyllables[(int)(Random.value * numLastSyllabels)];
        }
        firstName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName);
        lastName  = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName);
        return firstName + " " + lastName;
    }
}

using UnityEngine;
using System.Collections;
using LINQtoCSV;

public class LocalizationEntry
{
    [CsvColumn(Name = "Reference", FieldIndex = 1)]
    public string Reference { get; set; }

    [CsvColumn(Name = "Arabic", FieldIndex = 2)]
    public string Arabic { get; set; }

    [CsvColumn(Name = "English", FieldIndex = 3)]
    public string English { get; set; }

    [CsvColumn(Name = "Turkish", FieldIndex = 4)]
    public string Turkish { get; set; }
}

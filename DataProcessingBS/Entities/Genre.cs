using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace DataProcessingBS.Entities;

public class Genre
{
    [Key] 
    public int Genre_Id { get; set; }

    public string Type { get; set; }
}
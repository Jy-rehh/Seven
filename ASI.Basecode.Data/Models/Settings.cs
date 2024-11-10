using System;
using System.ComponentModel.DataAnnotations;  // Required for [Key] attribute

namespace ASI.Basecode.Data.Models
{
    public class Settings
    {
        [Key]  // Marking this property as the primary key
        public int SettingsId { get; set; }  // Primary key property

        public string Username { get; set; }  // Example field for username
        public string Field1 { get; set; }  // Example field 1
        public string Field2 { get; set; }  // Example field 2
    }
}




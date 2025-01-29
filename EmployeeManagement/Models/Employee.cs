using System.ComponentModel.DataAnnotations;

using System;
using System.Xml.Serialization;
//Add by Anjal Agrawal
namespace EmployeeManagement.Models
{
    [XmlRoot("Employee")]
    public class Employee
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("Department")]
        public Dept? Department { get; set; }
    }
}

using System
using System.Collections.Genenric
namespace BlazorApp.Data {

    public class Employee
    {
        private string Vorname { get; set; }
        private string Nachname { get; set; }
        private List<string> Projekte { get; set; }
        private DateTime Geburtstag { get; set; }
    }
}
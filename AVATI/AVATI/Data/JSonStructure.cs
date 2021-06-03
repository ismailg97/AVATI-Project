using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AVATI.Data
{
    public class JSonStructure
    {
        public class SprachenundFrameworks
        {
            public List<string> Sprachen { get; set; }
            public List<string> Frameworks { get; set; }
            public List<string> Bibliotheken { get; set; }
        }

        public class Tools
        {
            [JsonPropertyName("Projektmanagement Tools")]
            public List<string> ProjektmanagementTools { get; set; }

            [JsonPropertyName("Konzept- und Ideenmanagement")]
            public List<string> KonzeptUndIdeenmanagement { get; set; }

            [JsonPropertyName("CI/CD")] public List<string> CICD { get; set; }
            public List<string> IDEs { get; set; }
            public List<string> Versionsverwaltung { get; set; }
            [JsonPropertyName("Digitales Design")] public List<string> DigitalesDesign { get; set; }

            [JsonPropertyName("3D-Visualisierung")]
            public List<string> _3DVisualisierung { get; set; }

            public List<string> Virtualisierung { get; set; }

            [JsonPropertyName("Server- und Infrastruktur-Management")]
            public List<string> ServerUndInfrastrukturManagement { get; set; }

            public List<string> QA { get; set; }
            public List<string> CAD { get; set; }

            [JsonPropertyName("Package-Management")]
            public List<string> PackageManagement { get; set; }

            [JsonPropertyName("Build-Management")] public List<string> BuildManagement { get; set; }

            [JsonPropertyName("Datenvisualisierung/-analyse")]
            public List<string> DatenvisualisierungAnalyse { get; set; }

            public List<string> Simulation { get; set; }

            [JsonPropertyName("Optimierung/technische Mathematik")]
            public List<string> OptimierungTechnischeMathematik { get; set; }

            public List<string> Datenbankverwaltung { get; set; }
        }

        public class MethodenundProzesse
        {
            [JsonPropertyName("Agile Methoden")] public List<string> AgileMethoden { get; set; }
            [JsonPropertyName("Agile Scaling")] public List<string> AgileScaling { get; set; }

            [JsonPropertyName("UI/UX")] public List<string> UIUX { get; set; }

            [JsonPropertyName("Software Engineering")]
            public List<string> SoftwareEngineering { get; set; }

            public List<string> Modellierung { get; set; }
            public List<string> Projektmanagement { get; set; }
        }

        public class BetriebssystemeCloudPlattformenHardware
        {
            public List<string> Betriebssysteme { get; set; }
            public List<string> Cloud { get; set; }
            public List<string> Hardware { get; set; }
            public List<string> Plattformen { get; set; }
        }

        public class Skills
        {
            [JsonPropertyName("Sprachen und Frameworks")]
            public SprachenundFrameworks SprachenundFrameworks { get; set; }

            public Tools Tools { get; set; }

            [JsonPropertyName("Methoden und Prozesse")]
            public MethodenundProzesse MethodenundProzesse { get; set; }

            public List<string> Datenbanken { get; set; }

            [JsonPropertyName("Betriebssysteme/Cloud/Plattformen/Hardware")]
            public BetriebssystemeCloudPlattformenHardware BetriebssystemeCloudPlattformenHardware { get; set; }

            [JsonPropertyName("Schnittstellen und Protokolle")]
            public List<string> SchnittstellenundProtokolle { get; set; }

            public List<string> Expertise { get; set; }
        }

        public class Rootclass
        {
            public List<string> fields { get; set; }
            public List<string> roles { get; set; }
            public List<string> languages { get; set; }
            public Skills skills { get; set; }
            public List<string> Softskills { get; set; }
        }
    }
}
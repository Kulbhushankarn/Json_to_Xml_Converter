using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "C:\\Users\\lenovo\\Downloads\\myfile.json";
        string jsonString = File.ReadAllText(jsonFilePath);

        // Convert JSON to XML
        string xmlString = ConvertJsonToXml(jsonString);

        // Write XML to file
        string xmlFilePath = "C:\\Users\\lenovo\\Downloads\\output.xml";
        File.WriteAllText(xmlFilePath, xmlString, Encoding.UTF8);

        Console.WriteLine("JSON file converted to XML successfully!");
    }

    static string ConvertJsonToXml(string jsonString)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.Formatting = Formatting.Indented;

                // Parse JSON
                using (var jsonDoc = JsonDocument.Parse(jsonString))
                {
                    // Start writing XML
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("company");

                    // Write company details
                    var root = jsonDoc.RootElement;
                    xmlWriter.WriteElementString("company", root.GetProperty("company").GetString());

                    // Write employees
                    xmlWriter.WriteStartElement("employees");
                    foreach (var employee in root.GetProperty("employees").EnumerateArray())
                    {
                        xmlWriter.WriteStartElement("employee");
                        xmlWriter.WriteElementString("id", employee.GetProperty("id").GetInt32().ToString());
                        xmlWriter.WriteElementString("name", employee.GetProperty("name").GetString());
                        xmlWriter.WriteElementString("position", employee.GetProperty("position").GetString());

                        foreach (var skill in employee.GetProperty("skills").EnumerateArray())
                        {
                            xmlWriter.WriteElementString("skills", skill.GetString());
                        }

                        xmlWriter.WriteStartElement("contact");
                        xmlWriter.WriteElementString("email", employee.GetProperty("contact").GetProperty("email").GetString());
                        xmlWriter.WriteElementString("phone", employee.GetProperty("contact").GetProperty("phone").GetString());
                        xmlWriter.WriteEndElement(); // contact

                        xmlWriter.WriteEndElement(); // employee
                    }
                    xmlWriter.WriteEndElement(); // employees

                    // Write departments
                    xmlWriter.WriteStartElement("departments");
                    foreach (var department in root.GetProperty("departments").EnumerateArray())
                    {
                        xmlWriter.WriteStartElement("department");
                        xmlWriter.WriteElementString("name", department.GetProperty("name").GetString());
                        xmlWriter.WriteElementString("location", department.GetProperty("location").GetString());

                        xmlWriter.WriteStartElement("head");
                        xmlWriter.WriteElementString("name", department.GetProperty("head").GetProperty("name").GetString());
                        xmlWriter.WriteElementString("position", department.GetProperty("head").GetProperty("position").GetString());
                        xmlWriter.WriteEndElement(); // head

                        xmlWriter.WriteStartElement("staff");
                        foreach (var staff in department.GetProperty("staff").EnumerateArray())
                        {
                            xmlWriter.WriteStartElement("member");
                            xmlWriter.WriteElementString("id", staff.GetProperty("id").GetInt32().ToString());
                            xmlWriter.WriteElementString("name", staff.GetProperty("name").GetString());
                            xmlWriter.WriteElementString("position", staff.GetProperty("position").GetString());
                            xmlWriter.WriteEndElement(); // member
                        }
                        xmlWriter.WriteEndElement(); // staff

                        xmlWriter.WriteEndElement(); // department
                    }
                    xmlWriter.WriteEndElement(); // departments

                    // End writing XML
                    xmlWriter.WriteEndElement(); // company
                    xmlWriter.WriteEndDocument();
                }
            }

            return sw.ToString();
        }
    }
}

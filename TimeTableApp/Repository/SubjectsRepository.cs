using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TimeTableApp.Models;

namespace TimeTableApp.Repository
{
    public class SubjectsRepository
    {
        private List<Subject> SubjectsList = new List<Subject>();

        public SubjectsRepository()
        {
            string filePath = GenerateDefaultFilePath();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return; 
            }
            LoadData(filePath);
        }

        private string GenerateDefaultFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subjects.xml");
        }

        private void LoadData(string filePath)
        {
            Console.WriteLine("Reading subjects data from file: " + filePath);

            try
            {
                XDocument xDocument = XDocument.Load(filePath);
                XElement root = xDocument.Element("Subjects");

                if (root != null && root.HasElements)
                {
                    foreach (var elem in root.Elements("Subject"))
                    {
                        Guid id;
                        string name = (string)elem.Attribute("Name");
                        bool specificRoom = false;
                        Guid roomId = Guid.Empty;

                        if (!Guid.TryParse((string)elem.Attribute("Id"), out id))
                        {
                            continue;
                        }
                        var specificRoomAttribute = elem.Attribute("SpecificRoom");
                        if (specificRoomAttribute != null)
                        {
                            bool.TryParse(specificRoomAttribute.Value, out specificRoom);
                        }
                        if (!Guid.TryParse((string)elem.Attribute("RoomId"), out roomId))
                        {
                            continue;
                        }
                        Subject s;
                        if (specificRoom)
                             s = new Subject(id, name);
                        else 
                             s = new Subject(id, name, roomId);
                        
                        SubjectsList.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading subjects data: " + ex.Message);
            }
        }

        public Guid GetSubjectGuid(string name)
        {
            foreach (Subject subject in SubjectsList)
            {
                if (subject.name == name)
                    return subject._id;
            }
            return Guid.Empty;
        }

        public void AddSubject(Subject subject)
        {
            SubjectsList.Add(subject);
        }

        public List<Subject> GetSubjects()
        {
            return SubjectsList;
        }
    }
}

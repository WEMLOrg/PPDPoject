using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TimeTableApp.Models;

namespace TimeTableApp.Repository
{
    public class TeachersRepository
    {
        private List<Teacher> TeachersList = new List<Teacher>();
        public TeachersRepository()
        {
            string filePath = GenerateDefaultFilePath();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                CreateNewXmlFile(filePath);
                return;
            }
            LoadData(filePath);
        }
        private void CreateNewXmlFile(string filePath)
        {
            XDocument newDocument = new XDocument(
                new XElement("Teachers")
            );

            newDocument.Save(filePath);
            Console.WriteLine("New Rooms.xml file created at: " + filePath);
        }

        private string GenerateDefaultFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Teachers.xml");
        }

        private void LoadData(string filePath)
        {
            Console.WriteLine("Reading teachers data from file: " + filePath);

            try
            {
                XDocument xDocument = XDocument.Load(filePath);
                XElement root = xDocument.Element("Teachers");

                if (root != null && root.HasElements)
                {
                    foreach (var elem in root.Elements("Teacher"))
                    {
                        string teacherName = (string)elem.Attribute("Name");
                        Guid Id;
                        if (!Guid.TryParse((string)elem.Attribute("Id"), out Id))
                        {
                            continue;
                        }

                        List<Guid> teacherSubjects = new List<Guid>();

                        XElement subjectsElement = elem.Element("Subjects");

                        if (subjectsElement != null && subjectsElement.HasElements)
                        {
                            foreach (var subjectElem in subjectsElement.Elements("Subject"))
                            {
                                Guid subjectId;
                                if (!Guid.TryParse((string)subjectElem.Attribute("SubjectId"), out subjectId))
                                {
                                    continue;
                                }
                                if (subjectId != Guid.Empty)
                                {
                                    teacherSubjects.Add(subjectId);
                                }
                            }
                        }
                        Teacher teacher = new Teacher(Id, teacherName, teacherSubjects);
                        TeachersList.Add(teacher);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading teachers data: " + ex.Message);
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            TeachersList.Add(teacher);
        }

        public List<Teacher> GetTeachers()
        {
            return TeachersList;
        }
    }
}

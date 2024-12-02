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
                CreateDefaultTeachersFile(filePath);
            }
            LoadData(filePath);
        }
        private void CreateNewXmlFile(string filePath)
        {
            XDocument newDocument = new XDocument(
                new XElement("Teachers")
            );

            newDocument.Save(filePath);
            Console.WriteLine("New Teachers.xml file created at: " + filePath);
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
                        Guid teacherId;
                        if (!Guid.TryParse((string)elem.Attribute("Id"), out teacherId))
                        {
                            continue; 
                        }

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
                        Teacher teacher = new Teacher(teacherId, teacherName, teacherSubjects);
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
        
        private void CreateDefaultTeachersFile(string filePath)
        {
            Console.WriteLine("Creating default teachers data file: " + filePath);

            XDocument xDocument = new XDocument(
                new XElement("Teachers",
                    new XElement("Teacher",
                        new XAttribute("Id", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"),   
                        new XAttribute("Name", "John Doe"),
                        new XElement("Subjects",
                            new XElement("Subject", new XAttribute("Name", "Mathematics"))
                        )
                    ),
                    new XElement("Teacher",
                        new XAttribute("Id", "d2a5316b-7242-431b-9abf-289f4bb6f831"),  
                        new XAttribute("Name", "Jane Smith"),
                        new XElement("Subjects",
                            new XElement("Subject", new XAttribute("Name", "Chemistry"))
                        )
                    ),
                    new XElement("Teacher",
                        new XAttribute("Id", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"),  
                        new XAttribute("Name", "Alice Johnson"),
                        new XElement("Subjects",
                            new XElement("Subject", new XAttribute("Name", "Physics"))
                        )
                    )
                )
            );

            try
            {
                xDocument.Save(filePath);
                Console.WriteLine("Teachers file created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating the default teachers file: " + ex.Message);
            }
        }

    }
}

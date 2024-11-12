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
        private SubjectsRepository SubjectsRepository;

        public TeachersRepository(SubjectsRepository subjectsRepository)
        {
            SubjectsRepository = subjectsRepository;
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
                        List<Guid> teacherSubjects = new List<Guid>();

                        XElement subjectsElement = elem.Element("Subjects");
                        if (subjectsElement != null && subjectsElement.HasElements)
                        {
                            foreach (var subjectElem in subjectsElement.Elements("Subject"))
                            {
                                string subjectName = (string)subjectElem.Attribute("Name");
                                Guid subjectId = SubjectsRepository.GetSubjectGuid(subjectName);
                                if (subjectId != Guid.Empty)
                                {
                                    teacherSubjects.Add(subjectId);
                                }
                                else
                                {
                                    Console.WriteLine($"Subject '{subjectName}' not found in SubjectsRepository.");
                                }
                            }
                        }
                        Teacher teacher = new Teacher(teacherName, teacherSubjects);
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

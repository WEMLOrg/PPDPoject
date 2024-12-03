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

        //    private void CreateDefaultTeachersFile(string filePath)
        //    {
        //        Console.WriteLine("Creating default teachers data file: " + filePath);

        //        XDocument xDocument = new XDocument(
        //            new XElement("Teachers",
        //                new XElement("Teacher",
        //                    new XAttribute("Id", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"),   
        //                    new XAttribute("Name", "John Doe"),
        //                    new XElement("Subjects",
        //                        new XElement("Subject", new XAttribute("Name", "Mathematics"))
        //                    )
        //                ),
        //                new XElement("Teacher",
        //                    new XAttribute("Id", "d2a5316b-7242-431b-9abf-289f4bb6f831"),  
        //                    new XAttribute("Name", "Jane Smith"),
        //                    new XElement("Subjects",
        //                        new XElement("Subject", new XAttribute("Name", "Chemistry"))
        //                    )
        //                ),
        //                new XElement("Teacher",
        //                    new XAttribute("Id", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"),  
        //                    new XAttribute("Name", "Alice Johnson"),
        //                    new XElement("Subjects",
        //                        new XElement("Subject", new XAttribute("Name", "Physics"))
        //                    )
        //                )
        //            )
        //        );

        //        try
        //        {
        //            xDocument.Save(filePath);
        //            Console.WriteLine("Teachers file created successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("An error occurred while creating the default teachers file: " + ex.Message);
        //        }
        //    }

        //}
        private void CreateDefaultTeachersFile(string filePath)
        {
            Console.WriteLine("Creating default teachers data file: " + filePath);

            XDocument xDocument = new XDocument(
                new XElement("Teachers",
                    new XElement("Teacher", new XAttribute("Id", "a1111111-1111-1111-1111-111111111111"), new XAttribute("Name", "Teacher_1"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111")))),
                    new XElement("Teacher", new XAttribute("Id", "a2222222-2222-2222-2222-222222222222"), new XAttribute("Name", "Teacher_2"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222")))),
                    new XElement("Teacher", new XAttribute("Id", "a3333333-3333-3333-3333-333333333333"), new XAttribute("Name", "Teacher_3"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333")))),
                    new XElement("Teacher", new XAttribute("Id", "a4444444-4444-4444-4444-444444444444"), new XAttribute("Name", "Teacher_4"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a4444444-4444-4444-4444-444444444444")))),
                    new XElement("Teacher", new XAttribute("Id", "h1234567-8901-2345-6789-012345678902"), new XAttribute("Name", "Teacher_5"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a5555555-5555-5555-5555-555555555555")))),
                    new XElement("Teacher", new XAttribute("Id", "i1234567-8901-2345-6789-012345678903"), new XAttribute("Name", "Teacher_6"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666")))),
                    new XElement("Teacher", new XAttribute("Id", "j1234567-8901-2345-6789-012345678904"), new XAttribute("Name", "Teacher_7"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777")))),
                    new XElement("Teacher", new XAttribute("Id", "k1234567-8901-2345-6789-012345678905"), new XAttribute("Name", "Teacher_8"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888")))),
                    new XElement("Teacher", new XAttribute("Id", "l1234567-8901-2345-6789-012345678906"), new XAttribute("Name", "Teacher_9"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999")))),
                    new XElement("Teacher", new XAttribute("Id", "m1234567-8901-2345-6789-012345678907"), new XAttribute("Name", "Teacher_10"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111")))),
                    new XElement("Teacher", new XAttribute("Id", "n1234567-8901-2345-6789-012345678908"), new XAttribute("Name", "Teacher_11"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "b2222222-2222-2222-2222-222222222222")))),
                    new XElement("Teacher", new XAttribute("Id", "o1234567-8901-2345-6789-012345678909"), new XAttribute("Name", "Teacher_12"), new XElement("Subjects", new XElement("Subject", new XAttribute("SubjectId", "b3333333-3333-3333-3333-333333333333")))),

                    new XElement("Teacher", new XAttribute("Id", "p1234567-8901-2345-6789-012345678910"), new XAttribute("Name", "Teacher_13"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111")),
                        new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "q1234567-8901-2345-6789-012345678911"), new XAttribute("Name", "Teacher_14"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222")),
                        new XElement("Subject", new XAttribute("SubjectId", "b2222222-2222-2222-2222-222222222222"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "r1234567-8901-2345-6789-012345678912"), new XAttribute("Name", "Teacher_15"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333")),
                        new XElement("Subject", new XAttribute("SubjectId", "b3333333-3333-3333-3333-333333333333"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "y1234567-8901-2345-6789-012345678919"), new XAttribute("Name", "Teacher_22"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111")),
                        new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "z1234567-8901-2345-6789-012345678920"), new XAttribute("Name", "Teacher_23"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "b2222222-2222-2222-2222-222222222222")),
                        new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "a21234567-8901-2345-6789-012345678921"), new XAttribute("Name", "Teacher_24"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "b3333333-3333-3333-3333-333333333333")),
                        new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "bb1234567-8901-2345-6789-012345678922"), new XAttribute("Name", "Teacher_25"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777")),
                        new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "cc1234567-8901-2345-6789-012345678923"), new XAttribute("Name", "Teacher_26"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777")),
                        new XElement("Subject", new XAttribute("SubjectId", "a5555555-5555-5555-5555-555555555555"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "dd1234567-8901-2345-6789-012345678924"), new XAttribute("Name", "Teacher_27"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "b6666666-6666-6666-6666-666666666666")),
                        new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "ee1234567-8901-2345-6789-012345678925"), new XAttribute("Name", "Teacher_28"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999")),
                        new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "ff1234567-8901-2345-6789-012345678926"), new XAttribute("Name", "Teacher_29"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888")),
                        new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888"))
                    )),
                    new XElement("Teacher", new XAttribute("Id", "gg1234567-8901-2345-6789-012345678927"), new XAttribute("Name", "Teacher_30"), new XElement("Subjects",
                        new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666")),
                        new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999"))
                    ))
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

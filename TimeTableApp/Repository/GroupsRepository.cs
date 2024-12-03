using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TimeTableApp.Models;

namespace TimeTableApp.Repository
{
    public class GroupsRepository
    {
        private List<Group> GroupsList = new List<Group>();

        public GroupsRepository()
        {
            string filePath = GenerateDefaultFilePath();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found, creating one: " + filePath);
                CreateDefaultGroupsFile(filePath);
            }
            LoadData(filePath);
        }

        private void CreateNewXmlFile(string filePath)
        {
            XDocument newDocument = new XDocument(
                new XElement("Groups")
            );

            newDocument.Save(filePath);
            Console.WriteLine("New Groups.xml file created at: " + filePath);
        }
        private string GenerateDefaultFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Groups.xml");
        }

        private void LoadData(string filePath)
        {
            Console.WriteLine("Reading groups data from file: " + filePath);

            try
            {
                XDocument xDocument = XDocument.Load(filePath);
                XElement root = xDocument.Element("Groups");

                if (root != null && root.HasElements)
                {
                    foreach (var elem in root.Elements("Group"))
                    {
                        Guid id;
                        int nrOfKids;
                        var necessarySubjects = new Dictionary<KeyValuePair<Guid, Guid>, int>();

                        if (!Guid.TryParse((string)elem.Attribute("Id"), out id))
                        {
                            continue; 
                        }

                        if (!int.TryParse((string)elem.Attribute("NrOfKids"), out nrOfKids))
                        {
                            continue; 
                        }

                        XElement subjectsElement = elem.Element("NecessarySubjects");
                        if (subjectsElement != null && subjectsElement.HasElements)
                        {
                            Console.WriteLine("aaa " + subjectsElement);
                            foreach (var subjectElem in subjectsElement.Elements("Subject"))
                            {
                                Guid subjectId, teacherId;
                                int hours;

                                if (!Guid.TryParse((string)subjectElem.Attribute("SubjectId"), out subjectId))
                                {
                                    Console.WriteLine("aaaIf1 " + subjectsElement);
                                    continue;
                                }
                                if (!Guid.TryParse((string)subjectElem.Attribute("TeacherId"), out teacherId))
                                {
                                    Console.WriteLine($"Invalid TeacherId: {teacherId}");
                                    continue;
                                }
                                if (!int.TryParse((string)subjectElem.Attribute("Hours"), out hours))
                                {
                                    Console.WriteLine("aaaIf3 " + subjectsElement);
                                    continue;
                                }

                                Console.WriteLine("Group " + id + " are subject " + subjectId + " cu teacher " + teacherId + " cu hours " + hours);
                                necessarySubjects[new KeyValuePair<Guid, Guid>(subjectId, teacherId)] = hours;

                            }
                        }
                           

                        Group group = new Group(id, nrOfKids, necessarySubjects);
                        GroupsList.Add(group);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading groups data: " + ex.Message);
            }
        }

        public Guid GetGroupGuidByNrOfKids(int nrOfKids)
        {
            var group = GroupsList.FirstOrDefault(g => g.nrOfKids == nrOfKids);
            return group?._id ?? Guid.Empty;
        }

        public void AddGroup(Group group)
        {
            GroupsList.Add(group);
        }

        public List<Group> GetGroups()
        {
            return GroupsList;
        }

        private void CreateDefaultGroupsFile(string filePath)
        {
            Console.WriteLine("Creating default groups data file: " + filePath);

            XDocument xDocument = new XDocument(
                new XElement("Groups",
                    new XElement("Group",
                        new XAttribute("Id", "f949b1f0-2718-487b-a1a0-c1b4729ac7a9"),
                        new XAttribute("NrOfKids", "30"),
                        new XElement("NecessarySubjects",
                            new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"), new XAttribute("TeacherId", "d2a5316b-7242-431b-9abf-289f4bb6f831"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333"), new XAttribute("TeacherId", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a4444444-4444-4444-4444-444444444444"), new XAttribute("TeacherId", "g1234567-8901-2345-6789-012345678901"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a5555555-5555-5555-5555-555555555555"), new XAttribute("TeacherId", "h1234567-8901-2345-6789-012345678902"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666"), new XAttribute("TeacherId", "i1234567-8901-2345-6789-012345678903"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777"), new XAttribute("TeacherId", "j1234567-8901-2345-6789-012345678904"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888"), new XAttribute("TeacherId", "k1234567-8901-2345-6789-012345678905"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999"), new XAttribute("TeacherId", "l1234567-8901-2345-6789-012345678906"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "m1234567-8901-2345-6789-012345678907"), new XAttribute("Hours", "1"))
                        )
                    ),
                    new XElement("Group",
                        new XAttribute("Id", "c87c9e5b-3a5e-4f0e-badd-990396d12be7"),
                        new XAttribute("NrOfKids", "25"),
                        new XElement("NecessarySubjects",
                            new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"), new XAttribute("TeacherId", "d2a5316b-7242-431b-9abf-289f4bb6f831"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333"), new XAttribute("TeacherId", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a4444444-4444-4444-4444-444444444444"), new XAttribute("TeacherId", "g1234567-8901-2345-6789-012345678901"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a5555555-5555-5555-5555-555555555555"), new XAttribute("TeacherId", "h1234567-8901-2345-6789-012345678902"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666"), new XAttribute("TeacherId", "i1234567-8901-2345-6789-012345678903"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777"), new XAttribute("TeacherId", "j1234567-8901-2345-6789-012345678904"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888"), new XAttribute("TeacherId", "k1234567-8901-2345-6789-012345678905"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999"), new XAttribute("TeacherId", "l1234567-8901-2345-6789-012345678906"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "m1234567-8901-2345-6789-012345678907"), new XAttribute("Hours", "1"))
                        )
                    ),
                    new XElement("Group",
                        new XAttribute("Id", "b33d6c3a-5c5f-4738-bfb9-f3ef77563a9b"),
                        new XAttribute("NrOfKids", "28"),
                        new XElement("NecessarySubjects",
                            new XElement("Subject", new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"), new XAttribute("TeacherId", "d2a5316b-7242-431b-9abf-289f4bb6f831"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333"), new XAttribute("TeacherId", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a4444444-4444-4444-4444-444444444444"), new XAttribute("TeacherId", "g1234567-8901-2345-6789-012345678901"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a5555555-5555-5555-5555-555555555555"), new XAttribute("TeacherId", "h1234567-8901-2345-6789-012345678902"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a6666666-6666-6666-6666-666666666666"), new XAttribute("TeacherId", "i1234567-8901-2345-6789-012345678903"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a7777777-7777-7777-7777-777777777777"), new XAttribute("TeacherId", "j1234567-8901-2345-6789-012345678904"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a8888888-8888-8888-8888-888888888888"), new XAttribute("TeacherId", "k1234567-8901-2345-6789-012345678905"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "a9999999-9999-9999-9999-999999999999"), new XAttribute("TeacherId", "l1234567-8901-2345-6789-012345678906"), new XAttribute("Hours", "1")),
                            new XElement("Subject", new XAttribute("SubjectId", "b1111111-1111-1111-1111-111111111111"), new XAttribute("TeacherId", "m1234567-8901-2345-6789-012345678907"), new XAttribute("Hours", "1"))
                        )
                    )
                )
            );
            xDocument.Save(filePath);

            Console.WriteLine("Groups data file created successfully at: " + filePath);
        }



    }
}

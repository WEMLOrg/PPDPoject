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
                return;
            }
            LoadData(filePath);
        }

        private void CreateNewXmlFile(string filePath)
        {
            XDocument newDocument = new XDocument(
                new XElement("Groups")
            );

            newDocument.Save(filePath);
            Console.WriteLine("New Rooms.xml file created at: " + filePath);
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
                            foreach (var subjectElem in subjectsElement.Elements("Subject"))
                            {
                                Guid subjectId, teacherId;
                                int hours;

                                if (!Guid.TryParse((string)subjectElem.Attribute("SubjectId"), out subjectId))
                                {
                                    continue;
                                }
                                if (!Guid.TryParse((string)subjectElem.Attribute("TeacherId"), out teacherId))
                                {
                                    continue; 
                                }
                                if (!int.TryParse((string)subjectElem.Attribute("Hours"), out hours))
                                {
                                    continue;
                                }
                                
                                necessarySubjects.Add(new KeyValuePair<Guid, Guid>(subjectId, teacherId), hours);

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
                new XAttribute("Id", "f949b1f0-2718-487b-a1a0-c1b4729ac7a9"),  // Valid GUID for Group 1
                new XAttribute("NrOfKids", 30),
                new XElement("NecessarySubjects",
                    new XElement("Subject",
                        new XAttribute("SubjectId", "a1111111-1111-1111-1111-111111111111"), // Mathematics
                        new XAttribute("TeacherId", "b9f7d214-bab4-46a9-95f2-2d8f1b08470e"), // Teacher John Doe
                        new XAttribute("Hours", 4)
                    ),
                    new XElement("Subject",
                        new XAttribute("SubjectId", "a2222222-2222-2222-2222-222222222222"), // Chemistry
                        new XAttribute("TeacherId", "d2a5316b-7242-431b-9abf-289f4bb6f831"), // Teacher Jane Smith
                        new XAttribute("Hours", 3)
                    )
                )
            ),
            new XElement("Group",
                new XAttribute("Id", "c87c9e5b-3a5e-4f0e-badd-990396d12be7"),  // Valid GUID for Group 2
                new XAttribute("NrOfKids", 25),
                new XElement("NecessarySubjects",
                    new XElement("Subject",
                        new XAttribute("SubjectId", "a3333333-3333-3333-3333-333333333333"), // Physics
                        new XAttribute("TeacherId", "f1d5a34b-497f-42d4-a72e-0198c1b40c29"), // Teacher Alice Johnson
                        new XAttribute("Hours", 5)
                    )
                )
            )
        )
    );

    try
    {
        xDocument.Save(filePath);
        Console.WriteLine("Groups file created successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while creating the default groups file: " + ex.Message);
    }
}


    }
}

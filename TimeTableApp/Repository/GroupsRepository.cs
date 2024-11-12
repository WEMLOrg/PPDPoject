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
                Console.WriteLine("File not found: " + filePath);
                return;
            }
            LoadData(filePath);
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
                                //necessarySubjects.Add(new Dictionary<new KeyValuePair<Guid, Guid>((subjectId, teacherId), hours);
                                necessarySubjects.Add(new KeyValuePair<Guid, Guid>(subjectId, teacherId), hours);

                                //necessarySubjects.Add(new KeyValuePair<Guid, Guid>(subjectId, teacherId), hours);

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
    }
}

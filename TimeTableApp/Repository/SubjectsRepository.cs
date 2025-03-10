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
                CreateDefaultSubjectsFile(filePath);
            }
            LoadData(filePath);
        }
        private void CreateNewXmlFile(string filePath)
        {
            XDocument newDocument = new XDocument(
                new XElement("Subjects")
            );

            newDocument.Save(filePath);
            Console.WriteLine("New Subjects.xml file created at: " + filePath);
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
                       // Console.WriteLine("in for : "+ name);
                        if (!Guid.TryParse((string)elem.Attribute("Id"), out id))
                        {
                           // Console.WriteLine("Subject in if1: " + id);
                            continue;
                        }
                        var specificRoomAttribute = elem.Attribute("SpecificRoom");
                        bool.TryParse(specificRoomAttribute.Value, out specificRoom);
                        
                       if (!Guid.TryParse((string)elem.Attribute("RoomId"), out roomId))
                            {
                                continue;
                            }
                        
                        Subject s;
                        if (specificRoom == false)
                            s = new Subject(id, name);
                        else
                            s = new Subject(id, name, roomId);
                        //Console.WriteLine(name + specificRoom.ToString());
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

        //    private void CreateDefaultSubjectsFile(string filePath)
        //    {
        //        Console.WriteLine("Creating default subjects data file: " + filePath);

        //        XDocument xDocument = new XDocument(
        //            new XElement("Subjects",
        //                new XElement("Subject",
        //                    new XAttribute("Id", "a1111111-1111-1111-1111-111111111111"),
        //                    new XAttribute("Name", "Mathematics"),
        //                    new XAttribute("SpecificRoom", "false"),  
        //                    new XAttribute("RoomId", "a16e8d98-5c24-4a37-824f-2dcfb5f246a3")  
        //                ),
        //                new XElement("Subject",
        //                    new XAttribute("Id", "a2222222-2222-2222-2222-222222222222"),
        //                    new XAttribute("Name", "Chemistry"),
        //                    new XAttribute("SpecificRoom", "true"),
        //                    new XAttribute("RoomId", "b59f16b7-e30c-4cde-8a3c-1f239fb367de") 
        //                ),
        //                new XElement("Subject",
        //                    new XAttribute("Id", "a3333333-3333-3333-3333-333333333333"),  
        //                    new XAttribute("Name", "Physics"),
        //                    new XAttribute("SpecificRoom", "false"),  
        //                    new XAttribute("RoomId", "c4038de8-a58e-46bc-8247-6a3e9ecf8471")  
        //                )
        //            )
        //        );

        //        try
        //        {
        //            xDocument.Save(filePath);
        //            Console.WriteLine("Subjects file created successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("An error occurred while creating the default subjects file: " + ex.Message);
        //        }
        //    }

        //}
        private void CreateDefaultSubjectsFile(string filePath)
{
    Console.WriteLine("Creating default subjects data file: " + filePath);

    XDocument xDocument = new XDocument(
        new XElement("Subjects",
            new XElement("Subject",
                new XAttribute("Id", "a1111111-1111-1111-1111-111111111111"),
                new XAttribute("Name", "Mathematics"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "a1111111-1111-1111-1111-111111111111") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a2222222-2222-2222-2222-222222222222"),
                new XAttribute("Name", "Chemistry"),
                new XAttribute("SpecificRoom", "true"),
                new XAttribute("RoomId", "b2222222-2222-2222-2222-222222222222") 
            ),
            new XElement("Subject",
                new XAttribute("Id", "a3333333-3333-3333-3333-333333333333"),
                new XAttribute("Name", "Physics"),
                new XAttribute("SpecificRoom", "true"),
                new XAttribute("RoomId", "c3333333-3333-3333-3333-333333333333") 
            ),
            new XElement("Subject",
                new XAttribute("Id", "a4444444-4444-4444-4444-444444444444"),
                new XAttribute("Name", "English"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "a1111111-1111-1111-1111-111111111112") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a5555555-5555-5555-5555-555555555555"),
                new XAttribute("Name", "Biology"),
                new XAttribute("SpecificRoom", "true"),
                new XAttribute("RoomId", "b2222222-2222-2222-2222-222222222223") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a6666666-6666-6666-6666-666666666666"),
                new XAttribute("Name", "History"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "c3333333-3333-3333-3333-333333333334") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a7777777-7777-7777-7777-777777777777"),
                new XAttribute("Name", "Geography"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "b2222222-2222-2222-2222-222222222223") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a8888888-8888-8888-8888-888888888888"),
                new XAttribute("Name", "German"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "b2222222-2222-2222-2222-222222222222") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "a9999999-9999-9999-9999-999999999999"),
                new XAttribute("Name", "French"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "b2222222-2222-2222-2222-222222222222") // Matches Room ID
            ),
            new XElement("Subject",
                new XAttribute("Id", "b1111111-1111-1111-1111-111111111111"),
                new XAttribute("Name", "Computer Science"),
                new XAttribute("SpecificRoom", "true"),
                new XAttribute("RoomId", "d1111111-1111-1111-1111-111111111111") // Matches Room ID
            ),
             new XElement("Subject",
                new XAttribute("Id", "b2222222-2222-2222-2222-222222222222"),
                new XAttribute("Name", "Psychology"),
                new XAttribute("SpecificRoom", "false"),
                new XAttribute("RoomId", "d1111111-1111-1111-1111-111111111111") // Matches Room ID
            ),
             new XElement("Subject",
                new XAttribute("Id", "b3333333-3333-3333-3333-333333333333"),
                new XAttribute("Name", "Sport"),
                new XAttribute("SpecificRoom", "true"),
                new XAttribute("RoomId", "d1111111-1111-1111-1111-111111111112") // Matches Room ID
            )


        )
    );

    try
    {
        xDocument.Save(filePath);
        Console.WriteLine("Subjects file created successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while creating the default subjects file: " + ex.Message);
    }
}


    }
}

using System;
using System.Collections.Generic;
using System.Linq; 
using TimeTableApp.Models;

namespace TimeTableApp.Repository
{
    public class TimeTableRepo
    {
        public List<TimetableEntry> list { get; set; }

        public TimeTableRepo()
        {
            list = new List<TimetableEntry>();
        }

        public TimeTableRepo(List<TimetableEntry> list)
        {
            this.list = list ?? new List<TimetableEntry>(); 
        }

        public void AddEntry(TimetableEntry timeTableEntry)
        {
            if (timeTableEntry == null)
                throw new ArgumentNullException(nameof(timeTableEntry), "Cannot add null entry.");

            list.Add(timeTableEntry);
        }

        public void DeleteEntry(TimetableEntry timeTableEntry)
        {
            if (timeTableEntry == null)
                throw new ArgumentNullException(nameof(timeTableEntry), "Cannot delete null entry.");

            list.Remove(timeTableEntry); 
        }

        public List<TimetableEntry> GetEntriesByTeacherId(Guid teacherId)
        {
            return list.Where(l => l.teacher._id == teacherId).ToList();
        }
        
        public List<TimetableEntry> GetEntries()
        {
            return list;
        }

        public List<TimetableEntry> GetEntriesBySubjectId(Guid subjectId)
        {
            return list.Where(l => l.subject._id == subjectId).ToList(); 
        }

        public List<TimetableEntry> GetEntriesByGroupId(Guid groupId)
        {
            return list.Where(l => l.group._id == groupId).ToList();
        }

        public List<TimetableEntry> GetEntriesByRoomId(Guid roomId)
        {
            return list.Where(l => l.room._id == roomId).ToList();
        }

        public void PrintToCSV(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Teacher,Subject,GroupID,RoomID,Day,Hour");
                    foreach (var entry in list)
                    {
                        string teacherName = entry.teacher?.teacherName ?? "Unknown";
                        string subjectName = entry.subject?.name ?? "Unknown";
                        string groupId = entry.group?._id.ToString() ?? "Unknown";
                        string roomId = entry.room?._id.ToString() ?? "Unknown";
                        string day = entry.day.ToString();
                        string hour = entry.hour.ToString();

                        writer.WriteLine($"{teacherName},{subjectName},{groupId},{roomId},{day},{hour}");
                    }
                }

                Console.WriteLine("Timetable successfully exported to CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while exporting to CSV: " + ex.Message);
            }
        }
    }
}

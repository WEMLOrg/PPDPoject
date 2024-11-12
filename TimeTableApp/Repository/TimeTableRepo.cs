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
    }
}

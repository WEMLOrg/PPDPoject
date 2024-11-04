using System;
using System.Collections.Generic;
using System.Linq; 
using TimeTableApp.Models;

namespace TimeTableApp.Repository
{
    public class TimeTableRepo
    {
        private List<TimeTableEntry> list { get; set; }

        public TimeTableRepo()
        {
            list = new List<TimeTableEntry>();
        }

        public TimeTableRepo(List<TimeTableEntry> list)
        {
            this.list = list ?? new List<TimeTableEntry>(); 
        }

        public void AddEntry(TimeTableEntry timeTableEntry)
        {
            if (timeTableEntry == null)
                throw new ArgumentNullException(nameof(timeTableEntry), "Cannot add null entry.");

            list.Add(timeTableEntry);
        }

        public void DeleteEntry(TimeTableEntry timeTableEntry)
        {
            if (timeTableEntry == null)
                throw new ArgumentNullException(nameof(timeTableEntry), "Cannot delete null entry.");

            list.Remove(timeTableEntry); 
        }

        public List<TimeTableEntry> GetEntriesByTeacherId(Guid teacherId)
        {
            return list.Where(l => l.teacher._id == teacherId).ToList();
        }

        public List<TimeTableEntry> GetEntriesBySubjectId(Guid subjectId)
        {
            return list.Where(l => l.subject._id == subjectId).ToList(); 
        }

        public List<TimeTableEntry> GetEntriesByGroupId(Guid groupId)
        {
            return list.Where(l => l.group._id == groupId).ToList();
        }

        public List<TimeTableEntry> GetEntriesByRoomId(Guid roomId)
        {
            return list.Where(l => l.room._id == roomId).ToList();
        }
    }
}

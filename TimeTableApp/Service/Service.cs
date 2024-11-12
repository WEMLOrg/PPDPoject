using TimeTableApp.Repository;
using TimeTableApp.Models;

namespace TimeTableApp
{
    class Service
    {
        public int startTime { get; set; }
        public int endTime { get; set; }
        public TeachersRepository teachersRepository { get; set; }
        public SubjectsRepository subjectsRepository { get; set; }
        public RoomsRepository roomsRepository { get; set; }
        public TimeTableRepo timeTableRepo { get; set; }


        public Service(TeachersRepository teacherRepo, SubjectsRepository subjectRepo, RoomsRepository roomRepo,  TimeTableRepo orarRepo, int startTime, int endTime)
        {
            teachersRepository = teacherRepo;
            subjectsRepository = subjectRepo;
            roomsRepository = roomRepo;
            timeTableRepo = orarRepo;
            startTime = startTime;
            endTime = endTime;
        }
        public void BackTracking ()
        {

        }
        public bool doesGroupFitInRoom(Room r, Group g)
        {
            if (r.capacity >= g.nrOfKids)
                return true;
            return false;
        }
        public bool isRoomAvailable(Room r, TimetableEntry.Days day, int hour)
        {
            foreach(TimetableEntry te in timeTableRepo.GetEntries())
            {
                if (te.day == day) 
                    if (te.hour == hour)
                        if (te.room == r)
                            return false;
            }
            return true;
        }
        public bool isSolution(TimetableEntry t) 
        {
            foreach (TimetableEntry te in timeTableRepo.GetEntries())
            {
                if (te == t)
                    return false;
                if (te.hour == t.hour && te.day == t.day)
                {
                    if (te.teacher == t.teacher || te.group == t.group || te.room == t.room)
                        return false;
                }
            }

            return true;
        }

        public bool doesGroupHaveAlreadyEnoughHoursForSubject(Subject s, Group g, Teacher t)
        {
            if (g.doesGroupHaveSubject(s._id, t._id))
            {
                int necessaryHours = g.necessarySubjects.GetValueOrDefault(new KeyValuePair<Guid, Guid>(s._id, t._id));
                foreach (TimetableEntry te in timeTableRepo.GetEntries())
                {
                    if (necessaryHours == 0)
                        return true;
                    if (te.subject == s && te.group == g)
                        necessaryHours--;
                }

                if (necessaryHours == 0)
                    return true;
            }

            return false;
        }
        public bool isSpecificRoomNeeded(Subject s)
        {
            if (s.specificRoom)
                return true;
            return false;
        }
        public bool isValid(Group g, Room r, Teacher t, Subject s, int h, TimetableEntry.Days d) 
        {
            if (doesGroupFitInRoom(r, g) && g.doesGroupHaveSubject(s._id, t._id) &&
                t.DoesTeacherTeachSubject(s._id) && isRoomAvailable(r, d, h) &&
                !doesGroupHaveAlreadyEnoughHoursForSubject(s, g, t))
                if (isSpecificRoomNeeded(s))
                    if (r._id != s.roomId)
                        return false; 
                    else
                        return true;

            return false;

        }
    }
}
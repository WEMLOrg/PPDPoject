using TimeTableApp.Repository;

namespace TimeTableApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int startTime=8, endTime=14;
            
            GroupsRepository groupsRepository = new GroupsRepository();
            SubjectsRepository subjectsRepository = new SubjectsRepository();
            TeachersRepository teachersRepository = new TeachersRepository();
            RoomsRepository roomsRepository = new RoomsRepository();
            TimeTableRepo timeTableRepo = new TimeTableRepo();
            
            Service service = new Service(groupsRepository, teachersRepository, subjectsRepository, roomsRepository, timeTableRepo, startTime, endTime);
            //service.BackTracking();
            service.GenerateTimetable();
            timeTableRepo.PrintToCSV("result.csv");
        }
    }
}
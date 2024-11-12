using TimeTableApp.Repository;

namespace TimeTableApp
{
    //Generate a school timetable through exhaustive search.
    class Program
    {
        static void Main(string[] args)
        {
            int startTime=8, endTime=14;
            
            SubjectsRepository subjectsRepository = new SubjectsRepository();
            TeachersRepository teachersRepository = new TeachersRepository(subjectsRepository);
            RoomsRepository roomsRepository = new RoomsRepository();
            TimeTableRepo timeTableRepo = new TimeTableRepo();
            GroupsRepository groupsRepository = new GroupsRepository();
            
            Service service = new Service(groupsRepository, teachersRepository, subjectsRepository, roomsRepository, timeTableRepo, startTime, endTime);
            service.BackTracking();

            timeTableRepo.PrintToCSV("result.csv");
            Console.WriteLine("hel");
        }
    }
}
namespace TimeTableApp
{
    class Service
    {
        public Repository repository {  get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }


        Service(Repository repo, int startTime, int endTime)
        {
            repository = repo;
            startTime = startTime;
            endTime = endTime;
        }
        public void BackTracking ()
        {

            foreach (var t)
        }
        public bool isValid() {  return true; }
    }
}
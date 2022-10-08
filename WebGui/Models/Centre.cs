namespace WebGui.Models
{
    public class Centre
    {
        public string CentreName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> BookStatus { get; set; }
        public string BookedUserName { get; set; }
    }
}

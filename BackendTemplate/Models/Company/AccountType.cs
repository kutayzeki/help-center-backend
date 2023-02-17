namespace HelpCenter.Models.Company
{
    public class AccountType
    {
        public Guid AccountTypeId { get; set; }
        public string Type { get; set; }
        public int MaxFeatureRequests { get; set; }
        public int MaxProducts { get; set; }
        public int MaxUsers { get; set; }
        public bool HasAnalytics { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Company> Companies { get; set; }
    }
}

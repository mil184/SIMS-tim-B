namespace InitialProject.Model.DTO
{
    public class OwnerCommentDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string UserInfo { get; set; }
        public int ForumId { get; set; }
        public bool WasPresent { get; set; }
        public int ReportCount { get; set; }

        public OwnerCommentDTO() { }
        public OwnerCommentDTO(int id, string comment, string username, int forumId, bool wasPresent, int reportCount)
        {
            Id = id;
            Comment = comment;
            UserInfo = username;
            ForumId = forumId;
            WasPresent = wasPresent;
            ReportCount = reportCount;
        }
    }
}

namespace SAPB1SLayerWebAPI.Models.SLayer
{
    public class SLApprovalRequest
    {
        public int Code { get; set; }
        public int ApprovalTemplatesID { get; set; }
        public string ObjectType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public int CurrentStage { get; set; }
        public int OriginatorID { get; set; }
        public int DraftEntry { get; set; }
        public List<ApprovalRequestLine> ApprovalRequestLines { get; set; } = [];
    }

    public class ApprovalRequestLine
    {
        public int StageCode { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}

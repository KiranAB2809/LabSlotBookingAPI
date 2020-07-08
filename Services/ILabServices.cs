using Models;
using System.Collections.Generic;

namespace Services
{
    public interface ILabServices
    {
        List<LabList> GetLabList();
        bool GetLabSlots(LabModel labModel);
        string InsertLabSLot(LabModel labModel);
        string ApproveLabSlot(int bookingid, bool approved);
        List<ApprovalList> ApprovalSlots();
        void DeleteLabSlot();
        void WeeklyReportJob();
    }
}
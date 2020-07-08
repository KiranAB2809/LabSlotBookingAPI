using Models;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface ILabInfrastructure
    {
        List<LabList> GetLabList();
        bool GetLabSlots(LabModel labModel);
        string InsertLabSLot(LabModel labModel);
        string ApproveLabSlot(int bookingid, bool approved);
        List<ApprovalList> ApprovalSlots();
    }
}
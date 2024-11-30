using TaskMasterDetails.DTO;

namespace TaskMasterDetails.services.contarcts
{
    public interface IMasterService
    {

        Task<int> CreateMasterDetail(CreateMasterDetailDTO dto);
        Task<bool> DeleteMasterDetail(int id);
        Task<bool> UpdateMasterDetail(UpdateMasterDetailDTO dto);
        Task<RetrieveMasterDetailDTO> GetMasterDetailById(int id);
        Task<IEnumerable<RetrieveMasterDetailDTO>> GetAllMasterDetails();
    }
}

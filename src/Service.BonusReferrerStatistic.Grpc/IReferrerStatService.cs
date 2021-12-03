using System.ServiceModel;
using System.Threading.Tasks;
using Service.BonusReferrerStatistic.Grpc.Models;

namespace Service.BonusReferrerStatistic.Grpc
{
    [ServiceContract]
    public interface IReferrerStatService
    {
        [OperationContract]
        Task<ReferrerStatResponse> GetReferrerStats(GetStatRequest request);
    }
}
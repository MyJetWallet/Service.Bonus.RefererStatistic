using System.ServiceModel;
using System.Threading.Tasks;
using Service.BonusReferrerStatistic.Grpc.Models;

namespace Service.BonusReferrerStatistic.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}
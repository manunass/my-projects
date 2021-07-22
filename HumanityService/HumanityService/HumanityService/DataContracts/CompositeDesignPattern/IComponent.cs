using HumanityService.Stores.Interfaces;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public interface IComponent
    {
        public string Id { get; set; }   
        public string Status { get; set; }

        Task Save();
        Task Update();
        void SetStore(ITransactionStore transactionStore);
        Task Cancel();
    }
}

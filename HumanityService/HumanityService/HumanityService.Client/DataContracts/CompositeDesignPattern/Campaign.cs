using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanityService.DataContracts
{
    public class Campaign : IComponent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NgoName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int Target { get; set; }
        public int CompletedCount { get; set; }
        public long TimeCreated { get; set; }
        public long TimeCompleted { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
    }
}

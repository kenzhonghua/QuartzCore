using Quartz;

namespace QuartzCore.WebApi.Models
{
    public class KeyModel
    {
        public KeyModel(JobKey jobKey)
        {
            Name = jobKey.Name;
            Group = jobKey.Group;
        }

        public KeyModel(TriggerKey triggerKey)
        {
            Name = triggerKey.Name;
            Group = triggerKey.Group;
        }

        public string Name { get; private set; }
        public string Group { get; private set; }
    }
}

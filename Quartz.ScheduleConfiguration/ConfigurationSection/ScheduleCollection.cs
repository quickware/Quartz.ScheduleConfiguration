using System.Configuration;

namespace Quartz.ScheduleConfiguration.ConfigurationSection
{
    [ConfigurationCollection(typeof(ScheduleElement), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class ScheduleCollection : ConfigurationElementCollection
    {
        protected override string ElementName { get; } = "Schedule";
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMapAlternate;
        protected override ConfigurationElement CreateNewElement() => new ScheduleElement();
        protected override object GetElementKey(ConfigurationElement element) =>  ((ScheduleElement)element).Name;
    }
}
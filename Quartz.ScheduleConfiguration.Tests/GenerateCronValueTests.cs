using System;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Quartz.ScheduleConfiguration.ConfigurationSection;
using Shouldly;

namespace Quartz.ScheduleConfiguration.Tests
{
    [TestFixture]
    public class GenerateCronValueTests
    {
        private const string TestSpecificConfig = @"Samples\TestSpecific.config";
        private const string SuccessExampleConfig = @"Samples\SuccessExample.config";

        private static DailySchedules GetScheduleConfig(string configFile)
        {
            configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return configuration.GetSection("DailySchedules") as DailySchedules;
        }

        [Test]
        public void GenerateCronValue_GivenValidSchedule_ShouldProduceValidCronString()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("Twelve");

            var parts = cron.Split(new[] { ' ' });
            parts.Count().ShouldBe(7);
            parts[3].ShouldBe("*");
            parts[4].ShouldBe("*");
            parts[5].ShouldBe("?");
            parts[6].ShouldBe("*");
        }

        [Test]
        public void GenerateCronValue_CheckingHoursValue_GivenListWithOneItem_ShouldUseSingleValueSyntax()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("Twelve");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            hours.ShouldBe("12");
        }

        [Test]
        public void GenerateCronValue_CheckingHoursValue_GivenListWithMultipleItems_ShouldUseMultipleValueSyntax()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("TwelveThirteen");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            hours.ShouldBe("12,13");
        }

        [Test]
        public void GenerateCronValue_CheckingHoursValue_GivenRange_ShouldUseRangeSyntax()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("FourToTen");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            hours.ShouldBe("4-10");
        }

        [Test]
        public void GenerateCronValue_CheckingHoursValue_GivenAll_ShouldUseWildcardSyntax()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("AllHours");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            hours.ShouldBe("*");
        }

        [Test]
        public void GenerateCronValue_CheckingHoursValue_GivenInterval_ShouldUseIntervalSyntax()
        {
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("TwoFromThree");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            hours.ShouldBe("3/2");
        }

        [Test]
        public void GenerateCronValue_CheckingAllTimeValues_GivenDifferentUnitValues_ShouldCreateExpectedCron()
        {
            //Trigger: Every odd hour after and including 3am, at the hour and half past the hour, on every second between 10 and 14
            var config = GetScheduleConfig(TestSpecificConfig);
            var cron = config.GetCronValue("Complex");

            var hours = cron.Split(new[] { ' ' }).ElementAt(2);
            var minutes = cron.Split(new[] { ' ' }).ElementAt(1);
            var seconds = cron.Split(new[] { ' ' }).ElementAt(0);
            hours.ShouldBe("3/2");
            minutes.ShouldBe("0,30");
            seconds.ShouldBe("10-14");
        }

        [TestCase(0), TestCase(1), TestCase(2), TestCase(3)]
        public void GenerateCronValue_GivenConfigurationFromXml_ShouldConvertToExpectedCronString(int index)
        {
            var scheduleElement = GetScheduleConfig(SuccessExampleConfig).Items
                .Cast<ScheduleElement>().ElementAt(index);
            var expectedCron = scheduleElement.Name;

            var config = GetScheduleConfig(SuccessExampleConfig);
            var actualCron = config.GetCronValue(expectedCron);
            
            actualCron.ShouldBe(expectedCron);
        }
    }
}

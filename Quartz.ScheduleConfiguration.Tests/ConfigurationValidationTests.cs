using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using Quartz.ScheduleConfiguration.ConfigurationSection;

namespace Quartz.ScheduleConfiguration.Tests
{
    [TestFixture]
    public class ConfigurationValidationTests
    {

        private const string BadExampleConfig = @"Samples\BadExample.";
        private const string NoNameExampleConfig = @"Samples\NoNameExample.config";
        private const string MissingMinuteConfig = @"Samples\Missing.Minutes.config";
        private const string MissingSecondConfig = @"Samples\Missing.Seconds.config";
        private const string MissingHourConfig = @"Samples\Missing.Hours.config";


        private static void LoadScheduleConfig(string configFile)
        {
            configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            var section = configuration.GetSection("DailySchedules") as DailySchedules;
            if (section == null)
                throw new NullReferenceException("section not found.");
        }

        private void LoadSchedule(string configFilePrefix, string name)
        {
            LoadScheduleConfig(configFilePrefix + name + ".config");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "Required attribute 'Name' not found", MatchType = MessageMatch.StartsWith)]
        public void ConfigurationValidation_ScheduleWithNoName_ShouldThrowImmediately()
        {
            LoadScheduleConfig(NoNameExampleConfig);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "The value of the property 'Type' cannot be parsed", MatchType = MessageMatch.StartsWith)]
        public void ConfigurationValidation_BadScheduleType_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "BadEnum");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "The value for the property 'Values' is not valid", MatchType = MessageMatch.StartsWith)]
        public void ConfigurationValidation_BadValues_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "BadValue");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "The value of the property 'From' cannot be parsed", MatchType = MessageMatch.StartsWith)]
        public void ConfigurationValidation_BadRange_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "BadRange");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "property From should be set and be a positive number", MatchType = MessageMatch.Contains)]
        public void ConfigurationValidation_ConfusedRange_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "ConfusedRange");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "The value of the property 'Start' cannot be parsed", MatchType = MessageMatch.StartsWith)]
        public void ConfigurationValidation_BadInterval_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "BadInterval");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "property Start should be set and be a positive number", MatchType = MessageMatch.Contains)]
        public void ConfigurationValidation_ConfusedInterval_ShouldThrowImmediately()
        {
            LoadSchedule(BadExampleConfig, "ConfusedInterval");
        }

        [TestCase(MissingSecondConfig)]
        [TestCase(MissingMinuteConfig)]
        [TestCase(MissingHourConfig)]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "Required attribute '[A-Za-z]+' not found", MatchType = MessageMatch.Regex)]
        public void ConfigurationValidation_MissingChildElement_ShouldThrowWhenLoadingSchedule(string configFile)
        {
            LoadScheduleConfig(configFile);
        }
    }
}

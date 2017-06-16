// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure.OData;
using SSModels = Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure;

namespace StorSimple8000Series.Tests
{
    public static partial class Helpers
    {
        public static IPage<Alert> GetAlertsBySeverity(StorSimple8000SeriesTestBase testBase, AlertSeverity severity)
        {
            var minTime = DateTime.MinValue;
            var maxTime = DateTime.Now;

            Expression<Func<AlertFilter, bool>> filterExp = filter =>
                filter.AppearedOnTime >= minTime &&
                filter.AppearedOnTime <= maxTime &&
                filter.Severity == severity;

            var alertFilters = new ODataQuery<AlertFilter>(filterExp);
            alertFilters.Top = 100;

            var alerts = Helpers.GetAlertsByFilters(testBase, alertFilters);

            Assert.True(alerts.Any(), "No Alerts found");

            return alerts;
        }

        public static IPage<Alert> GetActiveAlertsForDevice(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var minTime = DateTime.MinValue;
            var maxTime = DateTime.Now;

            Expression<Func<AlertFilter, bool>> filterExp = filter =>
                filter.Status == AlertStatus.Active &&
                filter.AppearedOnTime >= minTime &&
                filter.AppearedOnTime <= maxTime &&
                filter.SourceType == AlertSourceType.Device &&
                filter.SourceName == deviceName;

            var alertFilters = new ODataQuery<AlertFilter>(filterExp);
            alertFilters.Top = 100;

            var alerts = Helpers.GetAlertsByFilters(testBase, alertFilters);

            Assert.True(alerts.Any(), "No Active Alerts found");

            return alerts;
        }

        public static IPage<Alert> GetClearedAlerts(StorSimple8000SeriesTestBase testBase)
        {
            var minTime = DateTime.MinValue;
            var maxTime = DateTime.Now;
            Expression<Func<AlertFilter, bool>> filterExp = filter =>
                filter.Status == AlertStatus.Cleared &&
                filter.AppearedOnTime >= minTime &&
                filter.AppearedOnTime <= maxTime;

            var alertFilters = new ODataQuery<AlertFilter>(filterExp);
            alertFilters.Top = 100;

            var alerts = Helpers.GetAlertsByFilters(testBase, alertFilters);

            Assert.True(alerts.Any(), "No Cleared Alerts found");

            return alerts;
        }

        private static IPage<Alert> GetAlertsByFilters(StorSimple8000SeriesTestBase testBase, ODataQuery<AlertFilter> filters)
        {
            return testBase.Client.Alerts.ListByManager(testBase.ResourceGroupName, testBase.ManagerName, filters);
        }

        public static void ClearAlert(StorSimple8000SeriesTestBase testBase, string alertId)
        {
            var clearAlertRequest = new ClearAlertRequest()
            {
                Alerts = new List<string>()
                    {
                        alertId
                    }
            };

            testBase.Client.Alerts.Clear(clearAlertRequest, testBase.ResourceGroupName, testBase.ManagerName);
        }
    }
}
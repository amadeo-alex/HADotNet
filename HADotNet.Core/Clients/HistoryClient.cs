using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HADotNet.Core.Models;

namespace HADotNet.Core.Clients
{
    /// <summary>
    /// Provides access to the history API for retrieving and querying for historical state information.
    /// </summary>
    public sealed class HistoryClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryClient" />.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient" /> preconfigured to communicate with a Home Assistant instance.</param>
        public HistoryClient(HttpClient client) : base(client) { }

        /// <summary>
        /// Retrieves a list of historical states for the specified <paramref name="entityId" /> for the specified time range, from <paramref name="startDate" /> to <paramref name="endDate" />.
        /// </summary>
        /// <param name="entityId">The entity ID to filter on.</param>
        /// <param name="startDate">The earliest history entry to retrieve.</param>
        /// <param name="endDate">The most recent history entry to retrieve.</param>
        /// <returns>A <see cref="HistoryList"/> of history snapshots for the specified <paramref name="entityId" />, from <paramref name="startDate" /> to <paramref name="endDate" />.</returns>
        public async Task<HistoryList> GetHistory(string entityId, DateTimeOffset startDate, DateTimeOffset endDate) => (await Get<List<HistoryList>>($"/api/history/period/{startDate.UtcDateTime:yyyy-MM-dd\\THH:mm:ss\\+00\\:00}?filter_entity_id={entityId}&end_time={Uri.EscapeDataString(endDate.UtcDateTime.ToString("yyyy-MM-dd\\THH:mm:ss\\+00\\:00"))}")).FirstOrDefault();

        /// <summary>
        /// Retrieves a list of historical states for the specified <paramref name="entityId" /> for the past 1 day.
        /// </summary>
        /// <param name="entityId">The entity ID to retrieve state history for.</param>
        /// <returns>A <see cref="HistoryList"/> representing a 24-hour history snapshot for the specified <paramref name="entityId" />.</returns>
        public async Task<HistoryList> GetHistory(string entityId) => (await Get<List<HistoryList>>($"/api/history/period?filter_entity_id={entityId}")).FirstOrDefault();
    }
}

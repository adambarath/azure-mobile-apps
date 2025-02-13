﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Datasync.Client.Extensions;
using Microsoft.Datasync.Client.Offline;
using Microsoft.Datasync.Client.Query;
using Microsoft.Datasync.Client.Table;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Datasync.Client
{
    /// <summary>
    /// A set of extension methods for the <see cref="IOfflineTable"/> and <see cref="IOfflineTable{T}"/> classes.
    /// </summary>
    public static class IOfflineTableExtensions
    {
        /// <summary>
        /// Pull all items from the remote table to the offline table.
        /// </summary>
        /// <param name="table">The table reference.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that completes when the pull operation is complete.</returns>
        public static Task PullItemsAsync(this IOfflineTable table, CancellationToken cancellationToken = default)
           => table.PullItemsAsync(string.Empty, new PullOptions(), cancellationToken);

        /// <summary>
        /// Pull all items from the remote table to the offline table.
        /// </summary>
        /// <param name="table">The table reference.</param>
        /// <param name="options">The pull options to use.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that completes when the pull operation is complete.</returns>
        public static Task PullItemsAsync(this IOfflineTable table, PullOptions options, CancellationToken cancellationToken = default)
            => table.PullItemsAsync(string.Empty, options, cancellationToken);

        /// <summary>
        /// Pull all items matching the OData query string from the remote table to the offline table.
        /// </summary>
        /// <param name="table">The table reference.</param>
        /// <param name="query">The OData query string.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that completes when the pull operation is complete.</returns>
        public static Task PullItemsAsync(this IOfflineTable table, string query, CancellationToken cancellationToken = default)
            => table.PullItemsAsync(query, new PullOptions(), cancellationToken);

        /// <summary>
        /// Pull all items matching the LINQ query from the remote table to the offline table.
        /// </summary>
        /// <param name="table">The table reference.</param>
        /// <param name="query">The LINQ query.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that completes when the pull operation is complete.</returns>
        public static Task PullItemsAsync<T, U>(this IOfflineTable<T> table, ITableQuery<U> query, CancellationToken cancellationToken = default)
            => table.PullItemsAsync(query, new PullOptions(), cancellationToken);

        /// <summary>
        /// Count the number of items that would be returned from the table without returning all the values.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that returns the number of items that will be in the result set when the query finishes.</returns>
        public static Task<long> CountItemsAsync<T>(this IOfflineTable<T> table, CancellationToken cancellationToken = default)
            => table.CountItemsAsync(table.CreateQuery(), cancellationToken);

        /// <summary>
        /// Creates an array from the result of a table query.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An array of the results.</returns>
        public static ValueTask<TSource[]> ToArrayAsync<TSource>(this IOfflineTable<TSource> table, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoArrayAsync(cancellationToken);

        /// <summary>
        /// Returns the data set as an <see cref="AsyncPageable{T}"/> set.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <returns>The result set as an <see cref="AsyncPageable{T}"/></returns>
        public static AsyncPageable<TSource> ToAsyncPageable<TSource>(this IOfflineTable<TSource> table)
            => (AsyncPageable<TSource>)table.ToAsyncEnumerable();

        /// <summary>
        /// Creates a dictionary from the result of a table query according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A dictionary mapping unique key values onto the corresponding result's element.</returns>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IOfflineTable<TSource> table, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default) where TKey : notnull
            => table.ToAsyncEnumerable().ToZumoDictionaryAsync(keySelector, cancellationToken);

        /// <summary>
        /// Creates a dictionary from the result of a table query according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A dictionary mapping unique key values onto the corresponding result's element.</returns>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IOfflineTable<TSource> table, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
            => table.ToAsyncEnumerable().ToZumoDictionaryAsync(keySelector, comparer, cancellationToken);

        /// <summary>
        /// Converts the result of a table query to an enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <returns>The enumerable sequence containing the elements in the result set.</returns>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IOfflineTable<TSource> table)
            => table.ToAsyncEnumerable().ToZumoEnumerable();

        /// <summary>
        /// Creates a hash set from the results of a table query.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A hash set containing all the elements of the source sequence.</returns>
        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IOfflineTable<TSource> table, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoHashSetAsync(cancellationToken);

        /// <summary>
        /// Creates a hash set from the results of a table query.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="comparer">An equality comparer to compare elements of the sequence.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A hash set containing all the elements of the source sequence.</returns>
        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IOfflineTable<TSource> table, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoHashSetAsync(comparer, cancellationToken);

        /// <summary>
        /// Creates a list from the result set of the table query.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A list containing all the elements of the source sequence.</returns>
        public static ValueTask<List<TSource>> ToListAsync<TSource>(this IOfflineTable<TSource> table, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoListAsync(cancellationToken);

        /// <summary>
        /// Creates a <see cref="ConcurrentObservableCollection{T}"/> from the result set of the table query
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A <see cref="ConcurrentObservableCollection{T}"/> containing all the elements of the source sequence.</returns>
        public static ValueTask<ConcurrentObservableCollection<TSource>> ToObservableCollection<TSource>(this IOfflineTable<TSource> table, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoObservableCollectionAsync(cancellationToken);

        /// <summary>
        /// Updates a <see cref="ConcurrentObservableCollection{T}"/> from the result set of the table query.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="table">The source table.</param>
        /// <param name="collection">The <see cref="ConcurrentObservableCollection{T}"/> to update.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>The <see cref="ConcurrentObservableCollection{T}"/> passed in containing all the elements of the source sequence (replacing the old content).</returns>
        public static ValueTask<ConcurrentObservableCollection<TSource>> ToObservableCollection<TSource>(this IOfflineTable<TSource> table, ConcurrentObservableCollection<TSource> collection, CancellationToken cancellationToken = default)
            => table.ToAsyncEnumerable().ToZumoObservableCollectionAsync(collection, cancellationToken);
    }
}

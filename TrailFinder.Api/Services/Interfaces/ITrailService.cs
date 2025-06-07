namespace TrailFinder.Api.Services.Interfaces;

public interface ITrailService
{
    /// <summary>
    /// Retrieves a collection of trails asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of trails.</returns>
    Task<IEnumerable<Trail>> GetTrailsAsync();

    /// <summary>
    /// Retrieves a trail asynchronously based on the provided slug.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the trail.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the trail associated with the given slug, or null if no trail is found.</returns>
    Task<Trail?> GetTrailBySlugAsync(string slug);

    /// <summary>
    /// Creates a new trail asynchronously.
    /// </summary>
    /// <param name="trail">The trail object to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created trail.</returns>
    Task<Trail> CreateTrailAsync(Trail trail);

    /// <summary>
    /// Updates an existing trail asynchronously using the provided trail information and identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the trail to be updated.</param>
    /// <param name="trail">The updated trail object with new data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated trail, or null if no trail is found for the given identifier.</returns>
    Task<Trail?> UpdateTrailAsync(string id, Trail trail);

    /// <summary>
    /// Deletes a trail asynchronously based on the provided identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the trail to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the trail is successfully deleted, or false if no trail is found for the given identifier.</returns>
    Task<bool> DeleteTrailAsync(string id);
}

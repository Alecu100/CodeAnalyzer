namespace CodeEvaluator.UserInterface.Interfaces
{
    public interface IXamlResourcesRepository
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Finds the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        object FindResource(object resourceKey);

        #endregion
    }
}
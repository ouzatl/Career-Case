namespace Career.Common.Mapper
{
    public interface IMapping
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
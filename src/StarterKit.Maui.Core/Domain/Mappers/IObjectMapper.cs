namespace StarterKit.Maui.Core.Domain.Mappers;

public interface IObjectMapper<TFrom, TTo>
{
    TTo Map(TFrom source);

    TFrom Map(TTo source);
}
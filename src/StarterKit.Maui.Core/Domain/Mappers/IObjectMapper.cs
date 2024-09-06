namespace StarterKit.Maui.Core.Domain.Mappers;

public interface IObjectMapper<in TFrom, out TTo>
{
    TTo Map(TFrom from);
}
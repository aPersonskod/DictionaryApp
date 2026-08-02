using DictionaryApp.Application.Dtos;
using DictionaryApp.Domain.Exceptions;
using DictionaryApp.Domain.Models;

namespace DictionaryApp.Application.Mappings;

public static class MappingsExtension
{
    public static EntryDto ToDto(this Entry? entry)
    {
        if (entry == null) throw new NotFoundException("Entry is null");
        return new EntryDto
        {
            Id = entry.Id,
            Word = entry.Word,
            Translate = entry.Translate
        };
    }
}
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class EnumValueConverter<TFrom, TEnum>(
    Func<TFrom, TEnum> toEnum,
    Func<TEnum, TFrom> fromEnum) 
    : ValueConverter<TFrom, string>(
        x => toEnum(x).ToString(),
        x => fromEnum(Enum.Parse<TEnum>(x.ToString()))) 
    where TEnum : struct, Enum;